/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月13日 18时00分19秒
** 描    述:      用户信息页面
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

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;

namespace IFMP.sysmanage
{
    public partial class SysUserInfo : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public string SysID
        {
            get
            {
                return GetQueryString<string>("id", "");
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InfoBind();
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            User user = db.User.FirstOrDefault(t => t.ID == UserID && t.IsDel != true);
            if (user != null)
            {
                UserDetails userdetails = db.UserDetails.FirstOrDefault(t => t.UserID == UserID);
                this.ltl_SysUserName.Text = user.UserName;
                this.ltl_RealName.Text = user.RealName;
                this.ltl_EmpCode.Text = user.UserNumber;
                this.ltl_JobName.Text = userdetails.Job;
                this.ltl_CellPhone.Text = user.Cellphone;
                this.ltl_Sex.Text = Enum.GetName(typeof(IFMPLibrary.Enums.Sex), userdetails.Sex);
                this.ltl_UserLeaveType.Text = Enum.GetName(typeof(UserLeaveType), user.UserLeaveType);
                string depname = "";
                List<DepartmentUser> departuserlist = db.DepartmentUser.Where(t => t.UserID == userdetails.UserID).ToList();
                if (departuserlist.Count > 0)
                {
                    for (int i = 0; i < departuserlist.Count; i++)
                    {
                        int did = Convert.ToInt32(departuserlist[i].DepartmentID.ToString());
                        depname += db.Department.FirstOrDefault(t => t.ID == did).Name + ",";
                    }
                }
                this.ltl_DepID.Text = depname.TrimEnd(',');
                this.ltl_PostID.Text = db.Post.FirstOrDefault(t => t.ID == (db.PostUser.FirstOrDefault(m => m.UserID == userdetails.UserID)).PostID) == null ? "" : db.Post.FirstOrDefault(t => t.ID == (db.PostUser.FirstOrDefault(m => m.UserID == userdetails.UserID)).PostID).Name;
                this.ltl_Polity.Text = Enum.GetName(typeof(IFMPLibrary.Enums.Polity), userdetails.Polity);
                this.ltl_Nationality.Text = Enum.GetName(typeof(Nationality), userdetails.Nationality);
                this.ltl_Birthdate.Text = userdetails.BirthDate == null ? "" : Convert.ToDateTime(userdetails.BirthDate).ToString("yyyy-MM-dd");
                this.ltl_Begindate.Text = Convert.ToDateTime(userdetails.HireDate).ToString("yyyy-MM-dd");
                this.ltl_periodDay.Text = userdetails.ProbationDays.ToString();
                this.ltl_CorrectionDate.Text = userdetails.QualifiedDate == null ? "" : Convert.ToDateTime(userdetails.QualifiedDate).ToString("yyyy-MM-dd");
                this.ltl_UserState.Text = Enum.GetName(typeof(UserState), db.User.FirstOrDefault(t => t.ID == userdetails.UserID).UserState);
                this.ltl_UserType.Text = Enum.GetName(typeof(UserType), userdetails.UserType);
                this.ltl_Censusaddr.Text = userdetails.Address;
                this.ltl_Identity.Text = userdetails.Identity;
                if (userdetails.HeaderUrl != null && userdetails.HeaderUrl.ToString() != "")
                {
                    this.img_Photo.ImageUrl = userdetails.HeaderUrl.ToString();
                }
                else
                {
                    this.img_Photo.Visible = false;
                }
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                User model = db.User.FirstOrDefault(t => t.ID == UserID);
                if (this.txt_OldPwd.Text.Trim() == "" || this.txt_NewPwd.Text.Trim() == "" || this.txt_AgainPwd.Text.Trim() == "")
                {
                    ShowMessage("若需修改密码，请将密码信息填写完整");
                    return;
                }

                if (new BaseUtils().BuildPW(model.UserName, txt_OldPwd.Text) != model.Password)
                {
                    ShowMessage("原密码输入错误，请重新输入");
                    return;
                }

                if (txt_NewPwd.Text.Trim() != txt_AgainPwd.Text.Trim())
                {
                    ShowMessage("两次输入密码不一致，请重新输入");
                    return;
                }

                model.Password = new BaseUtils().BuildPW(model.UserName, txt_NewPwd.Text.Trim());//新密码

                db.SaveChanges();
                new SysLogDAO().AddLog(LogType.操作日志_修改, "修改密码", UserID);
                ShowMessage("密码修改成功！");
                InfoBind();
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