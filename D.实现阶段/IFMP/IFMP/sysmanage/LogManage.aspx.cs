/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创建人:      樊紫红
** 创建日期:    2018年7月11日 15时33分
** 描 述:       日志管理页面
** 修改人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using GK.IFMP.Common;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.DAO;
using IFMPLibrary.Utils;


namespace IFMP.sysmanage
{
    public partial class LogManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<LogType>(this.ddl_LogType, "-2");
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["CreateUser"] = CommonFunction.GetCommoneString(this.txt_CreateUser.Text.Trim());//姓名
            ViewState["Begin"] = this.txt_BeginDate.Text == "" ? "1900-01-01" : this.txt_BeginDate.Text.ToString().Trim();
            ViewState["End"] = this.txt_EndDate.Text == "" ? "9999-12-31" : this.txt_EndDate.Text.ToString().Trim();
            ViewState["LogType"] = this.ddl_LogType.SelectedValue;
        }
        #endregion


        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void DataBindList()
        {
            string createuser = ViewState["CreateUser"].ToString();
            int logtype = Convert.ToInt32(this.ddl_LogType.SelectedValue.ToString());
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["Begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["End"].ToString()), false);
            var list = from sysloglist in db.SysLog.Where(t => (logtype == -2 || t.LogType == (LogType)logtype) && t.CreateDate > begin && t.CreateDate < end)
                       join sysuser in db.User.Where(t => t.IsDel != true) on sysloglist.InvolvedUser equals sysuser.ID
                       where sysuser.RealName.Contains(createuser)
                       orderby sysloglist.CreateDate descending
                       select new
                       {
                           sysloglist.LogType,
                           sysloglist.Message,
                           sysloglist.CreateDate,
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
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion
    }
}