using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FeiLongService
{
    public partial class FeiLongService : ServiceBase
    {
        public FeiLongService()
        {
            InitializeComponent();
        }

        private readonly string connectstring = "Data Source=(local);Initial Catalog=FeiLong;User ID=sa;Password=123456;";

        private readonly string WXMessageSecret = "fEELZQOmP1Js1scahtwNT8KwalscO2WXC12JuG7n5ng";

        private readonly string WXMessageAgentID = "1000002";

        private readonly string WXCorpid = "ww50a89c9a5fbcc901";

        //private static DateTime Nowdate = DateTime.Now;
        protected override void OnStart(string[] args)
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Send;
        }

        void Send(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                DataTable dt = DoSqlDT("select * from [FL_TaskFlow] where RemindDate>='" + DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' and RemindDate<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and [IsReminded]=0");
                DataTable userdt = DoSqlDT("select * from FL_SysUser");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string userid = dr["MaintainUserID"].ToString();
                        DataRow[] userdr = userdt.Select("ID='" + userid + "'");
                        string message = "";
                        if (dr["IsAudit"] != null || Convert.ToBoolean(dr["IsAudit"]))
                        {
                            message = "提示:您有需要审核的表单:";
                        }
                        else
                        {
                            message = "提示:您有需要填写的表单:";
                        }
                        if (userdr.Length > 0)
                        {
                            //Fa送通知
                            Send(userdr[0]["WXID"].ToString(), message);
                        }


                    }
                }
            }
            catch
            {

            }
        }

        protected override void OnStop()
        {

        }


        private DataTable DoSqlDT(string sql)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                conn.Open();
                adapter.Fill(dt);
                adapter.Dispose();
                conn.Close();
            }

            return dt;
        }

        private void DoSql(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
            }
        }

        private void Send(string WXID, string Message)
        {
            try
            {
                string accesstoken = GetAccessToken(WXMessageSecret);
                //发送
                SendMessage(accesstoken, WXID, Message, WXMessageAgentID);
            }
            catch
            {

            }
        }


        private string SendMessage(string accesstoken, string userlist, string message, string secretid)
        {
            string postUrl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + accesstoken;

            //string mediaid = this.POSTfile(accesstoken, "image", System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/Resource/SiteImage/1.jpg");

            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append("{");
            sBuilder.Append("\"touser\":  \"" + userlist + "\",");
            sBuilder.Append("\"msgtype\":  \"text\",");
            sBuilder.Append("\"agentid\":  \"" + secretid + "\",");
            //sBuilder.Append("\"mpnews\":  {\"title\": \"" + article.Title + "\",\"content\" :\"" + article.Content + "\",\"thumb_media_id\": \"" + mediaid + "\",\"author\":\"" + db.SysUser.FirstOrDefault(t => t.ID == article.UserID).RealName + "\" ,\"content_source_url\":\"" + ParaUtils.SitePath + "/show.html?Index=" + article.ID + "\"}");
            sBuilder.Append("\"text\":{\"content\" :\"" + message + "\" }");

            sBuilder.Append("}");

            string jsonStr = sBuilder.ToString();

            byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "text/xml";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            //声明一个HttpWebRequest请求  
            request.Timeout = 90000;
            //设置连接超时时间  
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(strResult);

            return ja["errmsg"].ToString();
        }

        private string GetData(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }

        private string GetAccessToken(string secret)
        {

            string accessget = GetData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", WXCorpid, secret));

            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(accessget);

            return ja["access_token"].ToString();
        }


    }
}
