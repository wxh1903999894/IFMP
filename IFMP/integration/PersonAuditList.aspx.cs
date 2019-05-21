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
    public partial class PersonAuditList : PageBase
    {
        #region 页面初始化

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<AuditState>(this.ddl_AduitState, "-2");
                this.ddl_AduitState.Items.Remove(new ListItem("通过", "3"));
                GetCondition();
                DataBindList();
            }
        }
        #endregion

        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["EventName"] = CommonFunction.GetCommoneString(this.txt_EventName.Text.Trim());
            ViewState["AduitState"] = this.ddl_AduitState.SelectedValue;
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            string eventname = Convert.ToString(ViewState["EventName"]);
            //AuditState auditstate = (AuditState)(Convert.ToInt32(ViewState["AduitState"]));
            //DateTime begin = Convert.ToDateTime(ViewState["begin"]);
            //DateTime end = Convert.ToDateTime(ViewState["end"]);
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"].ToString()), false);
            int auditstate = Convert.ToInt32(ViewState["AduitState"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                var list = from score in db.Score.Where(t => t.IsDel != true
                    && ((t.FirstAuditUserID == UserID && (t.AuditState == AuditState.待初审 || t.AuditState == AuditState.通过 || t.AuditState == AuditState.驳回))||(t.LastAuditUserID == UserID &&(t.AuditState == AuditState.待终审 || t.AuditState == AuditState.通过 || t.AuditState == AuditState.驳回))))
                           join scoreevent in db.ScoreEvent.Where(t => t.IsDel != true) on score.ScoreEventID equals scoreevent.ID
                           join recorduser in db.User.Where(t => t.IsDel != true) on score.CreateUserID equals recorduser.ID
                           where scoreevent.Name.Contains(eventname)
                           && (auditstate == -2 || score.AuditState == (AuditState)auditstate)
                           && score.CreateDate >= begin
                           && score.CreateDate <= end
                           orderby score.CreateDate descending
                           select new
                                  {
                                      score.ID,
                                      score.CreateDate,
                                      score.Title,
                                      EventName = scoreevent.Name,
                                      RecordUserName = recorduser.RealName,
                                      score.AuditState,
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
            try
            {
                string ids = this.hf_CheckIDS.Value.TrimEnd(',');

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (string id in ids.Split(','))
                    {
                        int selid = Convert.ToInt32(id);
                        Score Score = db.Score.FirstOrDefault(t => t.ID == selid);
                        Score.IsDel = true;
                    }
                    db.SaveChanges();
                    new SysLogDAO().AddLog(LogType.操作日志_删除, "删除积分信息");
                    ShowMessage("删除成功");
                    DataBindList();
                    this.hf_CheckIDS.Value = "";
                }
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                ShowMessage(ex.Message);
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