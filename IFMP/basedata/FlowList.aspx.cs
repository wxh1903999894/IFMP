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
    public partial class FlowList : PageBase
    {


        #region 参数集合
        /// <summary>
        /// 表单类型
        /// </summary>
        public int TableTypeID
        {
            get
            {
                return GetQueryString<int>("type", -1);
            }
        }
        #endregion


        #region 页面初始化


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hf_TableTypeID.Value = TableTypeID.ToString();
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
            //DateTime begin = Convert.ToDateTime(ViewState["begin"]);
            //DateTime end = Convert.ToDateTime(ViewState["end"]);
            string Name = ViewState["Name"].ToString();


            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<Flow> FlowList = db.Flow.Where(t => t.Name.Contains(Name) && t.TableTypeID == TableTypeID).ToList();

                if (FlowList.Count > 0)
                {
                    tr_null.Visible = false;
                }
                else
                {
                    tr_null.Visible = true;
                }

                rp_List.DataSource = FlowList.Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList();

                Pager.RecordCount = FlowList.Count;
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