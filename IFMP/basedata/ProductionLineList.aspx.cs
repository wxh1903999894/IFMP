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
    public partial class ProductionLineList : PageBase
    {
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
        public void GetCondition()
        {
            ViewState["Name"] = this.txt_Name.Text.Trim();
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            string Name = ViewState["Name"].ToString();


            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<ProductionLine> ProductionLineList = db.ProductionLine.Where(t => t.Name.Contains(Name)).ToList();
                //ProductionLineList.ForEach(t => t.TableTypeList = db.TableType.Where(m => m.ProductionLineID == t.ID).ToList());

                List<object> ReturnList = new List<object>();
                List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();
                foreach (ProductionLine ProductionLine in ProductionLineList.Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList())
                {
                    ReturnList.Add(new
                    {
                        ProductionLine.ID,
                        ProductionLine.Name,
                        TableListCount = TableTypeList.Count(t => t.ProductionLineID == ProductionLine.ID)
                    });
                }

                if (ProductionLineList.Count > 0)
                {
                    tr_null.Visible = false;
                }
                else
                {
                    tr_null.Visible = true;
                }

                rp_List.DataSource = ReturnList;

                Pager.RecordCount = ProductionLineList.Count;
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