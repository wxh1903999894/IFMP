/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 15时13分19秒
** 描    述:      用户编辑页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.IO;


using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;


namespace IFMP.sysmanage
{
    public partial class EmployeeEdit : PageBase
    {

        #region 参数集合
        public int UID
        {
            get
            {
                return GetQueryString<int>("id", 0);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<Sex>(this.ddl_Sex, "-99");
                CommonFunction.BindEnum<Polity>(this.ddl_Polity, "-99");
                CommonFunction.BindEnum<Nationality>(this.ddl_Nationality, "-99");
                CommonFunction.BindEnum<UserType>(this.ddl_UserType, "-99");
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && t.IsAdmin).ToList();
                    this.ddl_DepID.DataSource = DepartmentList;
                    this.ddl_DepID.DataValueField = "ID";
                    this.ddl_DepID.DataTextField = "Name";
                    this.ddl_DepID.DataBind();
                    this.ddl_DepID.Items.Insert(0, new ListItem("--请选择--", "-2"));

                    List<Post> postList = db.Post.Where(t => t.IsDel != true).ToList();
                    this.ddl_PostID.DataSource = postList;
                    this.ddl_PostID.DataValueField = "ID";
                    this.ddl_PostID.DataTextField = "Name";
                    this.ddl_PostID.DataBind();
                    this.ddl_PostID.Items.Insert(0, new ListItem("--请选择--", "-2"));
                }

