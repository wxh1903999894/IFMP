using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using IFMPLibrary.DAO;

namespace IFMP.ashx
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Params["method"];
            switch (method)
            {
                case "LoginIn":
                    LoginIn(context);
                    break;
                case "ClassLoginIn":
                    ClassLoginIn(context);
                    break;
                case "Out":
                    Out(context);
                    break;
            }
        }

        #region 登录
        public void LoginIn(HttpContext context)
        {
            string message = "";
            string username = context.Request.Params["name"];
            string pwd = context.Request.Params["psw"].ToString();

            pwd = new BaseUtils().BuildPW(username, pwd);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                //int Count = db.User.Count();
                //List<User> UserList = db.User.Where(t => t.ID == 10).ToList();
                User User = db.User.FirstOrDefault(t => t.UserName == username && t.Password == pwd);
                if (User != null)
                {
                    if (User.IsDel != true && User.UserState != UserState.离职)
                    {
                        HttpContext.Current.Response.Cookies["UserID"].Value = User.ID.ToString();
                        HttpContext.Current.Response.Cookies["SysUserName"].Value = User.UserName;
                        HttpContext.Current.Response.Cookies["RealName"].Value = HttpUtility.UrlEncode(User.RealName, Encoding.GetEncoding("UTF-8"));
                        HttpContext.Current.Response.Cookies["SysUserPwd"].Value = User.Password;

                        new SysLogDAO().AddLog(LogType.登录日志, "用户【" + User.RealName + "】于北京时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "登录系统", User.ID);
                    }
                    else
                    {
                        message = "账号存在异常，请检查";
                    }
                }
                else
                {
                    message = "用户名或密码错误";
                }
            }

            StringBuilder sb = new StringBuilder("");
            sb.Append("{\"result\":\"" + message + "\"}");
            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.End();
        }
        #endregion


        #region 登录
        public void ClassLoginIn(HttpContext context)
        {
            string message = "";
            int baseclassid = Convert.ToInt32(context.Request["classid"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                BaseClass BaseClass = db.BaseClass.FirstOrDefault(t => t.ID == baseclassid && t.IsDel != true);

                if (BaseClass != null)
                {
                    HttpContext.Current.Response.Cookies["BaseClassID"].Value = BaseClass.ID.ToString();
                    HttpContext.Current.Response.Cookies["BaseClassName"].Value = HttpUtility.UrlEncode(BaseClass.Name, Encoding.GetEncoding("UTF-8"));

                    new SysLogDAO().AddLog(LogType.登录日志, "班次【" + BaseClass.Name + "】于北京时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "登录系统");
                    message = "success";
                }
                else
                {
                    message = "用户名或密码错误";
                }
            }

            StringBuilder sb = new StringBuilder("");
            sb.Append("{\"result\":\"" + message + "\"}");
            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.End();
        }
        #endregion


        #region 退出系统
        private void Out(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("");
            //string name = context.Request.Params["name"].ToString();
            int id = Convert.ToInt32(context.Request["UserID"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                User user = db.User.FirstOrDefault(t => t.ID == id);
                if (user != null)
                {
                    context.Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
                    context.Response.Cookies["SysUserName"].Expires = DateTime.Now.AddDays(-1);
                    context.Response.Cookies["RealName"].Expires = DateTime.Now.AddDays(-1);
                    sb.Append("{\"result\":\"success\"}");
                    new SysLogDAO().AddLog(LogType.注销日志, "用户【" + user.RealName + "】于北京时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "注销登陆", Convert.ToInt32(user.ID));

                }
                else
                {
                    sb.Append("{\"result\":\"fail\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.End();
        }
        #endregion


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}