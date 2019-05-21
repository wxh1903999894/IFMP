/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月27日 11时29分19秒
** 描    述:      字典信息管理页面
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

namespace IFMP.dictionary
{
    public partial class TableColumnManage : PageBase
    {
        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindList();
            }
        }
        #endregion
       

        #region 数据绑定
        private void DataBindList()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();
                if (TableTypeList.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                this.rp_List.DataSource = TableTypeList;
                this.rp_List.DataBind();
            }

        }
        #endregion
    }
}