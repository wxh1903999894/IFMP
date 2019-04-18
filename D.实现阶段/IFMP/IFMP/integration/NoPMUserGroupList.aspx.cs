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
    public partial class NoPMUserGroupList : PageBase
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
            string realname = this.txt_Name.Text;
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<NoScoreUser> NoScoreUserList = db.NoScoreUser.Where(t => db.User.Where(m => m.RealName.Contains(realname) && m.IsDel != true && m.UserState != UserState.离职).Select(m => m.ID).Contains(t.UserID)).OrderBy(t => t.ID).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
                List<NoScoreUserDepartment> NoScoreUserDepartmentList = db.NoScoreUserDepartment.ToList();
                List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();
                List<User> UserList = db.User.Where(t => t.IsDel != true && t.UserState != UserState.离职).ToList();

                List<object> returnlist = new List<object>();
                foreach (NoScoreUser NoScoreUser in NoScoreUserList)
                {
                    string departmentname = "";
                    foreach (NoScoreUserDepartment NoScoreUserDepartment in NoScoreUserDepartmentList.Where(t => t.NoScoreUserID == NoScoreUser.ID).ToList())
                    {
                        departmentname = departmentname + DepartmentList.FirstOrDefault(t => t.ID == NoScoreUserDepartment.DepartmentID).Name + ",";
                    }
                    returnlist.Add(new
                    {
                        ID = NoScoreUser.ID,
                        RealName = UserList.FirstOrDefault(t => t.ID == NoScoreUser.UserID).RealName,
                        DepartmentName = departmentname.TrimEnd(',')
                    });
                }


                if (NoScoreUserList.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }

                this.rp_List.DataSource = returnlist;
                Pager.RecordCount = db.NoScoreUser.Count(t => db.User.Where(m => m.RealName.Contains(realname)).Select(m => m.ID).Contains(t.UserID));
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
            string ids = hf_CheckIDS.Value.ToString();
            try
            {
                ids = ids.TrimEnd(',').TrimStart(',');
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    try
                    {
                        foreach (string id in ids.Split(','))
                        {
                            int selid = Convert.ToInt32(id);

                            NoScoreUser NoScoreUser = db.NoScoreUser.FirstOrDefault(t => t.ID == selid);
                            if (NoScoreUser != null)
                            {
                                List<NoScoreUserDepartment> NoScoreUserDepartmentList = db.NoScoreUserDepartment.Where(t => t.NoScoreUserID == NoScoreUser.ID).ToList();
                                db.NoScoreUserDepartment.RemoveRange(NoScoreUserDepartmentList);
                            }
                            db.NoScoreUser.Remove(NoScoreUser);

                            db.SaveChanges();
                            new SysLogDAO().AddLog(LogType.操作日志_删除, "删除不排名人员信息", UserID);
                            ShowMessage("删除成功");
                            DataBindList();
                            this.hf_CheckIDS.Value = "";
                        }
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