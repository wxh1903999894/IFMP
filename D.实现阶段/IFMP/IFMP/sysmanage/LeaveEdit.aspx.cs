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
    public partial class LeaveEdit : PageBase
    {

        #region 参数集合
        public int LeaveID
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
                CommonFunction.BindEnum<LeaveType>(this.ddl_LeaveType, "-99");

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    User User = db.User.FirstOrDefault(t => t.ID == UserID);
                    if (User.UserLeaveType == UserLeaveType.车间人员)
                    {
                        List<User> UserList = db.User.Where(t => db.UserRole.Where(m => db.Role.Where(k => k.Name == "主管班长").Select(k => k.ID).Contains(m.RoleID)).Select(m => m.UserID).Contains(t.ID)).ToList();
                        this.ddl_AuditUser.DataSource = UserList;
                        this.ddl_AuditUser.DataValueField = "ID";
                        this.ddl_AuditUser.DataTextField = "RealName";
                        this.ddl_AuditUser.DataBind();

                        hf_Type.Value = ((int)UserLeaveType.车间人员).ToString();
                    }

                    if (User.UserLeaveType == UserLeaveType.后勤人员)
                    {
                        List<User> UserList = db.User.Where(t => db.UserRole.Where(m => db.Role.Where(k => k.Name == "总经理").Select(k => k.ID).Contains(m.RoleID)).Select(m => m.UserID).Contains(t.ID)).ToList();
                        this.ddl_AuditUser.DataSource = UserList;
                        this.ddl_AuditUser.DataValueField = "ID";
                        this.ddl_AuditUser.DataTextField = "RealName";
                        this.ddl_AuditUser.DataBind();

                        hf_Type.Value = ((int)UserLeaveType.后勤人员).ToString();
                    }

                }

                if (LeaveID != 0)
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
                Leave Leave = db.Leave.FirstOrDefault(t => t.ID == LeaveID);
                if (Leave != null)
                {
                    txt_BeginDate.Text = Leave.BeginDate.ToString("yyyy-MM-dd");
                    txt_EndDate.Text = Leave.EndDate.ToString("yyyy-MM-dd");
                    txt_Day.Text = Leave.Day.ToString();
                    ddl_LeaveType.SelectedValue = Leave.LeaveType.ToString();
                    txt_Content.Text = Leave.Content;
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
                    Leave Leave = new Leave();
                    User User = db.User.FirstOrDefault(t => t.ID == UserID);

                    Leave.BeginDate = Convert.ToDateTime(txt_BeginDate.Text);
                    Leave.EndDate = Convert.ToDateTime(txt_EndDate.Text);
                    Leave.Day = Convert.ToDouble(txt_Day.Text);
                    Leave.LeaveType = (LeaveType)Convert.ToInt32(ddl_LeaveType.SelectedValue);
                    Leave.Content = txt_Content.Text;
                    Leave.IsDel = false;
                    Leave.LeaveState = LeaveState.未审核;
                    Leave.UserID = User.ID;
                    Leave.CreateDate = DateTime.Now;
                    //合法判断
                    if (Leave.Day <= 0)
                    {
                        ShowMessage("请输入合法的天数");
                        return;
                    }

                    //判断审核流程
                    if (Leave.Day <= 7)
                    {
                        Department Department = db.Department.FirstOrDefault(t => db.DepartmentUser.Where(m => m.UserID == UserID).Select(m => m.DepartmentID).Contains(t.ID) && t.IsAdmin);
                        if (Department == null)
                        {
                            ShowMessage("未查找到对应部门，请联系人事部门或系统管理员");
                            return;
                        }
                        //如果负责人就是自己，由上级负责人审核？
                        db.Leave.Add(Leave);
                        db.SaveChanges();
                        LeaveAudit LeaveAudit = new LeaveAudit();
                        LeaveAudit.LeaveID = Leave.ID;
                        LeaveAudit.SendDate = DateTime.Now;

                        LeaveAudit.UserID = Department.MasterUserID;
                        LeaveAudit.LeaveState = LeaveState.未审核;
                        db.LeaveAudit.Add(LeaveAudit);


                        //添加通知
                        Notice Notice = new Notice();
                        Notice.Contenet = "当前有一条请假申请待审核"
                              + "|发起人：" + User.RealName
                              + "|请假类型：" + Enum.GetName(typeof(LeaveType), Leave.LeaveType)
                              + "|请假日期：" + Leave.BeginDate.ToString("yyyy-MM-dd") + "至" + Leave.EndDate.ToString("yyyy-MM-dd")
                              + "|请假天数：" + Leave.Day
                              + "|请假描述：" + Leave.Content
                              + "|状态：待审核";
                        Notice.IsSend = false;
                        Notice.NoticeType = NoticeType.系统通知;
                        Notice.ReciveUserID = Department.MasterUserID;
                        Notice.SendUserID = UserID;
                        Notice.SourceID = Leave.ID;
                        Notice.SendDate = DateTime.Now;

                        //Notice.URL = ParaUtils.SiteURL + "/jfz/app/TaskAudit.html?flag=1&id=" + ScoreTask.ID;
                        db.Notice.Add(Notice);
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "发起请假", UserID);
                        db.SaveChanges();
                       
                    }
                    else
                    {
                        int ReciveUserID = 0;
                        //如果负责人就是自己，由上级负责人审核？
                        db.Leave.Add(Leave);
                        db.SaveChanges();
                        LeaveAudit LeaveAudit = new LeaveAudit();
                        if (User.UserLeaveType == UserLeaveType.后勤人员)
                        {
                            //发送给总经理
                            ReciveUserID = Convert.ToInt32(ddl_AuditUser.SelectedValue);
                            if (db.UserRole.FirstOrDefault(t => t.UserID == ReciveUserID && t.RoleID == db.Role.FirstOrDefault(m => m.Name == "总经理").ID) == null)
                            {
                                ShowMessage("请选择正确的审核人");
                                return;
                            }
                            LeaveAudit.RoleID = db.Role.FirstOrDefault(t => t.Name == "总经理").ID;
                        }

                        if (User.UserLeaveType == UserLeaveType.车间人员)
                        {
                            //发送给班长
                            ReciveUserID = Convert.ToInt32(ddl_AuditUser.SelectedValue);
                            if (db.UserRole.FirstOrDefault(t => t.UserID == ReciveUserID && t.RoleID == db.Role.FirstOrDefault(m => m.Name == "主管班长").ID) == null)
                            {
                                ShowMessage("请选择正确的审核人");
                                return;
                            }
                            LeaveAudit.RoleID = db.Role.FirstOrDefault(t => t.Name == "主管班长").ID;
                        }


                        LeaveAudit.LeaveID = Leave.ID;
                        LeaveAudit.SendDate = DateTime.Now;

                        LeaveAudit.UserID = ReciveUserID;
                        LeaveAudit.LeaveState = LeaveState.未审核;
                        db.LeaveAudit.Add(LeaveAudit);


                        //添加通知
                        Notice Notice = new Notice();
                        Notice.Contenet = "当前有一条请假申请待审核"
                              + "|发起人：" + User.RealName
                              + "|请假类型：" + Enum.GetName(typeof(LeaveType), Leave.LeaveType)
                              + "|请假日期：" + Leave.BeginDate.ToString("yyyy-MM-dd") + "至" + Leave.EndDate.ToString("yyyy-MM-dd")
                              + "|请假天数：" + Leave.Day
                              + "|请假描述：" + Leave.Content
                              + "|状态：待审核";
                        Notice.IsSend = false;
                        Notice.NoticeType = NoticeType.系统通知;
                        Notice.ReciveUserID = ReciveUserID;
                        Notice.SendUserID = UserID;
                        Notice.SourceID = Leave.ID;
                        Notice.SendDate = DateTime.Now;

                        //Notice.URL = ParaUtils.SiteURL + "/jfz/app/TaskAudit.html?flag=1&id=" + ScoreTask.ID;
                        db.Notice.Add(Notice);
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "发起请假", UserID);
                        db.SaveChanges();
                     
                    }
                    ShowMessage();
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