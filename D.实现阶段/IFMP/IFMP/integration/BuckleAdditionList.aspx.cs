/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月18日 16时40分47秒
** 描    述:     积分奖扣列表页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Linq;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.DAO;
using IFMPLibrary.Utils;

namespace IFMP.integration
{
    public partial class BuckleAdditionList : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User user = db.User.FirstOrDefault(t => t.ID == UserID);
                if (user != null)
                {
                    this.txt_VUser.Text = user.RealName;
                    this.txt_VUser.Enabled = false;
                    CommonFunction.BindEnum<CommonEnum.AduitState>(this.ddl_AduitState, "-2");
                    //CommonFunction.BindEnum<CommonEnum.IsorNot>(this.ddl_SState, "-2");
                    this.ddl_AduitState.Items.Remove(new ListItem("确认完成", "5"));
                    GetCondition();
                    DataBindList();
                }
            }
        }
        #endregion


        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["VUser"] = CommonFunction.GetCommoneString(this.txt_VUser.Text.Trim());
            ViewState["EventName"] = CommonFunction.GetCommoneString(this.txt_EventName.Text.Trim());
            ViewState["AduitState"] = this.ddl_AduitState.SelectedValue;
            //ViewState["SState"] = this.ddl_SState.SelectedValue;
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            string username = CommonFunction.GetCommoneString(this.txt_VUser.Text.Trim());
            string eventname = CommonFunction.GetCommoneString(this.txt_EventName.Text.Trim());
            int auditstate = Convert.ToInt32(this.ddl_AduitState.SelectedValue);
            //int isdel = Convert.ToInt32(this.ddl_SState.SelectedValue);
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"].ToString()), false);

            //List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.IsDel != true && t.UserID == UserID &&
            //    db.User.Where(m => m.RealName.Contains(username)).Select(m => m.ID).Contains(t.UserID) &&
            //    db.Score.Where(k => k.AuditState == (AuditState)auditstate && k.CreateDate > begin && k.CreateDate < end &&
            //    db.ScoreEvent.Where(n => n.Name.Contains(eventname)).Select(n => n.ID).Contains(k.ScoreEventID)
            //    ).Select(k => k.ID).Contains(t.ScoreID)
            //    ).OrderBy(t => t.ID).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();

            var list = from scoreuser in db.ScoreUser
                       //.Where(t => t.IsDel != true)
                       join score in db.Score.Where(t => t.IsDel != true) on scoreuser.ScoreID equals score.ID
                       join user in db.User.Where(t => t.IsDel != true) on scoreuser.UserID equals user.ID
                       join recorduser in db.User.Where(t => t.IsDel != true) on score.CreateUserID equals recorduser.ID
                       join scoreevent in db.ScoreEvent.Where(t => t.IsDel != true) on score.ScoreEventID equals scoreevent.ID
                       where scoreuser.UserID == UserID
                       && user.RealName.Contains(username)
                       && (auditstate == -2 || score.AuditState == (AuditState)auditstate)
                       && score.CreateDate >= begin
                       && score.CreateDate <= end
                       && scoreevent.Name.Contains(eventname)
                       orderby score.CreateDate descending
                       select new
                              {
                                  scoreuser.ID,
                                  user.RealName,
                                  score.CreateDate,
                                  score.Title,
                                  EventName = scoreevent.Name,
                                  scoreuser.BScore,
                                  RecordUName = recorduser.RealName,
                                  score.AuditState,
                                  scoreuser.IsDel,
                                  score.Content
                              };

            int total = list.Count();

            if (total > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            rp_List.DataSource = list.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            Pager.RecordCount = total;
            rp_List.DataBind();
            this.hf_CheckIDS.Value = "";
        }
        #endregion


        #region 查询
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 删除
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            string ids = hf_CheckIDS.Value.ToString();
            try
            {
                ids = ids.TrimEnd(',').TrimStart(',');
                foreach (string id in ids.Split(','))
                {

                    int iddata = Convert.ToInt32(id);
                    ScoreUser model = db.ScoreUser.FirstOrDefault(t => t.ID == iddata);
                    if (model != null)
                    {
                        model.IsDel = true;
                    }
                    else
                    {
                        ShowMessage("删除失败");
                        return;
                    }
                }
                db.SaveChanges();
                new SysLogDAO().AddLog(LogType.操作日志_删除, "删除积分奖扣信息");
                ShowMessage("删除成功");
                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message.ToString());
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message);
            }
        }
        #endregion


        #region 分页
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion
    }
}