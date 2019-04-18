/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 8时49分19秒
** 描    述:      用户信息管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
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
using System.Data;


namespace IFMP.sysmanage
{
    public partial class LeaveStatistics : PageBase
    {

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<object> LeaveTypeList = new List<object>();

                foreach (LeaveType item in Enum.GetValues(typeof(LeaveType)))
                {
                    LeaveTypeList.Add(new
                    {
                        Name = Enum.GetName(typeof(LeaveType), item)
                    });
                }
                this.rp_LeaveType.DataSource = LeaveTypeList;
                this.rp_LeaveType.DataBind();
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["Name"] = this.txt_Name.Text;
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            DateTime begindate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"]));
            DateTime enddate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"]), false);
            string Name = ViewState["Name"].ToString();


            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> UserList = db.User.Where(t => t.IsDel != true
                    && t.UserState != UserState.离职
                    && t.RealName.Contains(Name)
                    && db.Leave.Where(m => m.IsDel != true
                    && m.LeaveState == LeaveState.通过
                    && m.BeginDate >= begindate && m.EndDate <= enddate).Select(m => m.UserID).Contains(t.ID)).ToList();

                int total = UserList.Count;
                UserList = UserList.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();

                List<object> returnList = new List<object>();

                List<Leave> LeaveList = db.Leave.Where(m => m.IsDel != true
                    && m.LeaveState == LeaveState.通过
                    && m.BeginDate >= begindate && m.EndDate <= enddate).ToList();

                foreach (User User in UserList)
                {
                    returnList.Add(new
                    {
                        User.ID,
                        User.RealName,
                    });
                }

                if (total > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }

                this.rp_List.DataSource = returnList;
                Pager.RecordCount = total;
                this.rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }
        }
        #endregion


        protected void rp_Count_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DateTime begindate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"]));
            DateTime enddate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"]), false);
            string Name = ViewState["Name"].ToString();

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("rp_Count") as Repeater;//找到里层的repeater对象
                HiddenField hfid = (HiddenField)e.Item.FindControl("hf_ID");
                int userid = Convert.ToInt32(hfid.Value);
                List<object> returnList = new List<object>();
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Leave> LeaveList = db.Leave.Where(t => t.UserID == userid
                            && t.IsDel != true
                    && t.LeaveState == LeaveState.通过
                    && t.BeginDate >= begindate && t.EndDate <= enddate).ToList();

                    List<object> LeaveTypeList = new List<object>();
                    foreach (LeaveType item in Enum.GetValues(typeof(LeaveType)))
                    {
                        double count = LeaveList.Count(t => t.LeaveType == item) > 0 ? LeaveList.Where(t => t.LeaveType == item).Sum(t => t.Day) : 0.0;

                        returnList.Add(new
                        {
                            Count = count
                        });
                    }
                }
                rep.DataSource = returnList;
                rep.DataBind();
            }
        }

        #region 分页事件
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 查询事件
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion

    }
}