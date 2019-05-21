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
    public partial class RewardTaskList : PageBase
    {


        #region 页面初始化

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            string name = ViewState["TaskName"].ToString();
            string taskusername = ViewState["TaskUser"].ToString();
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"].ToString()), false);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                var list = from scoretask in db.ScoreTask
                           join user in db.User.Where(t => t.IsDel != true) on scoretask.CreateUserID equals user.ID
                           where scoretask.AuditState == AuditState.通过
                           && scoretask.EndDate >= begin && scoretask.EndDate <= end
                           && scoretask.Name.Contains(name)
                           && scoretask.IsDel != true
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
                               IsSelf = user.ID == UserID
                           };
                int total = list.Count();
                //List<ScoreTask> ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && t.AuditState == AuditState.通过 && t.CreateDate > begin && t.CreateDate < end && t.Name.Contains(name)).ToList();
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
                this.hf_CheckIDS.Value = "";
            }
        }
        #endregion


        #region 查询
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 分页
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 确认完成
        protected void btn_Compelete_Click(object sender, EventArgs e)
        {
            try
            {
                string ids = this.hf_CheckIDS.Value.TrimEnd(',');

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (string id in ids.Split(','))
                    {
                        int selid = Convert.ToInt32(id);
                        ScoreTask ScoreTask = db.ScoreTask.FirstOrDefault(t => t.IsDel != true && t.CreateUserID == UserID && t.AuditState == AuditState.通过);
                        if (ScoreTask != null)
                        {
                            ScoreTask.AuditState = AuditState.确认完成;
                        }
                    }
                    db.SaveChanges();

                    new SysLogDAO().AddLog(LogType.操作日志_修改, "确认完成成功", UserID);
                    ShowMessage("确认成功");
                    DataBindList();
                    this.hf_CheckIDS.Value = "";
                }
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                ShowMessage(ex.Message);
                return;
            }
        }
        #endregion
    }
}