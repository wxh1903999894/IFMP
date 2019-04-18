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


namespace IFMP.integration
{
    public partial class UserTypeManage : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.hf_tflag.Value = TFlag.ToString();
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();

                    this.ddl_UserType.DataSource = DepartmentList;
                    this.ddl_UserType.DataValueField = "ID";
                    this.ddl_UserType.DataTextField = "Name";
                    this.ddl_UserType.DataBind();
                    this.ddl_UserType.Items.Insert(0, new ListItem("--请选择--", "-1"));
                }

                DataBindList();

            }
        }
        #region 绑定信息
        /// <summary>
        /// 信息绑定
        /// </summary>
        private void DataBindList()
        {
            int myheight = GetCookie<int>("ScreenH");
            if (myheight > 800)
            {
                Pager.PageSize = 15;
            }

            using (IFMPDBContext db = new IFMPDBContext())
            {
                int seldepid = Convert.ToInt32(this.ddl_UserType.SelectedValue);
                List<DepartmentUser> DepartmentUserList = db.DepartmentUser.Where(t => (seldepid == -1 || t.DepartmentID == seldepid) && db.User.Where(m => m.IsDel != true && m.UserState != UserState.离职 && !db.NoScoreUser.Select(k => k.UserID).Contains(m.ID)).Select(m => m.ID).Contains(t.UserID)).OrderBy(t => t.DepartmentID).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
                List<User> UserList = db.User.Where(t => t.IsDel != true && t.UserState != UserState.离职 && !db.NoScoreUser.Select(m => m.UserID).Contains(t.ID)).ToList();
                List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();
                //List<DepartmentUser> DepartmentUser = db.DepartmentUser.ToList();
                List<object> DataList = new List<object>();
                foreach (DepartmentUser DepartmentUser in DepartmentUserList)
                {
                    DataList.Add(new
                    {
                        DepartmentName = DepartmentList.FirstOrDefault(t => t.ID == DepartmentUser.DepartmentID).Name,
                        RealName = UserList.FirstOrDefault(t => t.ID == DepartmentUser.UserID).RealName,
                        DepartmentUser.ID
                    });
                }

                if (DepartmentUserList.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                this.rp_List.DataSource = DataList;
                Pager.RecordCount = db.DepartmentUser.Count(t => db.User.Where(m => m.IsDel != true && m.UserState != UserState.离职).Select(m => m.ID).Contains(t.UserID));
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
            Pager.CurrentPageIndex = 1;
            DataBindList();
        }
        #endregion

        #region 删除事件
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            string ids = hf_CheckIDS.Value.ToString().TrimEnd(',').TrimStart(',');
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    try
                    {

                        foreach (string id in ids.Split('.'))
                        {
                            int selid = Convert.ToInt32(id);
                            DepartmentUser DepartmentUser = db.DepartmentUser.FirstOrDefault(t => t.ID == selid);
                            if (DepartmentUser != null)
                                db.DepartmentUser.Remove(DepartmentUser);
                        }
                        db.SaveChanges();
                        new SysLogDAO().AddLog(LogType.操作日志_删除, "删除人员部门信息", UserID);
                        ShowMessage("删除成功");
                        DataBindList();
                        this.hf_CheckIDS.Value = "";
                    }
                    catch
                    {
                        ShowMessage("删除失败");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                ShowMessage(ex.Message);
                return;
            }
        }
        #endregion
    }
}