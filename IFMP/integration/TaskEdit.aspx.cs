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
    public partial class TaskEdit : PageBase
    {
        #region 参数集合
        public int ScoreTaskID
        {
            get
            {
                return GetQueryString<int>("id", -2);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<ScoreAuditUser> ScoreAuditUserList = db.ScoreAuditUser.ToList();
                    var list = from scoreaudituser in db.ScoreAuditUser
                               join user in db.User.Where(t => t.IsDel != true) on scoreaudituser.UserID equals user.ID
                               orderby user.ID
                               select new
                                      {
                                          //scoreaudituser.ID,
                                          user.RealName,
                                          scoreaudituser.ScoreAuditUserType,
                                          scoreaudituser.UserID
                                      };


                    this.ddl_FirstAduitUser.DataSource = list.Where(t => t.ScoreAuditUserType == ScoreAuditUserType.初审人).ToList();
                    this.ddl_FirstAduitUser.DataValueField = "UserID";
                    this.ddl_FirstAduitUser.DataTextField = "RealName";
                    this.ddl_FirstAduitUser.DataBind();
                    this.ddl_FirstAduitUser.Items.Add(new ListItem("--请选择--", "-1"));
                    this.ddl_FirstAduitUser.SelectedValue = "-1";

                    this.ddl_LastAduitUser.DataSource = list.Where(t => t.ScoreAuditUserType == ScoreAuditUserType.终审人).ToList();
                    this.ddl_LastAduitUser.DataValueField = "UserID";
                    this.ddl_LastAduitUser.DataTextField = "RealName";
                    this.ddl_LastAduitUser.DataBind();
                    this.ddl_LastAduitUser.Items.Add(new ListItem("--请选择--", "-1"));
                    this.ddl_LastAduitUser.SelectedValue = "-1";


                    //默认的人
                    Department department = db.Department.FirstOrDefault(t => db.DepartmentUser.Where(m => m.UserID == UserID).Select(m => m.UserID).Contains(t.ID));
                    if (department != null && db.ScoreAuditUser.FirstOrDefault(t => t.UserID == department.MasterUserID && t.ScoreAuditUserType == ScoreAuditUserType.初审人) != null)
                    {
                        this.ddl_FirstAduitUser.SelectedValue = department.MasterUserID.ToString();
                    }
                }

                if (ScoreTaskID != -2)
                {
                    BindInfo();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        public void BindInfo()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                ScoreTask ScoreTask = db.ScoreTask.FirstOrDefault(t => t.IsDel != true && t.ID == ScoreTaskID);
                if (ScoreTask != null)
                {
                    this.txt_SignScore.Text = ScoreTask.SignBScore.ToString();
                    this.txt_TScore.Text = ScoreTask.CompleteBScore.ToString();
                    this.txt_TaskName.Text = ScoreTask.Name;
                    this.txt_TaskContent.Text = ScoreTask.Content;
                    this.txt_EndDate.Text = ScoreTask.EndDate.ToString("yyyy-MM-dd");
                    this.ddl_FirstAduitUser.SelectedValue = ScoreTask.FirstAuditUserID.ToString();
                    this.ddl_LastAduitUser.SelectedValue = ScoreTask.LastAuditUserID.ToString();
                }
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
                    if (ScoreTask == null)
                    {
                        ScoreTask = new ScoreTask();
                        ScoreTask.AuditState = AuditState.待初审;
                        ScoreTask.CompleteBScore = Convert.ToInt32(this.txt_TScore.Text.Trim());
                        ScoreTask.Content = this.txt_TaskContent.Text.Trim();
                        ScoreTask.CreateDate = DateTime.Now;
                        ScoreTask.CreateUserID = UserID;
                        ScoreTask.EndDate = Convert.ToDateTime(this.txt_EndDate.Text);
                        ScoreTask.FirstAuditUserID = Convert.ToInt32(this.ddl_FirstAduitUser.SelectedValue);
                        ScoreTask.LastAuditUserID = Convert.ToInt32(this.ddl_LastAduitUser.SelectedValue);
                        ScoreTask.IsDel = false;
                        ScoreTask.Name = this.txt_TaskName.Text.Trim();
                        ScoreTask.SignBScore = Convert.ToInt32(this.txt_SignScore.Text.Trim());

                        db.ScoreTask.Add(ScoreTask);
                        db.SaveChanges();

                        //发通知
                        Notice Notice = new Notice();
                        Notice.Contenet = "当前有一条任务需要审核|主题：" + ScoreTask.Name + "(" + ScoreTask.CompleteBScore
                              + "分)"
                              + "|记录人：" + db.User.FirstOrDefault(t => t.ID == UserID).RealName
                              + "|初审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                              + "|终审人：" + db.User.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                              + "|状态：待初审";
                        Notice.IsSend = false;
                        Notice.NoticeType = NoticeType.积分制消息;
                        Notice.ReciveUserID = ScoreTask.FirstAuditUserID;
                        Notice.SendUserID = UserID;
                        Notice.SourceID = ScoreTask.ID;
                        Notice.SendDate = DateTime.Now;
                        Notice.URL = ParaUtils.SiteURL + "/jfz/app/TaskAudit.html?flag=1&id=" + ScoreTask.ID;
                        db.Notice.Add(Notice);
                        db.SaveChanges();
                    }

                    new SysLogDAO().AddLog(LogType.操作日志_添加, "发布积分任务", UserID);
                    ShowMessage();
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