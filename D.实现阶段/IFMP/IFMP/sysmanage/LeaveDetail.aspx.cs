/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月8日 9时33分19秒
** 描    述:      请假详情
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

namespace IFMP.sysmanage
{
    public partial class LeaveDetail : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int LID
        {
            get
            {
                return GetQueryString<int>("id", -1); ;
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InfoBind();
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            Leave leave = db.Leave.FirstOrDefault(t => t.ID == LID);
            if (leave != null)
            {
                this.ltl_UserID.Text = db.User.FirstOrDefault(t => t.ID == leave.UserID).RealName;
                this.ltl_Day.Text = leave.Day.ToString();
                this.ltl_BeginDate.Text = leave.BeginDate.ToString("yyyy-MM-dd");
                this.ltl_EndDate.Text = leave.EndDate.ToString("yyyy-MM-dd");
                this.ltl_LeaveType.Text = leave.LeaveType.ToString();
                this.ltl_CreateDate.Text = leave.CreateDate.ToString();
                this.ltl_Content.Text = leave.Content.ToString();
            }


            var list = from auditlist in db.LeaveAudit
                       join user in db.User on auditlist.UserID equals user.ID
                       //join role in db.Role on auditlist.RoleID equals role.ID
                       //from role in db.Role
                       where auditlist.LeaveID == LID
                       select new
                       {
                           //auditlist.RoleEnums,
                           auditlist.SendDate,
                           auditlist.AuditDate,
                           auditlist.LeaveState,
                           user.RealName,
                           RoleName = auditlist.RoleID == null ? "部门领导" : db.Role.FirstOrDefault(t => t.ID == auditlist.RoleID).Name
                       };


            if (list.Count() > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = list.ToList();
            this.rp_List.DataBind();
        }
        #endregion
    }
}