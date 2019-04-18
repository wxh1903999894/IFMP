/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月1日 16时42分19秒
** 描    述:      用户信息管理页面
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

namespace IFMP.taskflow
{
    public partial class TaskManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<ClassTypeEnums>(this.ddl_ClassType, "-2");

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();

                    this.ddl_TableType.DataSource = TableTypeList;
                    this.ddl_TableType.DataValueField = "ID";
                    this.ddl_TableType.DataTextField = "Name";
                    this.ddl_TableType.DataBind();
                    this.ddl_TableType.Items.Insert(0, new ListItem("--请选择--", "-2"));
                }

                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["TaskName"] = CommonFunction.GetCommoneString(this.txt_TaskName.Text.ToString().Trim());
            ViewState["ClassType"] = this.ddl_ClassType.SelectedValue.ToString();
            ViewState["TableType"] = this.ddl_TableType.SelectedValue.ToString();
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string name = ViewState["TaskName"].ToString();
            int clatype = Convert.ToInt32(ViewState["ClassType"].ToString());
            int tabtype = Convert.ToInt32(ViewState["TableType"].ToString());
            var tasklist = from task in db.Task
                           join tabletype in db.TableType on task.TableTypeID equals tabletype.ID
                           where task.IsDel != true && task.TaskName.Contains(name)
                           && (clatype == -2 || task.ClassType == (ClassTypeEnums)clatype)
                           && (tabtype == -2 || task.TableTypeID == tabtype)
                           orderby task.CreateDate
                           select new
                           {
                               task.ID,
                               task.TaskName,
                               task.ClassType,
                               task.CreateDate,
                               TableTypeName = tabletype.Name,
                           };

            //List<Task> tasklist = db.Task.Where(t => t.IsDel != true && t.TaskName.Contains(name)
            //    && (clatype == -2 || t.ClassType == (ClassTypeEnums)clatype)
            //    && (tabtype == -2 || t.TableTypeID == tabtype)).OrderBy(t => t.CreateDate).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            //int total = db.Task.Count(t => t.IsDel != true && t.TaskName.Contains(name)
            //    && (clatype == -2 || t.ClassType == (ClassTypeEnums)clatype)
            //    && (tabtype == -2 || t.TableTypeID == tabtype));
            if (tasklist.Count() > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = tasklist.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            Pager.RecordCount = tasklist.Count();
            this.rp_List.DataBind();
        }
        #endregion


        #region 查询条件
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
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


        #region 删除事件
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                string ids = this.hf_CheckIDS.Value.ToString();
                ids = ids.TrimEnd(',').TrimStart(',');
                foreach (string id in ids.Split(','))
                {
                    int iid = Convert.ToInt32(id);
                    Task model = db.Task.FirstOrDefault(t => t.ID == iid);
                    if (model != null)
                    {
                        model.IsDel = true;
                    }
                    else
                    {
                        ShowMessage("删除失败");
                        return;
                    }
                }
                db.SaveChanges();
                ShowMessage("删除成功");
                new SysLogDAO().AddLog(LogType.操作日志_删除, "删除任务信息", UserID);
                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
            }
        }
        #endregion
    }
}