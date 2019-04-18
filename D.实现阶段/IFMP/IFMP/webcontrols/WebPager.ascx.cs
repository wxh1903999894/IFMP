using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IFMP.webcontrols
{
    public partial class WebPager : System.Web.UI.UserControl
    {
        #region 分页属性
        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordCount
        {
            get
            {
                if (ViewState["RecordCount"] == null)
                {
                    return 0;
                }

                return (int)ViewState["RecordCount"];
            }
            set
            {
                ViewState["RecordCount"] = value;
            }
        }

        /// <summary>
        /// 一页显示的记录数
        /// </summary>
        public int PageSize
        {
            get
            {
                if (ViewState["PageSize"] == null)
                {
                    return 10;
                }

                return (int)ViewState["PageSize"];
            }
            set
            {
                ViewState["PageSize"] = value;
            }
        }

        /// <summary>
        /// 页总数
        /// </summary>
        public int PageCount
        {
            get
            {
                int pageIndex = RecordCount / PageSize;
                return (RecordCount % PageSize == 0 ? pageIndex : pageIndex + 1);
            }
        }

        /// <summary>
        /// 页索引
        /// </summary>
        public int CurrentPageIndex
        {
            get
            {
                if (ViewState["CurrentPageIndex"] == null)
                {
                    return 1;
                }

                return (int)ViewState["CurrentPageIndex"];
            }
            set
            {
                ViewState["CurrentPageIndex"] = value;
            }
        }

        public event PageChangedEventHandler PageChanged;
        #endregion

        #region 页面加载
        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadPageData();
        }
        #endregion

        #region 加载分页信息
        private void LoadPageData()
        {
            //一页显示的记录数
            //ltlPageSize.Text = PageSize.ToString();
            this.ddl_PageSize.SelectedValue = PageSize.ToString();
            //总页数
            ltlPageCount.Text = PageCount.ToString();

            //记录总数
            ltlRecordCount.Text = RecordCount.ToString();

            //记录数为0时，按钮全部禁用
            if (ltlPageCount.Text == "0")
            {
                txtCurrentPage.Enabled = false;
                rvCurrentPage.MinimumValue = "0";
                rvCurrentPage.MaximumValue = "0";

                ibtnGo.Enabled = false;
                lbtnFirst.Enabled = false;
                lbtnPrevious.Enabled = false;
                lbtnNext.Enabled = false;
                lbtnLast.Enabled = false;
            }
            else
            {
                txtCurrentPage.Enabled = true;
                rvCurrentPage.MinimumValue = "1";
                rvCurrentPage.MaximumValue = ltlPageCount.Text;

                ibtnGo.Enabled = true;
                lbtnFirst.Enabled = true;

                //如果当前页数大于总页数，当前页数等于总页数
                if (CurrentPageIndex > PageCount)
                {
                    CurrentPageIndex = PageCount;
                    FirePageChanged();

                    return;
                }

                //设定上一页，下一页按钮启用状态
                if (CurrentPageIndex == 1)
                {
                    lbtnPrevious.Enabled = false;

                    if (ltlPageCount.Text != "1")
                    {
                        lbtnNext.Enabled = true;
                    }
                    else
                    {
                        lbtnNext.Enabled = false;
                    }
                }
                else if (CurrentPageIndex == PageCount)
                {
                    lbtnPrevious.Enabled = true;
                    lbtnNext.Enabled = false;
                }
                else
                {
                    lbtnPrevious.Enabled = true;
                    lbtnNext.Enabled = true;
                }

                lbtnLast.Enabled = true;
            }
            ////当索引页为首页是，清空跳转页数
            if (CurrentPageIndex == 1)
            {
                txtCurrentPage.Text = "";
            }
        }
        #endregion

        #region 共用按钮事件
        protected void ToggleCommon_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandArgument.ToString())
            {
                case "go":
                    if (txtCurrentPage.Text == "")
                    {
                        txtCurrentPage.Text = "1";
                    }
                    try
                    {
                        CurrentPageIndex = int.Parse(txtCurrentPage.Text);
                    }
                    catch
                    {
                        CurrentPageIndex = 1;
                    }
                    break;
                case "first":
                    CurrentPageIndex = 1;
                    break;
                case "previous":
                    CurrentPageIndex -= 1;
                    break;
                case "next":
                    CurrentPageIndex += 1;
                    break;
                case "last":
                    CurrentPageIndex = PageCount;
                    break;
            }
            txtCurrentPage.Text = CurrentPageIndex.ToString();
            FirePageChanged();
        }

        private void FirePageChanged()
        {
            if (PageChanged != null)
            {
                PageChanged(this, EventArgs.Empty);
            }

            if (Session["PageBack"] != null)
            {
                string condition = Session["PageBack"].ToString();
                Session["PageBack"] = string.Format("{0}\r{1}", condition.Substring(0, condition.LastIndexOf("\r")), CurrentPageIndex);
            }
        }
        #endregion

        #region 自定义分页事件
        public delegate void PageChangedEventHandler(object sender, EventArgs args);
        #endregion


        #region 设置每页显示页数
        protected void ddl_PageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageSize = Convert.ToInt32(this.ddl_PageSize.SelectedValue.ToString());
            FirePageChanged();
        }
        #endregion
    }
}