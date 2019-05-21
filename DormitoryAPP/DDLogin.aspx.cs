/*钉钉免登共如下步骤:
 * 1.加入免登群 申请获得appid与appserect。（access_token）
 * 2.创建钉钉微应用。设置微应用地址为：https://oapi.dingtalk.com/connect/oauth2/sns_authorize?appid=APPID&response_type=code&scope=snsapi_login&state=STATE&redirect_uri=REDIRECT_URI
 * （其中appid与redirect_uri（需要urlencode编码）必填）
 * 3.回调网址会追加code参数，登录页面需获取此code（临时身份验证码）（tmp_auth_code）。
 * 4.根据临时验证码获取持久授权码。（sns_token）
 * 5.根据永久验证码获取用户unionid。
 * 6.根据unionid获取用户userid。
 * 7.根据userid获取详细信息（手机，姓名。）
 */

using System;
using System.Linq;
using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using System.Configuration;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace DormitoryAPP
{
    public partial class DDLogin : System.Web.UI.Page
    {
        //public string agentId = "37232286";
        //public string corpId = "ding622179de41ce4b65";
        //public string CorpSecret = "bhwGmBemlj0mXv5UCKGAQ6WGoAXxPtf2mFkOzmRThUXFGfM96KCQb-PD32hdalp-";
        //public string nonceStr = "sgffd674efdgs";
        public string agentId = ConfigurationManager.AppSettings["ArgentID"];
        public string corpId = ConfigurationManager.AppSettings["CorpID"];
        public string CorpSecret = ConfigurationManager.AppSettings["CorpSecret"];
        public string nonceStr = ConfigurationManager.AppSettings["nonceStr"];
        public string timestamp = string.Empty;
        //测试使用
        //public string agentId = "38189067";
        //public string corpId = "dingbc5b86cca0a36b29";
        //public string CorpSecret = "Ua6F9ABeRKQFFT1JiuV-f7GrLYHXqeTfNuIH9ClqFW1Dz8vDpNcq4c2DzTDln9Yp";
        //public string timestamp = string.Empty;
        //public string nonceStr = "sgffd674efdgs";

        public string signature = string.Empty;
        public string accessToken = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //btn_CodeInfo();
                GetConfig();
            }
        }

        #region 测试方法
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CodeInfo()
        {
            //string code = this.hf_Code.Value;
            string code = "0b3e1cb45d033f239dd20827bd6f4f79";
            string at = CommonFunction.RequestUrl(string.Format("https://oapi.dingtalk.com/gettoken?corpid={0}&corpsecret={1}", corpId, CorpSecret));
            accessToken = CommonFunction.Json(at, "access_token");
            string useridJson = CommonFunction.RequestUrl(string.Format("https://oapi.dingtalk.com/user/getuserinfo?access_token={0}&code={1}", accessToken, code));
            string userid = CommonFunction.Json(useridJson, "userid");
            if (userid == null || userid == "")
            {
                string mess = CommonFunction.Json(useridJson, "errmsg");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "<script>alert('错误信息：" + mess + "');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'Default.aspx';", true);
            }
            else
            {
                string userInfoJson = CommonFunction.RequestUrl(string.Format("https://oapi.dingtalk.com/user/get?access_token={0}&userid={1}", accessToken, userid));
                string mobile = CommonFunction.Json(userInfoJson, "mobile");
                string name = CommonFunction.Json(userInfoJson, "name");
                if (mobile == null || name == "")
                {
                    string mess = CommonFunction.Json(userInfoJson, "errmsg");
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "<script>alert( '" + mess + " ');</script>");
                    //Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'Default.aspx';", true);
                    Response.Write(mess + "【请联系管理员】");
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(mobile))
                        {
                            using (IFMPDBContext db = new IFMPDBContext())
                            {
                                User user = db.User.FirstOrDefault(t => t.RealName == name && t.DDID == userid && t.Cellphone == mobile);
                                Response.Cookies["UserID"].Value = user.ID.ToString();
                                Response.Cookies["SysUserName"].Value = user.UserName;
                                Response.Cookies["RealName"].Value = HttpUtility.UrlEncode(user.RealName, Encoding.GetEncoding("UTF-8"));
                               // Response.Cookies["UserFace"].Value = sysuser.PreStr;
                                Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'index.html';", true);
                            }                            
                        }
                        else
                        {
                            Response.Write("身份验证出错，请联系管理员");
                            //Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'Default.aspx';", true);
                        }
                    }
                    catch (Exception error)
                    {
                        Response.Write(error.Message + "【请联系管理员】");
                        // Page.ClientScript.RegisterStartupScript(GetType(), "", "alert('" + error.Message + "');");
                    }
                }
            }
        }
        #endregion

        #region jsapi签名验证处理
        /// <summary>
        /// jsapi签名验证处理
        /// </summary>
        private void GetConfig()
        {
            string access_Token = CommonFunction.RequestUrl(string.Format("https://oapi.dingtalk.com/gettoken?corpid={0}&corpsecret={1}", corpId, CorpSecret));
            accessToken = CommonFunction.Json(access_Token, "access_token");
            string Ticket = CommonFunction.RequestUrl(string.Format(" https://oapi.dingtalk.com/get_jsapi_ticket?access_token={0}", accessToken.Trim()));
            string jsApiTicket = CommonFunction.Json(Ticket, "ticket");
            timestamp = timeStamp();
            //测试使用
            // string url = "http://dd.whsedu.cn/DDDefault.aspx";
            //string url = "http://whgkdz.whsedu.cn/sismpapp/DDDefault.aspx";
            // string url = ConfigurationManager.AppSettings["URL"];
            string url = Request.Url.AbsoluteUri;
            string signature1 = "jsapi_ticket=" + jsApiTicket.Trim() + "&noncestr=" + nonceStr.Trim() + "&timestamp=" + timestamp.Trim() + "&url=" + url.Trim();
            signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(signature1, "SHA1").ToLower();
        }
        #endregion

        #region 成功返回code成功点击事件
        /// <summary>
        /// 成功返回code成功点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Code_Click(object sender, EventArgs e)
        {
            string code = this.hf_Code.Value;
            string at = CommonFunction.RequestUrl(string.Format("https://oapi.dingtalk.com/gettoken?corpid={0}&corpsecret={1}", corpId, CorpSecret));
            accessToken = CommonFunction.Json(at, "access_token");
            string useridJson = CommonFunction.RequestUrl(string.Format("https://oapi.dingtalk.com/user/getuserinfo?access_token={0}&code={1}", accessToken, code));
            string userid = CommonFunction.Json(useridJson, "userid");
            if (userid == null || userid == "")
            {

                string mess = CommonFunction.Json(useridJson, "errmsg");
                Response.Write(mess + "【请联系管理员】");
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "<script>alert('错误信息：" + mess + "');</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'Default.aspx';", true);
            }
            else
            {
                string userInfoJson = CommonFunction.RequestUrl(string.Format("https://oapi.dingtalk.com/user/get?access_token={0}&userid={1}", accessToken, userid));
                string mobile = CommonFunction.Json(userInfoJson, "mobile");
                string name = CommonFunction.Json(userInfoJson, "name");
                if (mobile == null || name == "")
                {
                    string mess = CommonFunction.Json(userInfoJson, "errmsg");
                    Response.Write(mess + "【请联系管理员】");
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "<script>alert( '" + mess + " ');</script>");
                    //Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'Default.aspx';", true);
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(mobile))
                        {
                            using (IFMPDBContext db = new IFMPDBContext())
                            {
                                User user = db.User.FirstOrDefault(t => t.RealName == name && t.Cellphone == mobile);
                                if (user != null)
                                {
                                    user.DDID = userid;
                                    db.SaveChanges();
                                    Response.Cookies["UserID"].Value = user.ID.ToString();
                                    Response.Cookies["SysUserName"].Value = user.UserName;
                                    Response.Cookies["RealName"].Value = HttpUtility.UrlEncode(user.RealName, Encoding.GetEncoding("UTF-8"));
                                   // Response.Cookies["UserFace"].Value = sysuser.PreStr;
                                    Response.Cookies["SysUserPwd"].Value = user.Password;
                                    Response.Cookies["DepIds"].Value = db.DepartmentUser.FirstOrDefault(t => t.UserID == user.ID).DepartmentID.ToString();
                                    //Response.Cookies["UserType"].Value = sysuser.UserType;
                                    //Response.Cookies["RankName"].Value = sysuser.Rank.ToString();
                                    //Response.Cookies["RankScore"].Value = (sysuser.Rank == 1 ? "2" : sysuser.Rank == 2 ? "5" : sysuser.Rank == 3 ? "5" : sysuser.Rank == 4 ? "10" : sysuser.Rank == 5 ? "30" : sysuser.Rank == 6 ? "50" : "100");

                                    // Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'Main.aspx';", true);

                                    string url = Request.Cookies["rurl"] == null ? "" : Request.Cookies["rurl"].Value;
                                    if (string.IsNullOrEmpty(url))
                                    {
                                        Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'index.html';", true);
                                        //Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'index.html?UserID=" + userid + "';", true);
                                    }
                                    else
                                    {
                                        Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = '" + System.Web.HttpUtility.UrlDecode(url) + "';", true);
                                    }
                                }
                                else
                                {
                                    Page.ClientScript.RegisterStartupScript(GetType(), "", "alert('请登录pc端，完善个人信息！');", true);
                                    //Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'Default.aspx';", true);
                                   // Response.Redirect("Login.aspx");
                                }
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "", "alert('用户名或密码错误！');", true);
                            // Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'Default.aspx';", true);
                           // Response.Redirect("Login.aspx");
                        }
                    }
                    catch (Exception error)
                    {
                        Response.Write(error.Message + "【请联系管理员】");
                        // Page.ClientScript.RegisterStartupScript(GetType(), "", "alert('" + error.Message + "');");
                    }
                }
            }
        }
        #endregion

        #region 时间戳的随机数
        /// <summary>
        /// 时间戳的随机数
        /// </summary>
        /// <returns></returns>
        public static string timeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds).ToString();
        }
        #endregion

        //#region 后台post事件
        ///// <summary>
        ///// 后台post事件
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public static string Post(string url, string param)
        //{
        //    string strURL = url;
        //    System.Net.HttpWebRequest request;
        //    request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
        //    request.Method = "POST";
        //    request.ContentType = "application/json;charset=UTF-8";
        //    string paraUrlCoded = param;
        //    byte[] payload;
        //    payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
        //    request.ContentLength = payload.Length;
        //    Stream writer = request.GetRequestStream();
        //    writer.Write(payload, 0, payload.Length);
        //    writer.Close();
        //    System.Net.HttpWebResponse response;
        //    response = (System.Net.HttpWebResponse)request.GetResponse();
        //    System.IO.Stream s;
        //    s = response.GetResponseStream();
        //    string StrDate = "";
        //    string strValue = "";
        //    StreamReader Reader = new StreamReader(s, Encoding.UTF8);
        //    while ((StrDate = Reader.ReadLine()) != null)
        //    {
        //        strValue += StrDate + "\r\n";
        //    }
        //    return strValue;
        //}
        //#endregion

        //#region 截取Json字符串
        ///// <summary>
        ///// 截取Json字符串
        ///// </summary>
        ///// <param name="json"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string Json(string json, string key)
        //{
        //    string result = string.Empty;
        //    if (!string.IsNullOrEmpty(json))
        //    {
        //        key = "\"" + key.Trim('"') + "\"";
        //        int index = json.IndexOf(key) + key.Length + 1;
        //        if (index > key.Length + 1)
        //        {
        //            //先截逗号，若是最后一个，截“｝”号，取最小值
        //            int end = json.IndexOf(',', index);
        //            if (end == -1)
        //            {
        //                end = json.IndexOf('}', index);
        //            }

        //            result = json.Substring(index, end - index);
        //            result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
        //        }
        //    }
        //    return result;
        //}
        //#endregion

        //#region get事件
        ///// <summary>
        ///// get事件
        ///// </summary>
        ///// <param name="urlString"></param>
        ///// <returns></returns>
        //public static string RequestUrl(string urlString)
        //{
        //    //定义局部变量
        //    HttpWebRequest httpWebRequest = null;
        //    HttpWebResponse httpWebRespones = null;
        //    Stream stream = null;
        //    string htmlString = string.Empty;
        //    //请求页面
        //    try
        //    {
        //        httpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;
        //    }
        //    //处理异常
        //    catch (Exception ex)
        //    {
        //        throw new Exception("建立页面请求时发生错误！", ex);
        //    }
        //    httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
        //    //获取服务器的返回信息
        //    try
        //    {
        //        httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
        //        stream = httpWebRespones.GetResponseStream();
        //    }
        //    //处理异常
        //    catch (Exception ex)
        //    {
        //        throw new Exception("接受服务器返回页面时发生错误！", ex);
        //    }
        //    StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
        //    //读取返回页面
        //    try
        //    {
        //        htmlString = streamReader.ReadToEnd();
        //    }
        //    //处理异常
        //    catch (Exception ex)
        //    {
        //        throw new Exception("读取页面数据时发生错误！", ex);
        //    }
        //    //释放资源返回结果
        //    streamReader.Close();
        //    stream.Close();
        //    return htmlString;
        //}
        //#endregion
    }
}