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
    public partial class LeaveAuditFlow : PageBase
    {

        #region 参数集合
        public int LeaveAuditID
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
                CommonFunction.BindEnum<LeaveState>(this.ddl_Audit, "-99");
                this.ddl_Audit.Items.Remove(new ListItem("未审核", "0"));
                this.ddl_Audit.Items.Remove(new ListItem("审核中", "1"));

                if (LeaveAuditID != 0)
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
                LeaveAudit LeaveAudit = db.LeaveAudit.FirstOrDefault(t => t.ID == LeaveAuditID);

                if (LeaveAudit != null)
                {
                    Role Role = null;
                    if (LeaveAudit.RoleID != null)
                    {
                        Role = db.Role.FirstOrDefault(t => t.ID == LeaveAudit.RoleID);
                    }
                    Leave Leave = db.Leave.FirstOrDefault(t => t.ID == LeaveAudit.LeaveID);
                    ltl_LeaveUser.Text = db.User.FirstOrDefault(t => t.ID == Leave.UserID).RealName;
                    ltl_BeginDate.Text = Leave.BeginDate.ToString("yyyy-MM-dd");
                    ltl_EndDate.Text = Leave.EndDate.ToString("yyyy-MM-dd");
                    ltl_Day.Text = Leave.Day.ToString();
                    ltl_LeaveType.Text = Enum.GetName(typeof(LeaveType), Leave.LeaveType);
                    ltl_Content.Text = Leave.Content;

                    if (Role == null || Role.Name == "总经理" || Role.Name == "生产副总")
                    {
                        //无需下一级
                        LeaveAuditUser.Visible = false;
                    }
                    else if (Role.Name == "主管班长")
                    {
                        //发送给车间主任
                        List<User> UserList = db.User.Where(t => db.UserRole.Where(m => db.Role.Where(k => k.Name == "车间主任").Select(k => k.ID).Contains(m.RoleID)).Select(m => m.UserID).Contains(t.ID)).ToList();
                        this.ddl_AuditUser.DataSource = UserList;
                        this.ddl_AuditUser.DataValueField = "ID";
                        this.ddl_AuditUser.DataTextField = "RealName";
                        this.ddl_AuditUser.DataBind();
                    }
                    else if (Role.Name == "车间主任")
                    {
                        List<User> UserList = db.User.Where(t => db.UserRole.Where(m => db.Role.Where(k => k.Name == "生产副总").Select(k => k.ID).Contains(m.RoleID)).Select(m => m.UserID).Contains(t.ID)).ToList();
                        this.ddl_AuditUser.DataSource = UserList;
                        this.ddl_AuditUser.DataValueField = "ID";
                        this.ddl_AuditUser.DataTextField = "RealName";
                        this.ddl_AuditUser.DataBind();
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
                LeaveState LeaveState = (LeaveState)Convert.ToInt32(this.ddl_Audit.SelectedValue);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //这里应该有权限，很重要
                    LeaveAudit LeaveAudit = db.LeaveAudit.FirstOrDefault(t => t.ID == LeaveAuditID);
                    if (LeaveAudit != null)
                    {
                        Role Role = null;
                        if (LeaveAudit.RoleID != null)
                        {
                            Role = db.Role.FirstOrDefault(t => t.ID == LeaveAudit.RoleID);
                        }
                        Leave Leave = db.Leave.FirstOrDefault(t => t.ID == LeaveAudit.LeaveID);
                        if (Role == null || Role.Name == "总经理" || Role.Name == "生产副总")
                        {
                            Leave.LeaveState = LeaveState;
                            LeaveAudit.AuditDate = DateTime.Now;
                            LeaveAudit.LeaveState = LeaveState;

                            if (LeaveState == LeaveState.通过)
                            {
                                Notice Notice = new Notice();
                                Notice.Contenet = "您有一条请假申请通过"
                                      + "|请假类型：" + Enum.GetName(typeof(LeaveType), Leave.LeaveType)
                                      + "|请假日期：" + Leave.BeginDate.ToString("yyyy-MM-dd") + "至" + Leave.EndDate.ToString("yyyy-MM-dd")
                                      + "|请假天数：" + Leave.Day
                                      + "|请假描述：" + Leave.Content
                                      + "|状态：通过";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.系统通知;
                                Notice.ReciveUserID = Leave.UserID;
                                Notice.SendUserID = UserID;
                                Notice.SourceID = Leave.ID;
                                Notice.SendDate = DateTime.Now;
                                db.Notice.Add(Notice);
                            }
                            else if (LeaveState == LeaveState.不通过)
                            {
                                Notice Notice = new Notice();
                                Notice.Contenet = "您有一条请假申请被驳回"
                                      + "|请假类型：" + Enum.GetName(typeof(LeaveType), Leave.LeaveType)
                                      + "|请假日期：" + Leave.BeginDate.ToString("yyyy-MM-dd") + "至" + Leave.EndDate.ToString("yyyy-MM-dd")
                                      + "|请假天数：" + Leave.Day
                                      + "|请假描述：" + Leave.Content
                                      + "|状态：驳回";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.系统通知;
                                Notice.ReciveUserID = Leave.UserID;
                                Notice.SendUserID = UserID;
                                Notice.SourceID = Leave.ID;
                                Notice.SendDate = DateTime.Now;
                                db.Notice.Add(Notice);
                            }
                            else
                            {
                                ShowMessage("请选择正确的审核结果");
                                return;
                            }

                        }else if (Role.Name == "主管班长" || Role.Name == "车间主任")
                        {
                            int ReciveUserID = Convert.ToInt32(ddl_AuditUser.SelectedValue);

                            LeaveAudit.AuditDate = DateTime.Now;
                            LeaveAudit.LeaveState = LeaveState;

                            if (LeaveState == LeaveState.通过)
                            {
                                Leave.LeaveState = LeaveState.审核中;

                                LeaveAudit NewLeaveAudit = new IFMPLibrary.Entities.LeaveAudit();
                                NewLeaveAudit.LeaveID = Leave.ID;
                                NewLeaveAudit.UserID = ReciveUserID;
                                NewLeaveAudit.SendDate = DateTime.Now;
                                if (Role.Name == "主管班长")
                                {
                                    NewLeaveAudit.RoleID = db.Role.FirstOrDefault(t => t.Name == "车间主任").ID;
                                }
                                else
                                {
                                    NewLeaveAudit.RoleID = db.Role.FirstOrDefault(t => t.Name == "生产副总").ID;
                                }
                                NewLeaveAudit.LeaveState = LeaveState.未审核;
                                db.LeaveAudit.Add(NewLeaveAudit);
                                //NewLeaveAudit.RoleID=
                                //NewLeaveAudit.

                                Notice Notice = new Notice();
                                Notice.Contenet = "当前有一条请假申请待审核"
                                      + "|发起人：" + db.User.FirstOrDefault(t => t.ID == Leave.UserID).RealName
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
                                db.Notice.Add(Notice);


                            }
                            else if (LeaveState == LeaveState.不通过)
                            {
                                Leave.LeaveState = LeaveState.不通过;
                                Notice Notice = new Notice();
                                Notice.Contenet = "您有一条请假申请被驳回"
                                      + "|请假类型：" + Enum.GetName(typeof(LeaveType), Leave.LeaveType)
                                      + "|请假日期：" + Leave.BeginDate.ToString("yyyy-MM-dd") + "至" + Leave.EndDate.ToString("yyyy-MM-dd")
                                      + "|请假天数：" + Leave.Day
                                      + "|请假描述：" + Leave.Content
                                      + "|状态：驳回";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.系统通知;
                                Notice.ReciveUserID = Leave.UserID;
                                Notice.SendUserID = UserID;
                                Notice.SourceID = Leave.ID;
                                Notice.SendDate = DateTime.Now;
                                db.Notice.Add(Notice);
                            }
                            else
                            {
                                ShowMessage("请选择正确的审核结果");
                                return;
                            }
                        }
                        db.SaveChanges();
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "请假审核", UserID);
                        ShowMessage();
                    }
                    else
                    {
                        ShowMessage("找不到该请假审核纪录");
                        return;
                    }
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