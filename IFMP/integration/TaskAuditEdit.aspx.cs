using System;
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

namespace IFMP.integration
{
    public partial class TaskAuditEdit : PageBase
    {
        #region 参数集合
        public int ScoreTaskID
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
                CommonFunction.BindEnum<AuditState>(this.ddl_Audit, "-99");
                this.ddl_Audit.Items.Remove(new ListItem("待初审", "1"));
                this.ddl_Audit.Items.Remove(new ListItem("待终审", "2"));
                this.ddl_Audit.Items.Remove(new ListItem("确认完成", "5"));
            }
        }
        #endregion


        #region 提交
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    ScoreTask ScoreTask = db.ScoreTask.FirstOrDefault(t => t.ID == ScoreTaskID);
                    if (ScoreTask != null)
                    {
                        if (ScoreTask.AuditState == AuditState.待初审 && ScoreTask.FirstAuditUserID == UserID)
                        {

                            if (Convert.ToInt32(this.ddl_Audit.SelectedValue) == (int)AuditState.通过)
                            {
                                ScoreTask.AuditState = AuditState.待终审;

                                Notice Notice = new Notice();
                                Notice.Contenet = "当前有一条任务需要审核|主题：" + ScoreTask.Name + "(" + ScoreTask.CompleteBScore
                                      + "分)"
                                      + "|记录人：" + db.User.FirstOrDefault(t => t.ID == UserID).RealName
                                      + "|初审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                                      + "|终审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                                      + "|状态：待终审";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = ScoreTask.LastAuditUserID;
                                Notice.SendUserID = UserID;
                                Notice.SourceID = ScoreTask.ID;
                                Notice.SendDate = DateTime.Now;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/TaskAudit.html?flag=1&id=" + ScoreTask.ID;
                                db.Notice.Add(Notice);

                            }
                            else if (Convert.ToInt32(this.ddl_Audit.SelectedValue) == (int)AuditState.驳回)
                            {
                                ScoreTask.AuditState = AuditState.驳回;

                                Notice Notice = new Notice();
                                Notice.Contenet = "当前有一条任务被驳回|主题：" + ScoreTask.Name + "(" + ScoreTask.CompleteBScore
                                      + "分)"
                                      + "|记录人：" + db.User.FirstOrDefault(t => t.ID == UserID).RealName
                                      + "|初审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                                      + "|终审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                                      + "|状态：驳回";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = ScoreTask.CreateUserID;
                                Notice.SendUserID = UserID;
                                Notice.SourceID = ScoreTask.ID;
                                Notice.SendDate = DateTime.Now;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/TaskAudit.html?flag=1&id=" + ScoreTask.ID;
                                db.Notice.Add(Notice);
                            }

                            ScoreTask.FirstAuditDate = DateTime.Now;
                            ScoreTask.FirstAuditMark = this.txt_EventMark.Text.Trim();
                        }


                        if (ScoreTask.AuditState == AuditState.待终审 && ScoreTask.LastAuditUserID == UserID)
                        {

                            if (Convert.ToInt32(this.ddl_Audit.SelectedValue) == (int)AuditState.通过)
                            {
                                ScoreTask.AuditState = AuditState.通过;

                                Notice Notice = new Notice();
                                Notice.Contenet = "当前有一条任务需要通过审核|主题：" + ScoreTask.Name + "(" + ScoreTask.CompleteBScore
                                      + "分)"
                                      + "|记录人：" + db.User.FirstOrDefault(t => t.ID == UserID).RealName
                                      + "|初审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                                      + "|终审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                                      + "|状态：通过";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = ScoreTask.CreateUserID;
                                Notice.SendUserID = UserID;
                                Notice.SourceID = ScoreTask.ID;
                                Notice.SendDate = DateTime.Now;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/TaskAudit.html?flag=1&id=" + ScoreTask.ID;
                                db.Notice.Add(Notice);

                            }
                            else if (Convert.ToInt32(this.ddl_Audit.SelectedValue) == (int)AuditState.驳回)
                            {
                                ScoreTask.AuditState = AuditState.驳回;

                                Notice Notice = new Notice();
                                Notice.Contenet = "当前有一条任务被驳回|主题：" + ScoreTask.Name + "(" + ScoreTask.CompleteBScore
                                      + "分)"
                                      + "|记录人：" + db.User.FirstOrDefault(t => t.ID == UserID).RealName
                                      + "|初审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                                      + "|终审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                                      + "|状态：驳回";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = ScoreTask.CreateUserID;
                                Notice.SendUserID = UserID;
                                Notice.SourceID = ScoreTask.ID;
                                Notice.SendDate = DateTime.Now;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/TaskAudit.html?flag=1&id=" + ScoreTask.ID;
                                db.Notice.Add(Notice);
                            }

                            ScoreTask.LastAuditDate = DateTime.Now;
                            ScoreTask.LastAuditMark = this.txt_EventMark.Text.Trim();
                        }

                        db.SaveChanges();

                        new SysLogDAO().AddLog(LogType.操作日志_添加, "审核任务信息", UserID);
                        ShowMessage();
                    }
                    else
                    {
                        ShowMessage("保存失败");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                ShowMessage(ex.Message);
            }
        }
        #endregion
    }
}