using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using System.Web;
using IFMPLibrary.Utils;
using Newtonsoft.Json.Linq;

namespace IFMPLibrary.DAO
{
    public class NoticeDAO
    {
        //https://open-doc.dingtalk.com/microapp/serverapi2/ye8tup
        public void SendDDNotice(string userlist, string url, string pcurl, string head, string title, string content, List<KeyValuePair<string, string>> formlist = null)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                JObject msg = new JObject();
                msg.Add("msgtype", "oa");

                JObject oa = new JObject();
                oa.Add("message_url", url);
                //先不做
                //oa.Add("pc_message_url", pcurl);

                JObject headobject = new JObject();
                headobject.Add("bgcolor", "FF009ACD");
                headobject.Add("text", head);
                oa.Add("head", headobject);

                JObject bodyobject = new JObject();
                bodyobject.Add("title", title);
                bodyobject.Add("content", content);
                if (formlist != null && formlist.Count > 0)
                {
                    JArray formobject = new JArray();

                    foreach (KeyValuePair<string, string> formdata in formlist)
                    {
                        JObject formdataobject = new JObject();
                        formdataobject.Add("key", formdata.Key);
                        formdataobject.Add("value", formdata.Value);
                        formobject.Add(formdataobject);
                    }

                    bodyobject.Add("form", formobject);
                }
                bodyobject.Add("image", ParaUtils.AlertImageID);

                oa.Add("body", bodyobject);

                msg.Add("oa", oa);

                string accesstoken = new DDUtils().GetAccessToken();
                new DDUtils().SendMessage(msg, userlist, accesstoken);
                //日志内加一条报警记录

                string alertmessage = formlist[4].Value + formlist[5].Value + "报警！" + formlist[1].Value + formlist[2].Value + ":" + formlist[3].Value + "存在异常！";
                new SysLogDAO().AddLog(LogType.报警日志, alertmessage, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
            }

        }

        public void SendDDRemindNotice(string userlist, string url, string pcurl, string head, string title, string content, List<KeyValuePair<string, string>> formlist = null)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                JObject msg = new JObject();
                msg.Add("msgtype", "oa");

                JObject oa = new JObject();
                oa.Add("message_url", url);
                //先不做
                //oa.Add("pc_message_url", pcurl);

                JObject headobject = new JObject();
                headobject.Add("bgcolor", "FF009ACD");
                headobject.Add("text", head);
                oa.Add("head", headobject);

                JObject bodyobject = new JObject();
                bodyobject.Add("title", title);
                bodyobject.Add("content", content);
                if (formlist != null && formlist.Count > 0)
                {
                    JArray formobject = new JArray();

                    foreach (KeyValuePair<string, string> formdata in formlist)
                    {
                        JObject formdataobject = new JObject();
                        formdataobject.Add("key", formdata.Key);
                        formdataobject.Add("value", formdata.Value);
                        formobject.Add(formdataobject);
                    }

                    bodyobject.Add("form", formobject);
                }
                bodyobject.Add("image", ParaUtils.RemindImageID);

                oa.Add("body", bodyobject);

                msg.Add("oa", oa);

                string accesstoken = new DDUtils().GetAccessToken();
                new DDUtils().SendMessage(msg, userlist, accesstoken);
            }

        }

        public List<KeyValuePair<string, string>> BuildFormList(string tabletypename, string columnname, string data, string baseclassname, string realname)
        {
            List<KeyValuePair<string, string>> formlist = new List<KeyValuePair<string, string>>();
            formlist.Add(new KeyValuePair<string, string>("报警时间:", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            formlist.Add(new KeyValuePair<string, string>("表单类型:", tabletypename));
            formlist.Add(new KeyValuePair<string, string>("报警字段:", columnname));
            formlist.Add(new KeyValuePair<string, string>("报警数据:", data));
            formlist.Add(new KeyValuePair<string, string>("报警班次:", baseclassname));
            formlist.Add(new KeyValuePair<string, string>("报警人员:", realname));
            return formlist;
        }

        public List<KeyValuePair<string, string>> BuildRemindFormList(string tabletypename, string flowname, DateTime enddate)
        {
            List<KeyValuePair<string, string>> formlist = new List<KeyValuePair<string, string>>();
            formlist.Add(new KeyValuePair<string, string>("提醒时间:", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            formlist.Add(new KeyValuePair<string, string>("表单类型:", tabletypename));
            formlist.Add(new KeyValuePair<string, string>("表单流程:", flowname));
            formlist.Add(new KeyValuePair<string, string>("结束时间:", enddate.ToString("yyyy-MM-dd HH:mm:ss")));
            return formlist;
        }

        public string GetUrl(string url, int taskid, int flowid, bool ismobile = true)
        {
            if (ismobile)
            {
                url = url.Substring(0, url.IndexOf("mobile") + 7);
                url = url + "TaskAuditEdit.aspx?taskid=" + taskid + "&flowid=" + flowid;
            }
            else
            {

            }


            return url;
        }


    }
}
