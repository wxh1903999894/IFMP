/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      lfz
** 创建日期:      2016年07月20日 15时40分47秒
** 描    述:     客户信息管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;


namespace IFMP.sysmanage
{
    public partial class SysUserSelete : PageBase
    {
        #region 参数集合
        public int Flag
        {
            get
            {
                return GetQueryString<int>("flag", -1);
            }
        }
        #endregion


        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.hf_Flag.Value = Flag.ToString();
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();


                    this.ddl_Department.DataSource = DepartmentList;
                    this.ddl_Department.DataValueField = "ID";
                    this.ddl_Department.DataTextField = "Name";
                    this.ddl_Department.DataBind();
                    this.ddl_Department.Items.Add(new ListItem("--请选择--", "-1"));
                    this.ddl_Department.SelectedValue = "-1";
                }


                ViewState["RealName"] = CommonFunction.GetCommoneString(this.txt_RealName.Text.Trim());//姓名
                ViewState["DepID"] = Convert.ToInt32(this.ddl_Department.SelectedValue);//名称
                ViewState["Begin"] = this.txt_Begin.Text.Trim() == "" ? "1900-01-01" : this.txt_Begin.Text.Trim() + " 00:00:00";//操作日期
                ViewState["End"] = this.txt_End.Text.Trim() == "" ? "9999-12-31" : this.txt_End.Text.Trim() + " 23:59:59";//操作日期

                DataBindList();
            }
        }
        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void DataBindList()
        {
            string realname = ViewState["RealName"].ToString();
            int depid = Convert.ToInt32(ViewState["DepID"]);
            DateTime begindate = Convert.ToDateTime(ViewState["Begin"]);
            DateTime enddate = Convert.ToDateTime(ViewState["End"]);
            if (begindate > enddate)
            {
                DateTime tempdate = begindate;
                begindate = enddate;
                enddate = tempdate;
            }


            using (IFMPDBContext db = new IFMPDBContext())
            {
                //List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();
                List<User> UserList = db.User.Where(t => t.UserState != UserState.离职
                    && t.IsDel != true
                    && t.RealName.Contains(realname)
                    && (depid == -1 || db.DepartmentUser.Where(m => m.DepartmentID == depid).Select(m => m.UserID).Contains(t.ID))
                    && db.UserDetails.Where(m => m.HireDate >= begindate && m.HireDate <= enddate).Select(m => m.UserID).Contains(t.ID)
                    ).OrderBy(t => t.CreateDate).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();

                List<object> returnlist = new List<object>();
                foreach (User User in UserList)
                {
                    returnlist.Add(new
                    {
                        User.RealName,
                        User.ID,
                        User.UserName,
                        PostName = db.Post.FirstOrDefault(t => db.PostUser.FirstOrDefault(m => m.UserID == User.ID).PostID == t.ID) == null ? "" : db.Post.FirstOrDefault(t => db.PostUser.FirstOrDefault(m => m.UserID == User.ID).PostID == t.ID).Name,
                        DepartmentName = new DepartmentDAO().GetUserAdminDepartment(User.ID),
                        HireDate = db.UserDetails.FirstOrDefault(t => t.UserID == UserID) == null ? "" : db.UserDetails.FirstOrDefault(t => t.UserID == UserID).HireDate.ToString("yyyy-MM-dd")
                    });

                }

                if (UserList.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                this.rp_List.DataSource = returnlist;
                Pager.RecordCount = db.User.Count(t => t.UserState != UserState.离职
                    && t.IsDel != true
                    && t.RealName.Contains(realname)
                    && (depid == -1 || db.DepartmentUser.Where(m => m.DepartmentID == depid).Select(m => m.UserID).Contains(t.ID))
                    && db.UserDetails.Where(m => m.HireDate >= begindate && m.HireDate <= enddate).Select(m => m.UserID).Contains(t.ID)
                    );
                rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }
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
            this.hf_CheckIDS.Value = "";
            this.hf_CheckNames.Value = "";
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
            ViewState["RealName"] = CommonFunction.GetCommoneString(this.txt_RealName.Text.Trim());//姓名
            ViewState["DepID"] = Convert.ToInt32(this.ddl_Department.SelectedValue);//名称
            ViewState["Begin"] = this.txt_Begin.Text.Trim() == "" ? "1900-01-01" : this.txt_Begin.Text.Trim() + " 00:00:00";//操作日期
            ViewState["End"] = this.txt_End.Text.Trim() == "" ? "9999-12-31" : this.txt_End.Text.Trim() + " 23:59:59";//操作日期
            Pager.CurrentPageIndex = 1;
            this.hf_CheckIDS.Value = "";
            this.hf_CheckNames.Value = "";
            DataBindList();
        }
        #endregion

    }
}