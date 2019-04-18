/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 11时40分16秒
** 描    述:      PageBase
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using GK.IFMP.DAL;
using GK.IFMP.Entities;

namespace GK.IFMP.Common
{
    public class PageBase : System.Web.UI.Page
    {
        #region 页面加载
        protected override void OnLoad(EventArgs e)
        {
            string id = GetCookie<string>("UserID");
            string baseclassid = GetCookie<string>("BaseClassID");
            if (id == null || id == "")
            {
                if (baseclassid == null || baseclassid == "")
                {
                    CookieInvalidTo();
                }
            }
            //if (pwd == CommonFunction.Encrypt("888888"))
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "<script>alert('系统提示：请先修改初始化密码！');</script>");
            //    Page.ClientScript.RegisterStartupScript(GetType(), "", " window.location.href= '../SysUserInfo.aspx'", true);
            //}
            base.OnLoad(e);
        }
        #endregion

        #region 会话信息
        /// <summary>
        /// 用户ID
        /// </summary>
        protected internal int UserID
        {
            get
            {
                int result = GetCookie<int>("UserID");
                if (result == 0)
                {
                    CookieInvalidTo();
                }
                return result;
            }
        }

        protected internal int BaseClassID
        {
            get
            {
                int result = GetCookie<int>("BaseClassID");
                if (result == 0)
                {
                    CookieInvalidTo();
                }
                return result;
            }
        }

