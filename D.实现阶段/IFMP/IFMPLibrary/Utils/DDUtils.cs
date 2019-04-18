using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;

namespace IFMPLibrary.Utils
{
    public class DDUtils
    {
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

        private JObject PostData(string url, JObject data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data.ToString());
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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

            JObject ReturnObject = (JObject)JsonConvert.DeserializeObject(strResult);

            return ReturnObject;
        }

        private JObject PostFile(string url, JObject data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data.ToString());
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "multipart/form-data";
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

            JObject ReturnObject = (JObject)JsonConvert.DeserializeObject(strResult);

            return ReturnObject;
        }


        public string GetAccessToken()
        {
            string accesstokendate = new BaseUtils().GetXML("Setting/DDSetting/AccessTokenDate");

            if (string.IsNullOrEmpty(accesstokendate) || Convert.ToDateTime(accesstokendate) < DateTime.Now)
            {
                string accessget = GetData(string.Format("https://oapi.dingtalk.com/gettoken?corpid={0}&corpsecret={1}", ParaUtils.Corpid, ParaUtils.CorpSecret));
                JObject jo = (JObject)JsonConvert.DeserializeObject(accessget);
                //修改accesstoken和accesstokendate
                int expires = Convert.ToInt32(jo["expires_in"]);
                new BaseUtils().SetXML("Setting/DDSetting/AccessToken", jo["access_token"].ToString());
                new BaseUtils().SetXML("Setting/DDSetting/AccessTokenDate", DateTime.Now.AddSeconds(expires * 0.8).ToString("yyyy-MM-dd HH:mm:ss"));
            }

            string accesstoken = new BaseUtils().GetXML("Setting/DDSetting/AccessToken");
            return accesstoken;
        }

        public void GetAttendance(DateTime BeginDate, DateTime EndDate, List<string> UserList, int Limit = 10)
        {
            string accesstoken = "763dc1d442033182b34f16825c85a27c";
            int offset = 0;
            JArray ReturnArray = new JArray();
            JArray ReturnDataArray = GetFullAttendance(accesstoken, BeginDate, EndDate, UserList, ReturnArray, offset, Limit);

        }


        public JArray GetFullAttendance(string accesstoken, DateTime begindate, DateTime enddate, List<string> userlist, JArray ReturnArray, int offset = 0, int limit = 10)
        {
            string url = string.Format("https://oapi.dingtalk.com/attendance/list?access_token={0}", accesstoken);
            JObject GetData = new JObject();
            GetData.Add("workDateFrom", begindate.ToString("yyyy-MM-dd HH:mm:ss"));
            GetData.Add("workDateTo", enddate.ToString("yyyy-MM-dd HH:mm:ss"));
            JArray UserAray = new JArray();
            foreach (string user in userlist)
            {
                UserAray.Add(user);
            }
            GetData.Add("userIdList", UserAray);
            GetData.Add("offset", offset);
            GetData.Add("limit", limit);
            JObject ReturnData = PostData(url, GetData);
            ReturnArray.Add(ReturnData);
            if (Convert.ToBoolean(ReturnData["hasMore"]))
            {
                GetFullAttendance(accesstoken, begindate, enddate, userlist, ReturnArray, offset + limit, limit);
            }
            return ReturnArray;
        }


        public void SendMessage(JObject data, string userlist, string accesstoken)
        {
            string url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accesstoken);
            JObject GetData = new JObject();
            GetData.Add("agent_id", ParaUtils.AgentID);
            //GetData.Add("userid_list", "085146610134609723");
            GetData.Add("userid_list", userlist);
            GetData.Add("msg", data);
            JObject ReturnData = PostData(url, GetData);
        }

        //不是这个方法
        public void PostImage()
        {
            string accesstoken = GetAccessToken();
            string url = string.Format("https://oapi.dingtalk.com/media/upload?access_token={0}&type={1}", accesstoken, "image");
            JObject GetData = new JObject();
            //GetData.Add("type", "image");
            GetData.Add("media", "http://bpic.588ku.com/element_origin_min_pic/18/06/10/ca39270b76591ff77be13e42bde177bc.jpg");

            JObject ReturnData = PostFile(url, GetData);
        }

        public string NewPostFile()
        {
            string accesstoken = GetAccessToken();
            string fileWithPath = @"D:\timg.jpg";
            string url = string.Format("https://oapi.dingtalk.com/media/upload?access_token={0}&type={1}", accesstoken, "image");
            var result = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            using (Stream requestStream = request.GetRequestStream())
            {
                byte[] boundarybytes = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
                byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "–-\r\n");
                var filename = Path.GetFileName(fileWithPath);
                using (FileStream fs = new FileStream(fileWithPath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bArr = new byte[fs.Length];
                    fs.Read(bArr, 0, bArr.Length);
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    var header = "Content-Disposition:form-data;name=\"media\";filename=\"" + filename + "\"\r\nfilelength=\"" + fs.Length + "\"\r\nContent-Type:application/octet-stream\r\n\r\n";
                    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(header.ToString());
                    requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    fs.Close();
                    requestStream.Write(bArr, 0, bArr.Length);
                    requestStream.Write(trailer, 0, trailer.Length);
                }
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public void AddUser(JObject data, User user, string accesstoken)
        {
            string url = string.Format("https://oapi.dingtalk.com/user/create?access_token={0}", accesstoken);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                UserDetails UserDetails = db.UserDetails.FirstOrDefault(t => t.UserID == user.ID);
                JObject GetData = new JObject();
                GetData.Add("userid", user.ID);
                GetData.Add("name", user.RealName);
                GetData.Add("userid", user.ID);
                JArray departmentarray = new JArray();
                List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && t.IsAdmin && db.DepartmentUser.Where(m => m.UserID == user.ID).Select(m => m.DepartmentID).Contains(t.ID)).ToList();
                foreach (Department Department in DepartmentList)
                {
                    departmentarray.Add(Department.ID);
                }
                GetData.Add("department", departmentarray);

                Post Post = db.Post.Where(t => t.IsDel != true && db.PostUser.Where(m => m.UserID == user.ID).Select(m => m.PostID).Contains(t.ID)).FirstOrDefault();
                if (Post != null)
                {
                    GetData.Add("position", Post.Name);
                }

                GetData.Add("mobile", user.Cellphone);
                GetData.Add("jobnumber", UserDetails.Identity);
                DateTime basedate = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                GetData.Add("hiredDate", (long)(UserDetails.HireDate - basedate).TotalSeconds);
                //主管设置
                JObject ReturnData = PostData(url, GetData);
            }
        }

        public void UpdateUser(JObject data, User user, string accesstoken)
        {
            string url = string.Format("https://oapi.dingtalk.com/user/update?access_token={0}", accesstoken);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                JObject GetData = new JObject();
                GetData.Add("userid", user.ID);
                GetData.Add("name", user.RealName);
                GetData.Add("userid", user.ID);
                JArray departmentarray = new JArray();
                List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && t.IsAdmin && db.DepartmentUser.Where(m => m.UserID == user.ID).Select(m => m.DepartmentID).Contains(t.ID)).ToList();
                foreach (Department Department in DepartmentList)
                {
                    departmentarray.Add(Department.ID);
                }
                GetData.Add("department", departmentarray);

                Post Post = db.Post.Where(t => t.IsDel != true && db.PostUser.Where(m => m.UserID == user.ID).Select(m => m.PostID).Contains(t.ID)).FirstOrDefault();
                if (Post != null)
                {
                    GetData.Add("position", Post.Name);
                }

                GetData.Add("mobile", user.Cellphone);
                GetData.Add("jobnumber", db.UserDetails.FirstOrDefault(t => t.UserID == user.ID).Identity);

                JObject ReturnData = PostData(url, GetData);
            }
        }

        public void DeleteUser(JObject data, string ids, string accesstoken)
        {
            string url = string.Format("https://oapi.dingtalk.com/user/batchdelete?access_token={0}", accesstoken);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                JObject GetData = new JObject();
                JArray idarray = new JArray();
                foreach (string id in ids.Split(','))
                {
                    idarray.Add(id);
                }
                GetData.Add("useridlist", idarray);

                JObject ReturnData = PostData(url, GetData);
            }
        }


        public void AddDepartment(JObject data, Department department, string accesstoken)
        {
            string url = string.Format("https://oapi.dingtalk.com/department/create?access_token={0}", accesstoken);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                JObject GetData = new JObject();
                GetData.Add("name", department.Name);
                GetData.Add("parentid", department.ParentID == -1 ? 1 : db.Department.FirstOrDefault(t => t.ID == department.ParentID).DingID);
                JObject ReturnData = PostData(url, GetData);
                int dingid = Convert.ToInt32(ReturnData["id"]);
                department.DingID = dingid;
                db.SaveChanges();
            }
        }


        public void UpdateDepartment(JObject data, Department department, string accesstoken)
        {
            string url = string.Format("https://oapi.dingtalk.com/department/update?access_token={0}", accesstoken);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                JObject GetData = new JObject();
                GetData.Add("name", department.Name);
                GetData.Add("parentid", department.ParentID == -1 ? 1 : db.Department.FirstOrDefault(t => t.ID == department.ParentID).DingID);
                JObject ReturnData = PostData(url, GetData);
            }
        }

        public void DeleteDepartment(JObject data, Department department, string accesstoken)
        {
            string url = string.Format("https://oapi.dingtalk.com/department/delete?access_token={0}&id={}", accesstoken, department.ID);
            GetData(url);
        }
    }
}
