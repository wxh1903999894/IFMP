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
    public partial class TaskAuditList : PageBase
    {
        #region 页面初始化

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<AuditState>(this.ddl_AduitState, "-2");
                GetCondition();
                DataBindList();
            }
        }
        #endregion

        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["TaskName"] = CommonFunction.GetCommoneString(this.txt_TaskName.Text.Trim());
            ViewState["TaskUser"] = CommonFunction.GetCommoneString(this.txt_TaskUser.Text.Trim());
            ViewState["TState"] = this.ddl_AduitState.SelectedValue;
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            string name = ViewState["TaskName"].ToString();
            string taskusername = ViewState["TaskUser"].ToString();
            int auditstate = Convert.ToInt32(ViewState["TState"]);
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"].ToString()), false);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                //加个责任人
                var list = from scoretask in db.ScoreTask
                           join user in db.User.Where(t => t.IsDel != true) on scoretask.CreateUserID equals user.ID
                           where (auditstate == -2 || scoretask.AuditState == (AuditState)auditstate)
                           && scoretask.EndDate >= begin && scoretask.EndDate <= end && scoretask.Name.Contains(name)
                           && (db.User.Where(m => m.IsDel != true && m.RealName.Contains(taskusername)).Select(m => m.ID).Contains(scoretask.CreateUserID))
                           && scoretask.IsDel != true
                           && ((scoretask.FirstAuditUserID == UserID
                           && scoretask.AuditState == AuditState.待初审) || (scoretask.LastAuditUserID == UserID && scoretask.AuditState == AuditState.待终审))
                           orderby scoretask.CreateDate descending
                           select new
                                  {
                                      scoretask.Name,
                                      scoretask.CreateDate,
                                      scoretask.ID,
                                      scoretask.CompleteBScore,
                                      scoretask.SignBScore,
                                      scoretask.EndDate,
                                      scoretask.AuditState,
                                      UserName = user.RealName,
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
                rp_List.DataSource = list.OrderByDescending(t => t.CreateDate).Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList();
                Pager.RecordCount = total;
                rp_List.DataBind();
            }
        }
        #endregion


        #region 查询
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 分页
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            DataBindList();
        }
        #endregion
    }
}