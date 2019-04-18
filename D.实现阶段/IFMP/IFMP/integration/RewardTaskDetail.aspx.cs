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
    public partial class RewardTaskDetail : PageBase
    {

        #region 参数集合
        public int ScoreTaskID
        {
            get
            {
                return GetQueryString<int>("id", 0);
            }
        }

        //1 抢单 2 完成
        public int Flag
        {
            get
            {
                return GetQueryString<int>("flag", 0);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ScoreTaskID != 0)
                {
                    if (Flag == 0)
                    {
                        this.div.Visible = false;
                    }
                    else
                    {
                        this.div.Visible = true;
                    }
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
                ScoreTask ScoreTask = db.ScoreTask.FirstOrDefault(t => t.ID == ScoreTaskID && t.IsDel != true);
                if (ScoreTask != null)
                {
                    this.ltl_EndDate.Text = ScoreTask.EndDate.ToString("yyyy-MM-dd");
                    this.ltl_FirstAduitDate.Text = ScoreTask.FirstAuditDate == null ? "" : ScoreTask.FirstAuditDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_FirstAduitMess.Text = ScoreTask.FirstAuditMark;
                    this.ltl_FirstAduitUser.Text = db.User.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName;
                    this.ltl_LastAduitDate.Text = ScoreTask.LastAuditDate == null ? "" : ScoreTask.LastAuditDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_LastAduitMess.Text = ScoreTask.LastAuditMark;
                    this.ltl_LastAduitUser.Text = db.User.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName;
                    this.ltl_SignScore.Text = ScoreTask.SignBScore.ToString();
                    this.ltl_TaskContent.Text = ScoreTask.Content;
                    this.ltl_TaskName.Text = ScoreTask.Name;
                    this.ltl_TaskUser.Text = db.User.FirstOrDefault(t => t.ID == ScoreTask.CreateUserID).RealName;
                    this.ltl_TScore.Text = ScoreTask.CompleteBScore.ToString();
                    this.ltl_TState.Text = Enum.GetName(typeof(AuditState), ScoreTask.AuditState);
                    //List<ScoreTaskUser> ScoreTaskUserList = db.ScoreTaskUser.Where(t => t.ScoreTaskID == ScoreTask.ID).ToList();
                    var list = from scoretaskuser in db.ScoreTaskUser.Where(t => t.ScoreTaskID == ScoreTaskID)
                               join user in db.User.Where(t => t.IsDel != true) on scoretaskuser.UserID equals user.ID
                               orderby scoretaskuser.ID descending
                               select new
                               {
                                   UserName = user.RealName,
                                   scoretaskuser.CompleteDate,
                               };

                    if (list.Count() > 0)
                    {
                        this.rp_List.DataSource = list.ToList();
                        this.rp_List.DataBind();
                    }
                    else
                    {
                        this.tr.Visible = false;
                    }
                }
            }
        }
        #endregion

        #region 抢单or完成
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    ScoreTask ScoreTask = db.ScoreTask.FirstOrDefault(t => t.ID == ScoreTaskID && t.IsDel != true);
                    if (ScoreTask != null)
                    {
                        if (ScoreTask.EndDate > DateTime.Now)
                        {
                            string message = "";
                            if (Flag == 1)
                            {
                                //抢单
                                ScoreTaskUser ScoreTaskUser = db.ScoreTaskUser.FirstOrDefault(t => t.ScoreTaskID == ScoreTask.ID && t.UserID == UserID);
                                if (ScoreTaskUser == null)
                                {
                                    ScoreTaskUser = new ScoreTaskUser();
                                    ScoreTaskUser.UserID = UserID;
                                    ScoreTaskUser.ScoreTaskID = ScoreTask.ID;
                                    db.ScoreTaskUser.Add(ScoreTaskUser);
                                    message = "抢单成功";
                                }
                                else
                                {
                                    ShowMessage("您已报名");
                                    return;
                                }
                            }
                            else if (Flag == 2)
                            {
                                //完成
                                ScoreTaskUser ScoreTaskUser = db.ScoreTaskUser.FirstOrDefault(t => t.ScoreTaskID == ScoreTask.ID && t.UserID == UserID);
                                if (ScoreTaskUser != null)
                                {
                                    ScoreTaskUser.CompleteDate = DateTime.Now;
                                    //db.ScoreTaskUser.Add(ScoreTaskUser);
                                    message = "任务完成成功";
                                }
                                else
                                {
                                    ShowMessage("未查询到您的报名信息，请先抢单");
                                    return;
                                }
                            }
                            db.SaveChanges();
                            new SysLogDAO().AddLog(LogType.操作日志_添加, message, UserID);
                            ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('" + message + "');window.parent.location.href='RewardTaskList.aspx'</script>");
                        }
                        else
                        {
                            ShowMessage("任务已过期");
                            return;
                        }

                    }
                    else
                    {
                        ShowMessage("找不到任务");
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