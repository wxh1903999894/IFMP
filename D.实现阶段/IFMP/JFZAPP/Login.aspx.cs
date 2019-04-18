using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using IFMPLibrary.DAO;

namespace JFZAPP
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["SysUserName"] != null && Request.Cookies["SysUserPwd"] != null)
            {
                bool result = new AccountDAO().ValidatedAccountPage(Request.Cookies["SysUserName"].ToString(), Request.Cookies["SysUserPwd"].ToString());
                if (result)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'index.html';", true);
                }
                else
                {
                    ClearCookies();
                }
            }
            else
            {
                ClearCookies();
            }
        }

        #region 清理COOkies
        /// <summary>
        /// 清理COOkies
        /// </summary>
        public void ClearCookies()
        {
            Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["SysUserName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["RealName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["UserFace"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["SysUserPwd"].Expires = DateTime.Now.AddDays(-1);

            //Response.Cookies.Remove("UserID");
            //Response.Cookies.Remove("SysUserName");
            //Response.Cookies.Remove("RealName");
            //Response.Cookies.Remove("UserFace");
            //Response.Cookies.Remove("SysUserPwd");
        }
        #endregion


        #region 按钮点击登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Login_Click(object sender, EventArgs e)
        {
            string pwd = this.txt_Pwd.Text.Trim();
            string username = this.txt_UserID.Text.Trim();

            bool result = false;
            pwd = new BaseUtils().BuildPW(username, pwd);
            string message = "";
            using (IFMPDBContext db = new IFMPDBContext())
            {
                User user = db.User.FirstOrDefault(t => t.UserName == username && t.Password == pwd);
                if (user != null)
                {
                    if (user.IsDel != true &&user.UserState != UserState.离职)
                    {
                        result = true;
                        Response.Cookies["UserID"].Value = user.ID.ToString();
                        Response.Cookies["SysUserName"].Value = user.UserName;
                        Response.Cookies["RealName"].Value = user.RealName;
                        //Response.Cookies["UserFace"].Value = "";
                        Response.Cookies["SysUserPwd"].Value = user.Password;
                        Response.Cookies["DepIds"].Value = db.DepartmentUser.FirstOrDefault(t => t.UserID == user.ID).DepartmentID.ToString();
                        //Response.Cookies["UserType"].Value = sysuser.UserType;
                    }
                    else
                    {
                        message = "alert('该用户状态为离职状态，请联系系统管理员！');";
                    }
                }
                else
                {
                    message = "alert('用户名或密码错误！');";
                }
            }

            if (result)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", "window.location.href = 'index.html';", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", message, true);
            }

        }
        #endregion
    }
}