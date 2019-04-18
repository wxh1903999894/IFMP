/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月25日 16时25分19秒
** 描    述:      员工档案信息管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
    public partial class EmployeeManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<UserType>(this.ddl_UserType, "-2");
                List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && t.IsAdmin).ToList();
                this.ddl_DepartmentID.DataSource = DepartmentList;
                this.ddl_DepartmentID.DataValueField = "ID";
                this.ddl_DepartmentID.DataTextField = "Name";
                this.ddl_DepartmentID.DataBind();
                this.ddl_DepartmentID.Items.Insert(0, new ListItem("--请选择--", "-1"));

                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["RealName"] = CommonFunction.GetCommoneString(this.txt_RealName.Text.ToString().Trim());
            ViewState["DepID"] = this.ddl_DepartmentID.SelectedValue.ToString();
            ViewState["UserType"] = this.ddl_UserType.SelectedValue.ToString();
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string realname = ViewState["RealName"].ToString();
            int depid = Convert.ToInt32(ViewState["DepID"].ToString());
            int usertype = Convert.ToInt32(ViewState["UserType"].ToString());

            var list = from detaillist in db.UserDetails.Where(t => t.IsDel != true && (usertype == -2 || t.UserType == (UserType)usertype))
                       join deplist in db.DepartmentUser.Where(t => (depid == -1 || t.DepartmentID == depid)) on detaillist.UserID equals deplist.UserID
                       join departmentlist in db.Department.Where(t => t.IsDel != true && t.IsAdmin) on deplist.DepartmentID equals departmentlist.ID
                       join userlist in db.User.Where(t => t.IsDel != true && t.RealName.Contains(realname)) on detaillist.UserID equals userlist.ID
                       join postlist in db.PostUser on detaillist.UserID equals postlist.UserID
                       orderby detaillist.CreateDate
                       select new
                       {
                           detaillist.ID,
                           detaillist.UserType,
                           detaillist.Sex,
                           detaillist.BirthDate,
                           detaillist.Job,
                           detaillist.HireDate,
                           userlist.UserNumber,
                           userlist.RealName,
                           userlist.UserState,
                           DepName = departmentlist.Name,
                           PostName = db.Post.FirstOrDefault(t => t.ID == postlist.PostID).Name
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
            this.rp_List.DataBind();
            this.hf_CheckIDS.Value = "";
        }
        #endregion


        #region 查询事件
        protected void btn_Search_Click(object sender, EventArgs e)
        {
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


        #region 导出功能
        protected void btn_OutPut_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder();
            DataTable dtOut = new DataTable();
            string realname = ViewState["RealName"].ToString();
            int usertype = Convert.ToInt32(this.ddl_UserType.SelectedValue);
            int depid = Convert.ToInt32(this.ddl_DepartmentID.SelectedValue);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<UserDetails> UserList = db.UserDetails.Where(t => db.User.Where(m => m.RealName.Contains(realname) && m.IsDel != true).Select(m => m.ID).Contains(t.UserID)
                    && db.DepartmentUser.Where(n => (n.DepartmentID == depid || depid == -1)).Select(n => n.UserID).Contains(t.UserID)
                    && t.IsDel != true && (t.UserType == (UserType)usertype || usertype == -2)).OrderByDescending(t => t.CreateDate).ToList();
                if (UserList.Count > 0)
                {
                    List<Department> DepartmentList = db.Department.ToList();
                    List<DepartmentUser> DepartmentUserList = db.DepartmentUser.ToList();

                    dtOut.Columns.Add("员工编号", typeof(string));
                    dtOut.Columns.Add("姓名", typeof(string));
                    dtOut.Columns.Add("部门", typeof(string));
                    dtOut.Columns.Add("岗位", typeof(string));
                    dtOut.Columns.Add("手机号码", typeof(string));
                    dtOut.Columns.Add("性别", typeof(string));
                    dtOut.Columns.Add("政治面貌", typeof(string));
                    dtOut.Columns.Add("民族", typeof(string));
                    dtOut.Columns.Add("出生日期", typeof(string));
                    dtOut.Columns.Add("职位", typeof(string));
                    dtOut.Columns.Add("入职日期", typeof(string));
                    dtOut.Columns.Add("转正日期", typeof(string));
                    dtOut.Columns.Add("家庭地址", typeof(string));
                    dtOut.Columns.Add("员工类别", typeof(string));
                    dtOut.Columns.Add("状态", typeof(string));

                    foreach (UserDetails udetail in UserList)
                    {
                        List<string> list = new List<string>();
                        list.Add(db.User.FirstOrDefault(t => t.ID == udetail.UserID).UserNumber);
                        list.Add(db.User.FirstOrDefault(t => t.ID == udetail.UserID).RealName);
                        list.Add(db.Department.FirstOrDefault(t => t.ID == (db.DepartmentUser.FirstOrDefault(m => m.UserID == udetail.UserID)).DepartmentID).Name);
                        list.Add(db.Post.FirstOrDefault(t => t.ID == (db.PostUser.FirstOrDefault(m => m.UserID == udetail.UserID)).PostID).Name);
                        list.Add(db.User.FirstOrDefault(t => t.ID == udetail.UserID).Cellphone);
                        list.Add(Enum.GetName(typeof(Sex), udetail.Sex));
                        list.Add(Enum.GetName(typeof(Polity), udetail.Polity));
                        list.Add(Enum.GetName(typeof(Nationality), udetail.Nationality));
                        list.Add(udetail.BirthDate == null ? "" : Convert.ToDateTime(udetail.BirthDate).ToString("yyyy-MM-dd"));
                        list.Add(udetail.Job);
                        list.Add(Convert.ToDateTime(udetail.HireDate).ToString("yyyy-MM-dd"));
                        list.Add(udetail.QualifiedDate == null ? "" : Convert.ToDateTime(udetail.QualifiedDate).ToString("yyyy-MM-dd"));
                        list.Add(udetail.Address);
                        list.Add(Enum.GetName(typeof(UserType), udetail.UserType));
                        list.Add(Enum.GetName(typeof(UserState), db.User.FirstOrDefault(t => t.ID == udetail.UserID).UserState));
                        dtOut.Rows.Add(list.ToArray());
                    }
                }
                else
                {
                    ShowMessage("暂无数据导出！");
                    return;
                }
            }

            CommonFunction.ExportByWeb(dtOut, "", "员工档案信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            new SysLogDAO().AddLog(LogType.操作日志_导出, "导出员工档案信息", UserID);
        }
        #endregion
    }
}