/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创建人:      樊紫红
** 创建日期:    2018年7月24日 11时14分
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
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.DAO;

namespace IFMP.sysmanage
{
    public partial class SysNoticeDetail : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int NID
        {
            get
            {
                return GetQueryString<int>("id", -1);
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
            Notice model = db.Notice.FirstOrDefault(t => t.ID == NID);
            if (model != null)
            {
                this.ltl_SendUser.Text = db.User.FirstOrDefault(t => t.ID == model.SendUserID).RealName;
                this.ltl_SendDate.Text = model.SendDate.ToString();
                this.ltl_Contenet.Text = model.Contenet.ToString();
            }
        }
        #endregion
    }
}