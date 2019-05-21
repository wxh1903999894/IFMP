/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月1日 16时42分19秒
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

namespace IFMP.taskflow
{
    public partial class AlertTableDataManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();

                    this.ddl_TableType.DataSource = TableTypeList;
                    this.ddl_TableType.DataValueField = "ID";
                    this.ddl_TableType.DataTextField = "Name";
                    this.ddl_TableType.DataBind();
                    this.ddl_TableType.Items.Insert(0, new ListItem("--请选择--", "-2"));
                }

                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["TableType"] = this.ddl_TableType.SelectedValue.ToString();
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            int tabletypeid = Convert.ToInt32(ViewState["TableType"].ToString());
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"]));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"]), false);


            var TableData = from tabledata in db.TableData
                            join table in db.Table on tabledata.TableID equals table.ID
                            join tablecolumn in db.TableColumn on tabledata.TableColumnID equals tablecolumn.ID
                            join user in db.User on tabledata.CreateUserID equals user.ID
                            join tabletype in db.TableType on table.TableTypeID equals tabletype.ID
                            where tabledata.CreateDate >= begin && tabledata.CreateDate <= end
                            && (tabletypeid == -2 || table.TableTypeID == tabletypeid)
                            && tabledata.IsAlert == true
                            orderby tabledata.CreateDate
                            select new
                            {
                                tabledata.ID,
                                tabledata.CreateDate,
                                tabledata.Data,
                                TableTypeName = tabletype.Name,
                                user.RealName,
                                TableID = table.ID,
                                TableColumnName = tablecolumn.ColumnName,
                            };




            if (TableData.Count() > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = TableData.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            Pager.RecordCount = TableData.Count();
            this.rp_List.DataBind();
        }
        #endregion


        #region 查询条件
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 分页事件
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion

    }
}