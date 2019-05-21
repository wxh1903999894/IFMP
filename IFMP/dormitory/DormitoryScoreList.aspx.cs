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
using System.Text;

namespace IFMP.dormitory
{
    public partial class DormitoryScoreList : PageBase
    {
        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ddl_SState.Items.Add(new ListItem("所有", "-1"));
                this.ddl_SState.Items.Add(new ListItem("否", "0"));
                this.ddl_SState.Items.Add(new ListItem("是", "1"));


                GetCondition();
                DataBindList();
            }
        }
        #endregion

        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["SState"] = this.ddl_SState.SelectedValue;
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion

        #region 数据绑定
        public void DataBindList()
        {
            string dorname = CommonFunction.GetCommoneString(this.txt_DorName.Text.Trim());
            int isdel = Convert.ToInt32(this.ddl_SState.SelectedValue);
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"].ToString()), false);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var list = from spotroblem in db.SpotProblem
                           join createuser in db.User.Where(t => t.IsDel != true) on spotroblem.CreateUser equals createuser.ID.ToString()
                           join dutyuser in db.User.Where(t => t.IsDel != true) on spotroblem.DutyUser equals dutyuser.ID.ToString()
                           //join reviewuser in db.User.Where(t => t.IsDel != true) on spotroblem.ReviewUser equals reviewuser.ID
                           join spotcheck in db.SpotCheck on spotroblem.SpotId equals spotcheck.SpotId
                           join dormitory in db.Dormitory on spotcheck.DormitoryId equals dormitory.ID
                           where spotroblem.CreateDate >= begin
                           && spotroblem.CreateDate <= end
                            && dormitory.DormiName.Contains(dorname)
                            && (isdel == -1 || spotroblem.IsreView == (isdel == 0 ? false : true))
                           orderby spotroblem.CreateDate descending
                           select new
                           {
                               CreateUser = createuser.RealName,
                               DutyUser = dutyuser.RealName,
                               ReviewUser = spotroblem.ReviewUser == null ? "" : db.User.FirstOrDefault(x => x.ID.ToString() == spotroblem.ReviewUser).RealName,
                               spotroblem.ProDesc,
                               DormiName = dormitory.DormiName,
                               spotroblem.CreateDate,
                               spotroblem.ReviewDate,
                               spotroblem.ReviewMemo,
                               spotroblem.IsreView,
                               ID=spotroblem.SpId
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
                rp_List.DataSource = list.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList(); ;
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

        #region 导出
        protected void btn_OutPut_Click(object sender, EventArgs e)
        {
            try
            {
                string dorname = CommonFunction.GetCommoneString(this.txt_DorName.Text.Trim());
                int isdel = Convert.ToInt32(this.ddl_SState.SelectedValue);
                DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"].ToString()));
                DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"].ToString()), false);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    var list = from spotroblem in db.SpotProblem
                               join createuser in db.User.Where(t => t.IsDel != true) on spotroblem.CreateUser equals createuser.ID.ToString()
                               join dutyuser in db.User.Where(t => t.IsDel != true) on spotroblem.DutyUser equals dutyuser.ID.ToString()
                               //join reviewuser in db.User.Where(t => t.IsDel != true) on spotroblem.ReviewUser equals reviewuser.ID
                               join spotcheck in db.SpotCheck on spotroblem.SpotId equals spotcheck.SpotId
                               join dormitory in db.Dormitory on spotcheck.DormitoryId equals dormitory.ID
                               where spotroblem.CreateDate >= begin
                               && spotroblem.CreateDate <= end
                                && dormitory.DormiName.Contains(dorname)
                                && (isdel == -1 || spotroblem.IsreView == (isdel == 0 ? false : true))
                               orderby spotroblem.CreateDate descending
                               select new
                               {
                                   CreateUser = createuser.RealName,
                                   DutyUser = dutyuser.RealName,
                                   ReviewUser = spotroblem.ReviewUser == null ? "" : db.User.FirstOrDefault(x => x.ID.ToString() == spotroblem.ReviewUser).RealName,
                                   spotroblem.ProDesc,
                                   DormiName = dormitory.DormiName,
                                   spotroblem.CreateDate,
                                   spotroblem.ReviewDate,
                                   spotroblem.ReviewMemo,
                                   spotroblem.IsreView,
                                   ID = spotroblem.SpId
                               };
                    if (list.Count() > 0)
                    {
                        StringBuilder str = new StringBuilder("");
                        str.Append("<table border=\"1\" cellpadding=\"0\" cellspaccing=\"0\"><tr><th>宿舍名称</th><th>点检日期</th><th>问题描述</th><th>问题责任人</th><th>点检人</th></tr>");
                        foreach (var data in list.ToList())
                        {
                            str.Append("<tr>");
                            str.AppendFormat("<td>{0}</td>", data.DormiName.ToString());
                            str.AppendFormat("<td>{0}</td>", data.CreateDate.Value.ToString("yyyy-MM-dd"));
                            str.AppendFormat("<td>{0}</td>", data.ProDesc.ToString());
                            str.AppendFormat("<td>{0}</td>", data.DutyUser.ToString());
                            str.AppendFormat("<td>{0}</td>", data.CreateUser.ToString());
                            str.Append("</tr>");
                        }
                        str.Append("</table>");
                        new SysLogDAO().AddLog(LogType.操作日志_导出, "导出宿舍排名信息", UserID);
                        new BaseUtils().ExportExcel("宿舍排名信息", str.ToString());
                    }
                    else
                    {
                        ShowMessage("暂无数据导出");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message.ToString());
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message);
            }
        }
        #endregion

    }
}