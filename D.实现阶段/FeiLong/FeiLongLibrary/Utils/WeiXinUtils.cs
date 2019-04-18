using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using FeiLongLibrary.DAO;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Enums;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Utils;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace FeiLongLibrary.Utils
{
    /// <summary>
    /// Api 返回的结果
    /// </summary>
    public class WeiXinUtils
    {
        FLDbContext db = new FLDbContext();

        private string PostData(string url, string postData)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Method = "POST";
            myRequest.ContentType = "text/xml";
            //myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();

            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
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

        //public string BlindUser(WXUser wxuser)
        //{
        //    //string postdata = new WXUserDao().getUserJsonStr(wxuser);

        //    //string resultstr = PostData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token={0}", wxuser.accesstoken), postdata);

        //    string postUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token=" + GetAccessToken(ParaUtils.WXUserAddSecret);
        //    string jsonStr = new WXUserDao().getUserJsonStr(wxuser);
        //    byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
        //    request.Method = "POST";
        //    request.ContentLength = bytes.Length;
        //    request.ContentType = "text/xml";
        //    Stream reqstream = request.GetRequestStream();
        //    reqstream.Write(bytes, 0, bytes.Length);

        //    //声明一个HttpWebRequest请求  
        //    request.Timeout = 90000;
        //    //设置连接超时时间  
        //    request.Headers.Set("Pragma", "no-cache");
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    Stream streamReceive = response.GetResponseStream();
        //    Encoding encoding = Encoding.UTF8;

        //    StreamReader streamReader = new StreamReader(streamReceive, encoding);
        //    string strResult = streamReader.ReadToEnd();
        //    streamReceive.Dispose();
        //    streamReader.Dispose();


        //    Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(strResult);

        //    return ja["errmsg"].ToString();
        //}

        public string EditUser(string name, string mobile, string department, string userid, int enable)
        {
            string postUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token=" + GetAccessToken(ParaUtils.WXUserAddSecret);
            string jsonStr = "";
            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append("{");
            sBuilder.Append("\"userid\": \"" + userid + "\",");
            sBuilder.Append("\"name\": \"" + name + "\",");
            sBuilder.Append("\"department\": [" + department + "],");
            sBuilder.Append("\"mobile\": \"" + mobile + "\",");
            sBuilder.Append("\"enable\": " + enable + ",");
            sBuilder.Append("\"extattr\": {}");
            sBuilder.Append("}");
            jsonStr = sBuilder.ToString();

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


        public string BlindUnit(string name, int parentid, int order, int id)
        {
            //string postdata = new WXUserDao().getUserJsonStr(wxuser);

            //string resultstr = PostData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/create?access_token={0}", wxuser.accesstoken), postdata);

            string postUrl = "https://qyapi.weixin.qq.com/cgi-bin/department/create?access_token=" + GetAccessToken(ParaUtils.WXUserAddSecret);
            string jsonStr = "";

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append("\"name\": \"" + name + "\",");
            sBuilder.Append("\"parentid\": \"" + parentid + "\",");
            sBuilder.Append("\"order\": \"" + order + "\",");
            sBuilder.Append("\"id\": " + id + "");
            sBuilder.Append("}");
            jsonStr = sBuilder.ToString();
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


        public string EditUnit(string name, int parentid, int order, int id)
        {
            string postUrl = "https://qyapi.weixin.qq.com/cgi-bin/department/update?access_token=" + GetAccessToken(ParaUtils.WXUserAddSecret);
            string jsonStr = "";

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append("\"name\": \"" + name + "\",");
            sBuilder.Append("\"parentid\": \"" + parentid + "\",");
            sBuilder.Append("\"order\": \"" + order + "\",");
            sBuilder.Append("\"id\": " + id + "");
            sBuilder.Append("}");
            jsonStr = sBuilder.ToString();
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

        public string DelUnit(int id)
        {
            string accessget = GetData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/delete?access_token={0}&id={1}", GetAccessToken(ParaUtils.WXUserAddSecret), id));

            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(accessget);

            return ja["errcode"].ToString();
        }


        public string GetAccessToken(string secret)
        {

            string accessget = GetData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", ParaUtils.WXCorpid, secret));

            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(accessget);

            return ja["access_token"].ToString();
        }



        public string GetUserTicket(string accesstoken, string code)
        {
            string accessget = GetData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", accesstoken, code));

            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(accessget);

            return ja["user_ticket"].ToString();
        }

        public string GetUserID(string accesstoken, string code)
        {
            string accessget = "";
            try
            {
                accessget = GetData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", accesstoken, code));
                Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(accessget);
                try
                {
                    return ja["UserId"].ToString();
                }
                catch
                {
                    return accesstoken;
                }
            }
            catch
            {
                accessget = "api";
                return accessget;
            }



        }

        public string GetUserMoblieByID(string accesstoken, string userid)
        {
            string accessget = GetData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}", accesstoken, userid));

            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(accessget);

            return ja["mobile"].ToString();
        }

        public string GetUserMoblie(string accesstoken, string userticket)
        {
            string postUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserdetail?access_token=" + accesstoken;

            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append("{");
            sBuilder.Append("\"user_ticket\":  \"" + userticket + "\"");
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

            return ja["mobile"].ToString();
        }

        private ArrayList bytesArray;
        private Encoding encoding = Encoding.UTF8;
        private string boundary = String.Empty;

        private byte[] MergeContent()
        {
            int length = 0;
            int readLength = 0;
            string endBoundary = "--" + boundary + "--\r\n";
            byte[] endBoundaryBytes = encoding.GetBytes(endBoundary);

            bytesArray.Add(endBoundaryBytes);

            foreach (byte[] b in bytesArray)
            {
                length += b.Length;
            }

            byte[] bytes = new byte[length];

            foreach (byte[] b in bytesArray)
            {
                b.CopyTo(bytes, readLength);
                readLength += b.Length;
            }

            return bytes;
        }



        public string POSTfile(string access_token, string type, string file)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            //请求 
            WebRequest req = WebRequest.Create(@"https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token=" + access_token + "&type=" + type);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;

            //组织表单数据 
            StringBuilder sb = new StringBuilder();

            sb.Append("--" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"media\"; filename=\"" + file + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: image/pjpeg");
            sb.Append("\r\n\r\n");

            string head = sb.ToString();
            byte[] form_data = Encoding.UTF8.GetBytes(head);
            //结尾 
            byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            //文件 
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            //post总长度 
            long length = form_data.Length + fileStream.Length + foot_data.Length;
            req.ContentLength = length;

            Stream requestStream = req.GetRequestStream();
            //发送表单参数 
            requestStream.Write(form_data, 0, form_data.Length);
            //文件内容 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);
            //结尾 
            requestStream.Write(foot_data, 0, foot_data.Length);
            requestStream.Close();

            //响应 
            WebResponse pos = req.GetResponse();

            StreamReader streamReader = new StreamReader(pos.GetResponseStream(), encoding);
            string strResult = streamReader.ReadToEnd();
            requestStream.Dispose();
            streamReader.Dispose();


            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(strResult);

            return ja["media_id"].ToString();
        }


        public string GetMediaID(string accesstoken, string type, string filepath)
        {
            string postUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accesstoken, type);
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            FileInfo fileInfo = new FileInfo(filepath);

            //            string fileHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
            //"Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
            //Environment.NewLine + "Content-Type: multipart/form-data" + Environment.NewLine + Environment.NewLine;

            //            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(string.Format(fileHeaderTemplate, "media", fileInfo.FullName));

            string strBoundary = "----" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(fileInfo.FullName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("multipart/form-data");
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(strPostHeader);


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";

            WebProxy proxy = new WebProxy();
            proxy.UseDefaultCredentials = true;
            request.Proxy = proxy;

            request.ContentLength = bytes.Length;
            request.ContentType = "multipart/form-data;boundary=" + "----" + DateTime.Now.Ticks.ToString("x");
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            //声明一个HttpWebRequest请求  
            request.Timeout = 90000;
            //设置连接超时时间  
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            //Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(strResult);

            return ja["media_id"].ToString();
        }

        //public string SendArticle(string accesstoken, Article article)
        //{
        //    string postUrl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + accesstoken;

        //    string content = article.Content.Replace("\"", "\\\"").Replace("src=\"/upload/", "src=\"" + ParaUtils.SitePath + "/upload/");
        //    //string mediaid = this.POSTfile(accesstoken, "image", System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/Resource/SiteImage/1.jpg");

        //    StringBuilder sBuilder = new StringBuilder();

        //    sBuilder.Append("{");
        //    sBuilder.Append("\"touser\":  \"@all\",");
        //    sBuilder.Append("\"msgtype\":  \"news\",");
        //    sBuilder.Append("\"agentid\":  \"" + ParaUtils.WXArticleSecretID + "\",");
        //    //sBuilder.Append("\"mpnews\":  {\"title\": \"" + article.Title + "\",\"content\" :\"" + article.Content + "\",\"thumb_media_id\": \"" + mediaid + "\",\"author\":\"" + db.SysUser.FirstOrDefault(t => t.ID == article.UserID).RealName + "\" ,\"content_source_url\":\"" + ParaUtils.SitePath + "/show.html?Index=" + article.ID + "\"}");
        //    sBuilder.Append("\"news\":{\"articles\" : [ {\"title\": \"" + article.Title + "\",\"description\" :\"芜湖县教研室\",\"picurl\": \"" + ParaUtils.SitePath + "/Resource/SiteImage/1.jpg" + "\" ,\"url\":\"" + ParaUtils.SitePath + "/Weixin/Show.html?Index=" + article.ID + "\"}]}");

        //    sBuilder.Append("}");

        //    string jsonStr = sBuilder.ToString();

        //    byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
        //    request.Method = "POST";
        //    request.ContentLength = bytes.Length;
        //    request.ContentType = "text/xml";
        //    Stream reqstream = request.GetRequestStream();
        //    reqstream.Write(bytes, 0, bytes.Length);

        //    //声明一个HttpWebRequest请求  
        //    request.Timeout = 90000;
        //    //设置连接超时时间  
        //    request.Headers.Set("Pragma", "no-cache");
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    Stream streamReceive = response.GetResponseStream();
        //    Encoding encoding = Encoding.UTF8;

        //    StreamReader streamReader = new StreamReader(streamReceive, encoding);
        //    string strResult = streamReader.ReadToEnd();
        //    streamReceive.Dispose();
        //    streamReader.Dispose();

        //    Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(strResult);

        //    return ja["errmsg"].ToString();
        //}


        public void BuildWeiXinUser()
        {
            string accessget = GetData(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/simplelist?access_token=YL6XwSZC8R8-tYKIvziK8KhwKSaUkEgNNHtOPckutcFL1JgUA7OclszUhDyNtY3tO2YbhYbRQj7yt82Ggyev0WEFc6dND8ju3lsMJG-VIaLey6p0bA3XqHbhJ85ZIMVImgthcA_PyA9t4eN70eNvQi8_zVg7H73sGDoXVkSY8G3cq7xu4oQrJv0jJ5ssNW0NeegZFrtKpeinn9gydVDaGpv9ddkxOb6gKCN9uUmafk8zJV6r0M298UGuTZwcnBGN_rsLQuqU2r4gRznOazO7AvCecCzrB5vE_sMbJLJ29Iw&department_id=1&fetch_child=1"));

            Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(accessget);

            IEnumerable<JProperty> properties = ja.Properties();

            for (int i = 0; i < ja["userlist"].Count(); i++)
            {
                string realname = ja["userlist"][i]["name"].ToString();
                string userid = ja["userlist"][i]["userid"].ToString();
                SysUser user = db.SysUser.FirstOrDefault(t => t.RealName == realname && string.IsNullOrEmpty(t.WXID));
                if (user != null)
                {
                    user.WXID = userid;
                }
            }
            db.SaveChanges();

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <param name="userlist"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string SendMessage(string accesstoken, string userlist, string message, string secretid)
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


        public void BuildApartment(string accesstoken)
        {

        }

    }

}