/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月13日 10时39分19秒
** 描    述:      部门信息管理页面
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
using System.Text;
using System.Data;

namespace IFMP.sysmanage
{
    public partial class DepartmentEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int DID
        {
            get
            {
                return GetQueryString<int>("id", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PIDBind();
                if (DID != -1)
                {
                    this.ddl_PID.Enabled = false;
                    //this.rbol_MState.Enabled = false;
                    InfoBind();
                }
            }
        }
        #endregion


        #region 递归类型菜单
        /// <summary>
        /// 类型
        /// </summary>
        private void PIDBind()
        {
            this.ddl_PID.Items.Add(new ListItem("--请选择--", "-1"));
            ModelParent(-1, this.ddl_PID, "");
        }
        private void ModelParent(int parentid, DropDownList ddl, string str)
        {
            string str_;
            List<Department> depList = db.Department.Where(t => t.IsDel != true && t.ParentID == parentid).OrderBy(t => t.CreateDate).ToList();
            for (int i = 0; i < depList.Count; i++)
            {
                if (depList[i].ParentID == -1)
                {
                    str_ = "";
                }
                else
                {
                    str_ = "├";
                }
                ListItem item = new ListItem();
                item.Text = str + str_ + depList[i].Name.ToString();     //Bind text
                item.Value = depList[i].ID.ToString();                                //Bind value
                int parent_id = Convert.ToInt32(item.Value.ToString());
                ddl.Items.Add(item);
                ModelParent(parent_id, ddl, str + "..");
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                Department Department = db.Department.FirstOrDefault(t => t.IsDel != true && t.ID == DID);
                if (Department != null)
                {
                    this.ddl_PID.SelectedValue = Department.ParentID.ToString();
                    this.txt_DepName.Text = Department.Name;
                    this.txt_Master.Text = Department.MasterUserID.ToString();
                    this.txt_DepOrder.Text = Department.Order.ToString();
                    this.txt_DepMark.Text = Department.Description;
                    this.rbol_MState.SelectedValue = Department.IsAdmin ? "1" : "0";
                    if (Department.ParentID > 0)
                        this.rbol_MState.Enabled = false;
                }
            }
        }
        #endregion


        #region 切换事件
        protected void btn_Department_Change(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int ParentID = Convert.ToInt32(this.ddl_PID.SelectedValue.ToString());
                    if (ParentID == -1)
                    {
                        rbol_MState.Enabled = true;
                    }
                    else
                    {
                        Department Department = db.Department.FirstOrDefault(t => t.ID == ParentID);
                        if (Department.IsAdmin)
                        {
                            rbol_MState.SelectedValue = "1";
                        }
                        else
                        {
                            rbol_MState.SelectedValue = "0";
                        }
                        rbol_MState.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    string message = "";
                    Department Department = db.Department.FirstOrDefault(t => t.ID == DID && t.IsDel != null);
                    if (Department == null)
                    {
                        //添加
                        Department = new Department();
                        Department.CreateDate = DateTime.Now;
                        Department.CreateUserID = UserID;
                        Department.Description = this.txt_DepMark.Text.ToString().Trim();
                        //可考虑将下级全部转换为同一类型
                        Department.IsAdmin = rbol_MState.SelectedValue == "1" ? true : false;
                        Department.IsDel = false;
                        Department.MasterUserID = Convert.ToInt32(this.txt_Master.Text);
                        Department.Name = this.txt_DepName.Text.ToString();
                        Department.Order = this.txt_DepOrder.Text.ToString() == "" ? 0 : Convert.ToInt32(this.txt_DepOrder.Text.ToString());
                        Department.ParentID = Convert.ToInt32(this.ddl_PID.SelectedValue.ToString());
                        if (db.Department.FirstOrDefault(t => t.Name == Department.Name) != null)
                        {
                            ShowMessage("部门名称已存在，请修改后重新添加");
                            return;
                        }
                        db.Department.Add(Department);
                        message = "添加名称为【" + Department.Name + "】的部门信息";
                        new SysLogDAO().AddLog(LogType.操作日志_添加, message, UserID);
                    }
                    else
                    {
                        //修改
                        //Department Department = db.Department.FirstOrDefault(t => t.ID == DID);
                        //if (Department == null)
                        //{
                        //    ShowMessage("部门不存在");
                        //    return;
                        //}

                        Department.Description = this.txt_DepMark.Text.ToString().Trim();
                        Department.IsAdmin = rbol_MState.SelectedValue == "1" ? true : false;
                        Department.MasterUserID = Convert.ToInt32(this.txt_Master.Text);
                        Department.Name = this.txt_DepName.Text.ToString();
                        Department.Order = this.txt_DepOrder.Text.ToString() == "" ? 0 : Convert.ToInt32(this.txt_DepOrder.Text.ToString());
                        //Department.ParentID = DID == -1 ? -1 : Convert.ToInt32(this.ddl_PID.SelectedValue.ToString());
                        if (db.Department.FirstOrDefault(t => t.Name == Department.Name && t.ID != Department.ID) != null)
                        {
                            ShowMessage("部门名称已存在，请修改后重新添加");
                            return;
                        }
                        message = "修改名称为【" + Department.Name + "】的部门信息";
                        new SysLogDAO().AddLog(LogType.操作日志_修改, message, UserID);
                    }
                    db.SaveChanges();
                    ShowMessage();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}