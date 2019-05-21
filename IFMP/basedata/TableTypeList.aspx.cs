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

namespace IFMP.basedata
{
    public partial class TableTypeList : PageBase
    {
        #region 页面初始化

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<ProductionLine> ProductionLineList = db.ProductionLine.Where(t => t.IsDel != true).ToList();

                    this.ddl_ProductionLineID.DataSource = ProductionLineList;
                    this.ddl_ProductionLineID.DataValueField = "ID";
                    this.ddl_ProductionLineID.DataTextField = "Name";
                    this.ddl_ProductionLineID.DataBind();
                    this.ddl_ProductionLineID.Items.Insert(0, new ListItem("--请选择--", "-2"));

                }

                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["Name"] = this.txt_Name.Text.Trim();
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            string Name = ViewState["Name"].ToString();
            int ProductionLineID = Convert.ToInt32(ddl_ProductionLineID.SelectedValue);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<object> ReturnList = new List<object>();
                List<TableType> TableTypeList = db.TableType.Where(t => t.Name.Contains(Name) && (ProductionLineID == -2 || t.ProductionLineID == ProductionLineID) && t.IsDel != true).ToList();
                List<ProductionLine> ProductionLineList = db.ProductionLine.Where(t => t.IsDel != true).ToList();

                foreach (TableType TableType in TableTypeList.Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList())
                {
                    ReturnList.Add(new
                    {
                        TableType.ID,
                        TableType.Name,
                        TableType.IsMulti,
                        ProductionLineName = ProductionLineList.FirstOrDefault(t => t.ID == TableType.ProductionLineID)
                    });
                }

                if (TableTypeList.Count > 0)
                {
                    tr_null.Visible = false;
                }
                else
                {
                    tr_null.Visible = true;
                }

                rp_List.DataSource = ReturnList;

                Pager.RecordCount = TableTypeList.Count;
                rp_List.DataBind();
            }

        }
        #endregion

        #region 分页
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