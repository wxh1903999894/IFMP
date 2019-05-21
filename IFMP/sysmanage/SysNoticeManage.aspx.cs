/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创建人:      樊紫红
** 创建日期:    2018年7月24日 8时23分
** 描 述:       通知消息管理页面
** 修改人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.Utils;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.DAO;

namespace IFMP.sysmanage
{
    public partial class SysNoticeManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();


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
        private void GetCondition()
        {
            ViewState["Begin"] = this.txt_BeginDate.Text == "" ? "1900-01-01" :this.txt_BeginDate.Text.ToString().Trim();
            ViewState["End"] = this.txt_EndDate.Text == "" ? "9999-12-31" : this.txt_EndDate.Text.ToString().Trim();
        }
        #endregion


        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void DataBindList()
        {
            //DateTime begin = Convert.ToDateTime(ViewState["Begin"].ToString());
            //DateTime end = Convert.ToDateTime(ViewState["End"].ToString());
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["Begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["End"].ToString()), false);
            var list = from sysnoticelist in db.Notice.Where(t => t.SendDate >= begin && t.SendDate <= end)
                       join sysuser in db.User.Where(t => t.IsDel != true) on sysnoticelist.SendUserID equals sysuser.ID
                       where sysnoticelist.ReciveUserID == UserID
                       //&& sysnoticelist.IsSend == true
                       //sysuser.RealName.Contains(createuser)
                       orderby sysnoticelist.SendDate descending
                       select new
                       {
                           sysnoticelist.ID,
                           sysnoticelist.NoticeType,
                           sysnoticelist.SendDate,
                           sysnoticelist.Contenet,
                           sysuser.RealName
                       };

            int total = list.Count();

            if (list != null && total > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = list.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            Pager.RecordCount = total;
            rp_List.DataBind();
        }
        #endregion


        #region 分页事件
        /// <summary>
        /// 分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 查询事件
        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion
    }
}