/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 8时49分19秒
** 描    述:      用户信息管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;


using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using System.Text;
using System.Data;


namespace IFMP.sysmanage
{
    public partial class SysUserManage : PageBase
    {

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {

                    List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && t.IsAdmin).ToList();

                    this.ddl_DepartmentID.DataSource = DepartmentList;
                    this.ddl_DepartmentID.DataValueField = "ID";
                    this.ddl_DepartmentID.DataTextField = "Name";
                    this.ddl_DepartmentID.DataBind();
                    this.ddl_DepartmentID.Items.Insert(0, new ListItem("--请选择--", "-1"));
                }

                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["RealName"] = CommonFunction.GetCommoneString(this.txt_RealName.Text.ToString().Trim());
            ViewState["DepID"] = Convert.ToInt32(this.ddl_DepartmentID.SelectedValue);
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string realname = ViewState["RealName"].ToString();
            int depid = Convert.ToInt32(ViewState["DepID"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> Userlist = db.User.Where(t => t.RealName.Contains(realname)
                    && (depid == -1 || db.DepartmentUser.Where(m => m.DepartmentID == depid).Select(m => m.UserID).Contains(t.ID))
                    && t.IsDel != true).OrderByDescending(t => t.CreateDate).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();

                if (Userlist.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                this.rp_List.DataSource = Userlist;
                Pager.RecordCount = db.User.Count(t => t.RealName.Contains(realname)
                    && (depid == -1 || db.DepartmentUser.Where(m => m.DepartmentID == depid).Select(m => m.UserID).Contains(t.ID))
                    && t.IsDel != true);
                this.rp_List.DataBind();
                this.hf_CheckIDS.Value = "";

            }
        }
        #endregion


        #region 分页事件
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


        #region 导出事件
        protected void btn_OutPut_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder();
            DataTable dtOut = new DataTable();
            string realname = ViewState["RealName"].ToString();
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> UserList = db.User.Where(t => t.RealName.Contains(realname)
                    && t.IsDel != true).OrderByDescending(t => t.CreateDate).ToList();

                if (UserList.Count > 0)
                {
                    int depid = Convert.ToInt32(this.ddl_DepartmentID.SelectedValue);

                    List<Department> DepartmentList = db.Department.ToList();
                    List<DepartmentUser> DepartmentUserList = db.DepartmentUser.ToList();



                    dtOut.Columns.Add("用户名", typeof(string));
                    dtOut.Columns.Add("姓名", typeof(string));
                    dtOut.Columns.Add("手机号码", typeof(string));
                    dtOut.Columns.Add("部门", typeof(string));

                    foreach (User User in UserList)
                    {
                        List<string> list = new List<string>();
                        list.Add(User.UserName);
                        list.Add(User.RealName);
                        list.Add(User.Cellphone);
                        list.Add(depid == -1 ? DepartmentList.FirstOrDefault(t => t.IsAdmin && DepartmentUserList.Where(m => m.UserID == User.ID).Select(m => m.DepartmentID).Contains(t.ID)).Name : DepartmentList.FirstOrDefault(t => t.ID == depid).Name);
                        dtOut.Rows.Add(list.ToArray());
                    }


                }
                else
                {
                    ShowMessage("暂无数据导出！");
                    return;
                }
            }

            CommonFunction.ExportByWeb(dtOut, "", "用户基本信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            new SysLogDAO().AddLog(LogType.操作日志_导出, "导出用户信息", UserID);
        }
        #endregion


        #region 重置密码事件
        protected void btn_PwdReset_Click(object sender, EventArgs e)
        {
            string ids = hf_CheckIDS.Value.ToString();
            try
            {
                ids = ids.TrimEnd(',').TrimStart(',');

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (string id in ids.Split(','))
                    {
                        int selid = Convert.ToInt32(id);
                        User User = db.User.FirstOrDefault(t => t.ID == selid && t.IsDel != true);
                        if (User != null)
                        {
                            User.Password = new BaseUtils().BuildPW(User.UserName, "888888");
                        }
                        else
                        {
                            ShowMessage("密码重置失败");
                            return;
                        }
                        db.SaveChanges();
                        ShowMessage("密码重置成功");

                        new SysLogDAO().AddLog(LogType.操作日志_其他, "密码重置用户信息", UserID);
                    }
                }

                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion


        #region 删除事件
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            string ids = hf_CheckIDS.Value.ToString();

            try
            {

                ids = ids.TrimEnd(',').TrimStart(',');

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (string id in ids.Split(','))
                    {
                        int selid = Convert.ToInt32(id);
                        User User = db.User.FirstOrDefault(t => t.ID == selid && t.IsDel != true);
                        if (User != null)
                        {
                            User.IsDel = true;
                        }
                        else
                        {
                            ShowMessage("删除失败");
                            return;
                        }
                        db.SaveChanges();
                    }
                }
                ShowMessage("删除成功");
                new SysLogDAO().AddLog(LogType.操作日志_删除, "删除用户信息", UserID);
                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}