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
    public partial class TaskList : PageBase
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
            ViewState["TState"] = this.ddl_AduitState.SelectedValue;
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                string taskname = ViewState["TaskName"].ToString();
                int tstate = Convert.ToInt32(ViewState["TState"]);
                DateTime begindate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"]));
                DateTime enddate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"]), false);

                List<ScoreTask> ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && t.CreateUserID == UserID
                    && t.Name.Contains(taskname)
                    && t.EndDate >= begindate
                    && t.EndDate <= enddate
                    && (tstate == -2 || t.AuditState == (AuditState)tstate)
                    ).OrderByDescending(t => t.CreateDate).Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList();

                if (ScoreTaskList.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                rp_List.DataSource = ScoreTaskList;
                Pager.RecordCount = db.ScoreTask.Count(t => t.IsDel != true && t.CreateUserID == UserID);
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


        #region 删除
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                string ids = this.hf_CheckIDS.Value.TrimEnd(',');
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    try
                    {
                        foreach (string id in ids.Split(','))
                        {
                            int selid = Convert.ToInt32(id);
                            ScoreTask ScoreTask = db.ScoreTask.FirstOrDefault(t => t.ID == selid);
                            ScoreTask.IsDel = true;
                        }
                        db.SaveChanges();

                        new SysLogDAO().AddLog(LogType.操作日志_删除, "删除任务信息", UserID);
                        ShowMessage("删除成功");
                    }
                    catch
                    {
                        ShowMessage("删除失败");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                ShowMessage(ex.Message);
            }
            this.hf_CheckIDS.Value = "";
            DataBindList();
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