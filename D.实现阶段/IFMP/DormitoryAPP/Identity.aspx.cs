using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DormitoryAPP
{
    public partial class Identity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string result = GetCookie<string>("UserID");
                if (string.IsNullOrEmpty(result))
                {
                    CookieInvalidTo();
                }
                else
                {
                    string a = Request.Url.AbsoluteUri;
                    string[] aa = a.Split('/');
                    string url = Uri.EscapeDataString(aa[aa.Length - 1]);
                    Response.Clear();
                    Response.Write(
                      "<script language='javascript'>try{parent.parent.location.href = '" + url + "';} catch(e){ parent.location.href = '" + url + "'; }</script>"
                     );
                    Response.End();
                }
            }
        }
        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cookieName">Key</param>
        /// <returns></returns>
        protected T GetCookie<T>(string cookieName)
        {
            if ((typeof(T).FullName == "System.String"))
            {
                return GetCookie(cookieName, (T)Convert.ChangeType(string.Empty, typeof(T)));
            }

            return GetCookie(cookieName, default(T));
        }
        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cookieName">Key</param>
        /// <param name="defultValue">默认值</param>
        /// <returns></returns>
        protected T GetCookie<T>(string cookieName, T defultValue)
        {
            T cookieValue = defultValue;

            try
            {
                cookieValue = (T)Convert.ChangeType(Request.Cookies[cookieName].Value, typeof(T));
            }
            catch
            { }

            return cookieValue;
        }

        #region Cookie失效跳转页面
        /// <summary>
        /// Cookie失效跳转页面
        /// </summary>
        protected void CookieInvalidTo()
        {
            string a = Request.Url.AbsoluteUri;
            string[] aa = a.Split('/');
            string url = Uri.EscapeDataString(aa[aa.Length - 1]);
            //string url = "aspx";
            Response.Clear();
            Response.Write(
              "<script language='javascript'>try{parent.parent.location.href = 'DDLogin.aspx?rurl=" + url + "';} catch(e){ parent.location.href = 'DDLogin.aspx?rurl=" + url + "'; }</script>"
             );
            //Response.Write(
            //   "<script language='javascript'>try{parent.parent.location.href = 'WXOAuth.aspx?id=Main&url='" + url + ";} catch(e){ parent.location.href = 'WXOAuth.aspx?id=Main&url='" + url + "; }</script>"
            //  );
            Response.End();
        }
        #endregion
    }
}