        //protected internal string UserID
        //{
        //    get
        //    {
        //        string result = GetCookie<string>("UserID");
        //        if (string.IsNullOrEmpty(result))
        //        {
        //            CookieInvalidTo();
        //        }
        //        return 0;
        //    }
        //}
        /// <summary>
        /// 用户名
        /// </summary>
        protected string SysUserName
        {
            get
            {
                string result = Server.UrlDecode(GetCookie<string>("SysUserName"));

                if (result == null || result == "")
                {

                    CookieInvalidTo();
                }
                return result;
            }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        protected string UserRealName
        {
            get
            {
                string result = Server.UrlDecode(GetCookie<string>("RealName"));

                if (result == null || result == "")
                {

                    CookieInvalidTo();
                }
                return result;
            }
        }
        #endregion

        #region 获取QueryString值
        /// <summary>
        /// 获取QueryString值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryName">Key</param>
        /// <returns></returns>
        protected T GetQueryString<T>(string queryName)
        {
            if ((typeof(T).FullName == "System.String"))
            {
                return GetQueryString(queryName, (T)Convert.ChangeType(string.Empty, typeof(T)));
            }

            return GetQueryString(queryName, default(T));
        }

        /// <summary>
        /// 获取QueryString值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryName">Key</param>
        /// <param name="defultValue">默认值</param>
        /// <returns></returns>
        protected T GetQueryString<T>(string queryName, T defultValue)
        {
            T stateValue = defultValue;

            try
            {
                if (Request.QueryString[queryName] != null)
                    stateValue = (T)Convert.ChangeType(Request.QueryString[queryName], typeof(T));
            }
            catch
            {
                stateValue = defultValue;
            }
            return stateValue;
        }
        #endregion

        #region 获取和设置Cookie值
        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="cookieName">名称</param>
        /// <param name="cookieValue">值</param>
        protected void SetCookie(string cookieName, string cookieValue)
        {
            SetCookie(cookieName, cookieValue, string.Empty, string.Empty, null);
        }

        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="cookieName">名称</param>
        /// <param name="cookieValue">值</param>
        /// <param name="cookiePath">虚拟路径（默认：empty）</param>
        /// <param name="cookieDomain">关联的域（默认：empty）</param>
        /// <param name="cookieExpires">过期时间（默认：null）</param>
        protected void SetCookie(string cookieName, string cookieValue, string cookiePath, string cookieDomain, DateTime? cookieExpires)
        {
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);

            if (!string.IsNullOrEmpty(cookiePath))
            {
                cookie.Path = cookiePath;
            }

            if (!string.IsNullOrEmpty(cookieDomain))
            {
                cookie.Domain = cookieDomain;
            }

            if (cookieExpires != null)
            {
                cookie.Expires = (DateTime)cookieExpires;
            }

            Response.Cookies.Add(cookie);
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
        #endregion

        #region Cookie失效跳转页面
        /// <summary>
        /// Cookie失效跳转页面
        /// </summary>
        protected void CookieInvalidTo()
        {
            Response.Clear();
            Response.Write(
               "<script language='javascript'>try{parent.parent.location.href = '../Default.aspx';} catch(e){ parent.location.href = 'Default.aspx'; }</script>"
              );
            Response.End();
        }
        #endregion

        #region 消息提醒
        /// <summary>
        /// 消息提醒
        /// </summary>
        /// <param name="sMessage"></param>
        public void ShowMessage(string sMessage)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "<script>alert('系统提示：" + sMessage + "！');</script>");
            return;
        }

        /// <summary>
        /// 消息提醒,并跳转
        /// </summary>
        /// <param name="sMessage"></param>
        public void ShowMessage(string sMessage, string script)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "", "alert('" + sMessage + "！');" + script, true);
            return;
        }
        #endregion

        #region 消息提醒成功
        /// <summary>
        /// 消息提醒成功
        /// </summary>
        /// <param name="sMessage"></param>
        public void ShowMessage()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "", "alert('提交成功！');winclose();", true);

        }


        public void ShowMessagePad()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "", "alert('提交成功！');window.location.href='TaskList.aspx';", true);
        }
        /// <summary>
        /// 运行纯脚本
        /// </summary>
        /// <param name="sMessage"></param>
        public void ShowScript(string script)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "", script, true);
        }
        #endregion

        #region 截取字符
        /// <summary>
        /// 截取字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetCutStr(object str, int length)
        {
            if (str.ToString().Length > length)
            {
                return str.ToString().Substring(0, length).TrimEnd(',') + "...";
            }
            else
            {
                return str.ToString().TrimEnd(',');
            }
        }
        #endregion

        #region 页面按钮权限显示
        protected override void OnLoadComplete(EventArgs e)
        {
            //SetPageState();
        }
        /// <summary>
        /// 页面按钮权限显示
        /// </summary>
        /// <param name="userid"></param>
        public void SetPageState()
        {
            //SysRoleDAL roleDAL = new SysRoleDAL();
            //SysModuleDAL smDAL = new SysModuleDAL();
            //try
            //{
            //    int nPageControls = Page.Controls.Count;
            //    string path = HttpContext.Current.Request.Url.PathAndQuery;
            //    string[] idlist = path.Split(new Char[] { '=' });
            //    int index = idlist.Length - 1;
            //    if (path.IndexOf('=') > 0 && path.IndexOf("SysMUID") > 0 && path.IndexOf("ModuleEdit.aspx") < 0)
            //    //if (path.IndexOf('=') > 0 && path.IndexOf("ModuleEdit.aspx") < 0)
            //    {
            //        VisibleButton();
            //        System.Web.UI.Control lb = Page.Controls[1].FindControl("lbl_Menuname");
            //        System.Web.UI.Control lb1 = Page.Controls[1].FindControl("lbl_ParentMenu");
            //        SysModuleEntity sm = smDAL.GetObj(Convert.ToInt32(idlist[index].ToString()));
            //        if (lb != null && lb is Label)
            //        {
            //            Label lbl = (Label)lb;
            //            lbl.Text = sm.ModuleName;
            //        }
            //        if (lb1 != null && lb1 is Label)
            //        {
            //            Label lblparent = (Label)lb1;
            //            lblparent.Text = sm.ParentName;
            //        }

            //        DataTable dvbuttonlist = roleDAL.GetRoleButton(UserID, Convert.ToInt32(idlist[index].ToString()));
            //        if (dvbuttonlist.Rows.Count > 0)
            //        {
            //            foreach (DataRow row in dvbuttonlist.Rows)
            //            {
            //                for (int j = 0; j < Page.Controls.Count; j++)
            //                {
            //                    #region 搜索控制按钮
            //                    System.Web.UI.Control control = Page.Controls[j].FindControl(row["ButtonCode"].ToString());
            //                    if (control != null)
            //                    {
            //                        control.Visible = true;
            //                    }
            //                    if (control is HyperLink)
            //                    {
            //                        control.Visible = true;
            //                    }
            //                    if (control is Repeater)
            //                    {
            //                        Repeater repeater = (Repeater)control;
            //                        for (int x = 0; x < repeater.Items.Count; x++)
            //                        {
            //                            try
            //                            {
            //                                LinkButton Btn_Page = (LinkButton)repeater.Items[x].FindControl(row["ButtonCode"].ToString());
            //                                if (Btn_Page.Text != "")
            //                                {
            //                                    Btn_Page.Visible = true;
            //                                }
            //                            }
            //                            catch (Exception)
            //                            { }
            //                            try
            //                            {
            //                                HyperLink Btn_Page = (HyperLink)repeater.Items[x].FindControl(row["ButtonCode"].ToString());
            //                                if (Btn_Page.Text != "")
            //                                {
            //                                    Btn_Page.Visible = true;
            //                                }
            //                            }
            //                            catch (Exception)
            //                            { }
            //                            try
            //                            {
            //                                HtmlAnchor Btn_Page = (HtmlAnchor)repeater.Items[x].FindControl(row["ButtonCode"].ToString());
            //                                if (Btn_Page.InnerHtml != "")
            //                                {
            //                                    Btn_Page.Visible = true;
            //                                }
            //                            }
            //                            catch (Exception)
            //                            { }
            //                        }
            //                    }
            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //}
            ////}
            //catch (Exception ex)
            //{
            //    throw (ex);
            //    //ShowMessages(ex.Message);
            //}
        }

        /// <summary>
        /// 隐藏所有页面按钮
        /// </summary>
        public void VisibleButton()
        {
            for (int j = 0; j < Page.Controls.Count; j++)
            {
                foreach (System.Web.UI.Control control in Page.Controls[j].Controls)
                {
                    if (control is ImageButton)
                    {
                        ImageButton Btn_Page = (ImageButton)control;
                        if (Btn_Page.ID != "btnsear" && Btn_Page.ID != "btn_Sumbit" && Btn_Page.ID != "imgbtn_Import" && Btn_Page.ID != "imgbtn_Sumbit_Click")
                            Btn_Page.Visible = false;
                    }
                    if (control is Button)
                    {
                        Button Btn_Page = (Button)control;
                        if (Btn_Page.ID != "btn_Sumbit" && Btn_Page.ID != "btn_Cancel" && Btn_Page.ID != "btn_Deleted" && Btn_Page.ID != "btn_Save" && Btn_Page.ID != "bt_ok")
                            Btn_Page.Visible = false;
                    }
                    if (control is LinkButton)
                    {
                        LinkButton Btn_Page = (LinkButton)control;
                        if (Btn_Page.ID.IndexOf("lbtn_example") == 0 || Btn_Page.ID.IndexOf("lbtn_Wednesday") == 0 || Btn_Page.ID.IndexOf("lbtn_Friday") == 0 || Btn_Page.ID.IndexOf("lbtn_Saturday") == 0 || Btn_Page.ID.IndexOf("lbtn_Staff") == 0 || Btn_Page.ID.IndexOf("lbt_XM") == 0 || Btn_Page.ID.IndexOf("lbt_XB") == 0 || Btn_Page.ID.IndexOf("lbt_CSNY") == 0 || Btn_Page.ID.IndexOf("lbt_XL") == 0 || Btn_Page.ID.IndexOf("lbt_CJGZSJ") == 0 || Btn_Page.ID.IndexOf("lbt_JSZG") == 0 || Btn_Page.ID.IndexOf("lbt_ZYJSZW") == 0)
                        {
                            Btn_Page.Visible = true;
                        }
                        else
                        {
                            Btn_Page.Visible = false;
                        }
                    }
                    if (control is HyperLink)
                    {
                        HyperLink Btn_Page = (HyperLink)control;
                        Btn_Page.Visible = false;
                    }
                    if (control is Repeater)
                    {
                        Repeater repeater = (Repeater)control;
                        for (int x = 0; x < repeater.Items.Count; x++)
                        {
                            foreach (System.Web.UI.Control rpcontrol in repeater.Items[x].Controls)
                            {
                                if (rpcontrol is LinkButton)
                                {
                                    LinkButton Btn_Page = (LinkButton)rpcontrol;
                                    if (Btn_Page.ID != "lbtn_Edit" && Btn_Page.ID != "lbtn_Detail" && Btn_Page.ID != "lbtn_Role" && Btn_Page.ID != "lbtn_Reply" && Btn_Page.ID != "lbtn_RoomSet" && Btn_Page.ID != "lbtn_SubjectSet" && Btn_Page.ID != "lbtn_Delete" && Btn_Page.ID != "lbtn_Audit" && Btn_Page.ID != "lbtn_Borrow" && Btn_Page.ID != "lbtn_Scrap" && Btn_Page.ID != "lbtn_Purchase" && Btn_Page.ID != "lbtn_ArrangeDor" && Btn_Page.ID != "lbtn_Admin" && Btn_Page.ID != "lbtn_Apply" && Btn_Page.ID != "lbtn_Evaluate" && Btn_Page.ID != "lbtn_Publish" && Btn_Page.ID != "lbtn_OutDor" && Btn_Page.ID != "lbtn_Endorse" && Btn_Page.ID != "lbtn_File" && Btn_Page.ID != "lbtn_Sick" && Btn_Page.ID != "lbtn_Use" && Btn_Page.ID != "lbtn_Comp" && Btn_Page.ID != "lbtn_Leader" && Btn_Page.ID != "lbtn_Add" && Btn_Page.ID != "lbtn_SetGrades" && Btn_Page.ID != "lbtn_Grades" && Btn_Page.ID != "lbtn_Setting" && Btn_Page.ID != "lbtn_PayGood" && Btn_Page.ID != "lbtn_Gradution" && Btn_Page.ID != "lbtn_Assigned" && Btn_Page.ID != "lbtn_Invigilator" && Btn_Page.ID != "lbtn_SubArrange" && Btn_Page.ID != "lbtn_ExamMarking" && Btn_Page.ID != "lbtn_SName" && Btn_Page.ID != "lbtn_SchoolRoll")
                                        Btn_Page.Visible = false;
                                }
                                if (rpcontrol is HtmlAnchor)
                                {
                                    HtmlAnchor Btn_Page = (HtmlAnchor)rpcontrol;
                                    Btn_Page.Visible = false;
                                }
                                if (rpcontrol is DropDownList)
                                {
                                    DropDownList Btn_Page = (DropDownList)rpcontrol;
                                    if (Btn_Page.ID != "ddl_TID")
                                        Btn_Page.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
