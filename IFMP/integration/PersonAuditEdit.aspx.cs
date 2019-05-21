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
    public partial class PersonAuditEdit : PageBase
    {
        #region 参数集合
        public int ScoreID
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
                    Score Score = db.Score.FirstOrDefault(t => t.ID == ScoreID && t.IsDel != true);
                    AuditState auditstate = (AuditState)Convert.ToInt32(this.ddl_Audit.SelectedValue);
                    Notice Notice = new Notice();
                    if (Score != null)
                    {
                        if (Score.AuditState == AuditState.待初审 && Score.FirstAuditUserID == UserID && db.ScoreAuditUser.FirstOrDefault(t => t.UserID == UserID && t.ScoreAuditUserType == ScoreAuditUserType.初审人) != null)
                        {
                            if (auditstate == AuditState.通过)
                            {
                                Score.AuditState = AuditState.待终审;

                                Notice.Contenet = "当前有一条奖扣记录需要审核|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                               + "分)"
                               + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                               + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                               + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                               + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                               + "|状态：待终审";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = Score.LastAuditUserID.Value;
                                Notice.SendUserID = UserID;
                                Notice.SendDate = DateTime.Now;
                                Notice.SourceID = Score.ID;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=1&id=" + Score.ID;
                                db.Notice.Add(Notice);

                                //给每个人发一个
                                foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                                {
                                    Notice = new Notice();
                                    Notice.Contenet = "您有一条奖扣记录正在审核中|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                      + "分)"
                                      + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                      + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                      + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                      + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                      + "|状态：待终审";
                                    Notice.IsSend = false;
                                    Notice.NoticeType = NoticeType.积分制消息;
                                    Notice.ReciveUserID = ScoreUser.UserID;
                                    Notice.SendUserID = UserID;
                                    Notice.SourceID = Score.ID;
                                    Notice.SendDate = DateTime.Now;
                                    Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                                    db.Notice.Add(Notice);
                                }

                            }
                            else
                            {
                                Score.AuditState = AuditState.驳回;

                                foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                                {
                                    Notice = new Notice();
                                    Notice.Contenet = "您有一条奖扣记录被驳回|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                      + "分)"
                                      + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                      + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                      + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                      + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                      + "|状态：驳回";
                                    Notice.IsSend = false;
                                    Notice.NoticeType = NoticeType.积分制消息;
                                    Notice.ReciveUserID = ScoreUser.UserID;
                                    Notice.SendUserID = UserID;
                                    Notice.SendDate = DateTime.Now;
                                    Notice.SourceID = Score.ID;
                                    Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                                    db.Notice.Add(Notice);
                                }
                            }
                            Score.FirstAuditDate = DateTime.Now;
                            Score.FirstAuditMark = this.txt_EventMark.Text.Trim();
                        }
                        if (Score.AuditState == AuditState.待终审 && Score.LastAuditUserID == UserID && db.ScoreAuditUser.FirstOrDefault(t => t.UserID == UserID && t.ScoreAuditUserType == ScoreAuditUserType.终审人) != null)
                        {
                            if (auditstate == AuditState.通过)
                            {
                                Score.AuditState = AuditState.通过;

                                foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                                {
                                    Notice = new Notice();
                                    Notice.Contenet = "您有一条奖扣记录已审核通过|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                      + "分)"
                                      + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                      + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                      + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                      + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                      + "|状态：通过";
                                    Notice.IsSend = false;
                                    Notice.NoticeType = NoticeType.积分制消息;
                                    Notice.ReciveUserID = ScoreUser.UserID;
                                    Notice.SendUserID = UserID;
                                    Notice.SourceID = Score.ID;
                                    Notice.SendDate = DateTime.Now;
                                    Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                                    db.Notice.Add(Notice);
                                }
                            }
                            else
                            {
                                Score.AuditState = AuditState.驳回;

                                foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                                {
                                    Notice = new Notice();
                                    Notice.Contenet = "您有一条奖扣记录被驳回|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                      + "分)"
                                      + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                      + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                      + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                      + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                      + "|状态：驳回";
                                    Notice.IsSend = false;
                                    Notice.NoticeType = NoticeType.积分制消息;
                                    Notice.ReciveUserID = ScoreUser.UserID;
                                    Notice.SendUserID = UserID;
                                    Notice.SendDate = DateTime.Now;
                                    Notice.SourceID = Score.ID;
                                    Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                                    db.Notice.Add(Notice);
                                }
                            }

                            Score.LastAuditDate = DateTime.Now;
                            Score.LastAuditMark = this.txt_EventMark.Text.Trim();
                        }

                        db.SaveChanges();
                        ShowMessage();
                        new SysLogDAO().AddLog(LogType.操作日志_修改, "审核奖扣积分信息", UserID);
                    }
                    else
                    {
                        ShowMessage("找不到积分奖扣信息");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}