                if (UID != 0)
                {
                    InfoBind();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                User user = db.User.FirstOrDefault(t => t.ID == UID);
                if (user != null)
                {
                    this.ltl_UserNumber.Text = user.UserNumber ?? "";
                    this.ltl_SysUserName.Text = user.UserName;
                    this.ltl_RealName.Text = user.RealName;
                    this.ltl_CellPhone.Text = user.Cellphone;
                    this.ltl_UserState.Text = Enum.GetName(typeof(UserState), user.UserState);
                    this.ddl_DepID.SelectedValue = db.DepartmentUser.FirstOrDefault(t => t.UserID == user.ID).DepartmentID.ToString();
                    this.ddl_PostID.SelectedValue = db.PostUser.FirstOrDefault(t => t.UserID == user.ID).PostID.ToString();
                }
                UserDetails udetail = db.UserDetails.FirstOrDefault(t => t.UserID == UID);
                if (udetail != null)
                {
                    this.ddl_Sex.SelectedValue = ((int)udetail.Sex).ToString();
                    this.txt_JobName.Text = udetail.Job;
                    this.ddl_Nationality.SelectedValue = ((int)udetail.Nationality).ToString();
                    this.ddl_Polity.SelectedValue = ((int)udetail.Polity).ToString();
                    this.txt_BirthDate.Text = udetail.BirthDate == null ? "" : Convert.ToDateTime(udetail.BirthDate).ToString("yyyy-MM-dd");
                    this.txt_Address.Text = udetail.Address;
                    this.txt_HireDate.Text = Convert.ToDateTime(udetail.HireDate).ToString("yyyy-MM-dd");
                    this.txt_ProbationDays.Text = udetail.ProbationDays.ToString();
                    this.txt_QualifiedDate.Text = udetail.QualifiedDate == null ? "" : Convert.ToDateTime(udetail.QualifiedDate).ToString("yyyy-MM-dd");
                    this.ddl_UserType.SelectedValue = ((int)udetail.UserType).ToString();
                    if (udetail.HeaderUrl != null && udetail.HeaderUrl.ToString() != "")
                    {
                        this.img.Visible = true;
                        this.img.ImageUrl = udetail.HeaderUrl.ToString();
                        this.hf_UpFile.Value = udetail.HeaderUrl.ToString();
                    }
                }
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //这里应该有权限，很重要
                    User user = db.User.FirstOrDefault(t => t.ID == UID);
                    string message = "";

                    UserDetails udetail = db.UserDetails.FirstOrDefault(t => t.IsDel != true && t.UserID == user.ID);
                    udetail.Sex = (Sex)Convert.ToInt32(this.ddl_Sex.SelectedValue.ToString());
                    udetail.Nationality = (Nationality)Convert.ToInt32(this.ddl_Nationality.SelectedValue.ToString());
                    udetail.Polity = (Polity)Convert.ToInt32(this.ddl_Polity.SelectedValue.ToString());
                    if (!string.IsNullOrEmpty(this.txt_BirthDate.Text.Trim()))
                        udetail.BirthDate = Convert.ToDateTime(this.txt_BirthDate.Text.ToString());
                    udetail.Address = this.txt_Address.Text.ToString().Trim();
                    udetail.Job = this.txt_JobName.Text.ToString();
                    udetail.HireDate = Convert.ToDateTime(this.txt_HireDate.Text.ToString());
                    if (new BaseUtils().GetRegex(this.txt_ProbationDays.Text.ToString(), RegexType.非负整数) == false)
                    {
                        ShowMessage("请输入大于等于0的整数");
                        return;
                    }
                    udetail.ProbationDays = this.txt_ProbationDays.Text == "" ? 0 : Convert.ToInt32(this.txt_ProbationDays.Text.ToString());
                    if (!string.IsNullOrEmpty(this.txt_QualifiedDate.Text.Trim()))
                        udetail.QualifiedDate = Convert.ToDateTime(this.txt_QualifiedDate.Text);
                    udetail.UserType = (UserType)Convert.ToInt32(this.ddl_UserType.SelectedValue.ToString());
                    #region 上传图片
                    if (this.fl_UpFile.HasFile)
                    {
                        int upsize = 4000000;
                        try
                        {
                            upsize = Convert.ToInt32(ConfigurationManager.AppSettings["upsize"].ToString());
                        }
                        catch (Exception) { }

                        string attaurl = "";
                        string pname = "/userphoto/";
                        string path = System.Web.HttpContext.Current.Server.MapPath(pname);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string name = this.fl_UpFile.PostedFile.FileName.ToString();
                        string[] filename = name[name.Length - 1].ToString().Split('.');

                        string filepath = path + name;

                        if (this.fl_UpFile.PostedFile.ContentLength < upsize)
                        {
                            this.fl_UpFile.SaveAs(filepath);

                            int j = filepath.IndexOf("userphoto");
                            attaurl = filepath.Substring(j - 1);
                        }
                        udetail.HeaderUrl = attaurl;
                    }
                    else
                    {
                        udetail.HeaderUrl = this.hf_UpFile.Value;
                    }
                    #endregion
                    db.SaveChanges();

                    DepartmentUser DepartmentUser = db.DepartmentUser.FirstOrDefault(t => t.UserID == user.ID && db.Department.Where(m => m.IsDel != true && m.IsAdmin).Select(m => m.ID).Contains(t.DepartmentID));
                    DepartmentUser.DepartmentID = Convert.ToInt32(this.ddl_DepID.SelectedValue);
                    DepartmentUser.UserID = user.ID;
                    db.SaveChanges();

                    PostUser postuser = db.PostUser.FirstOrDefault(t => t.UserID == user.ID && db.Post.Where(m => m.IsDel != true).Select(m => m.ID).Contains(t.PostID));
                    postuser.PostID = Convert.ToInt32(this.ddl_PostID.SelectedValue);
                    postuser.UserID = user.ID;
                    db.SaveChanges();

                    if (db.User.FirstOrDefault(t => t.UserName == user.UserName && t.ID != user.ID) != null)
                    {
                        ShowMessage("用户名已存在，请修改后重新添加");
                        return;
                    }
                    message = "修改用户名为【" + user.UserName + "】的用户信息";


                    ShowMessage();
                    new SysLogDAO().AddLog(LogType.操作日志_添加, message, UserID);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}