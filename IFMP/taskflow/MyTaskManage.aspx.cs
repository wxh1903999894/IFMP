/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月6日 16时44分19秒
** 描    述:      任务管理页面
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

namespace IFMP.taskflow
{
    public partial class MyTaskManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();

                    this.ddl_TableType.DataSource = TableTypeList;
                    this.ddl_TableType.DataValueField = "ID";
                    this.ddl_TableType.DataTextField = "Name";
                    this.ddl_TableType.DataBind();
                    this.ddl_TableType.Items.Insert(0, new ListItem("--请选择--", "-2"));
                }


                CommonFunction.BindEnum<ClassTypeEnums>(this.ddl_ClassType, "-2");
                CommonFunction.BindEnum<ApplyTypeEnums>(this.ddl_ApplyType, "-2");
                this.ddl_ApplyType.Items.Insert(1, new ListItem("未开始", "-3"));
                this.ddl_ApplyType.SelectedValue = Convert.ToInt32(ApplyTypeEnums.未交).ToString();
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["TaskName"] = CommonFunction.GetCommoneString(this.txt_TaskName.Text.Trim());
            ViewState["ClassType"] = this.ddl_ClassType.SelectedValue.ToString();
            ViewState["TableType"] = this.ddl_TableType.SelectedValue.ToString();
            ViewState["ApplyType"] = this.ddl_ApplyType.SelectedValue.ToString();
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string taskname = ViewState["TaskName"].ToString();
            int tabletypeid = Convert.ToInt32(ViewState["TableType"].ToString());
            int classtype = Convert.ToInt32(ViewState["ClassType"].ToString());
            int applytype = Convert.ToInt32(ViewState["ApplyType"].ToString());
            var list = from taskflow in db.TaskFlow
                       join task in db.Task on taskflow.TaskID equals task.ID
                       join flow in db.Flow on taskflow.FlowID equals flow.ID
                       join tabletype in db.TableType on task.TableTypeID equals tabletype.ID
                       where taskflow.UserID == UserID
                       && (applytype == -3 ? taskflow.BeginDate > DateTime.Now : (applytype == -2 || taskflow.ApplyType == (ApplyTypeEnums)applytype))
                       && task.TaskName.Contains(taskname) && task.IsDel != true
                       && (tabletypeid == -2 || task.TableTypeID == tabletypeid)
                       && (classtype == -2 || task.ClassType == (ClassTypeEnums)classtype)
                       orderby task.CreateDate
                       select new
                       {
                           taskflow.ID,
                           taskflow.BeginDate,
                           taskflow.EndDate,
                           taskflow.ApplyType,
                           ApplyTypeName = (taskflow.BeginDate > DateTime.Now) ? "未开始" : taskflow.ApplyType.ToString(),
                           TaskID = task.ID,
                           task.TaskName,
                           task.ClassType,
                           task.TableTypeID,
                           TableTypeName = tabletype.Name,
                           FlowID = flow.ID,
                           flow.IsAudit,
                           flow.Name,
                           flow.ParentID,
                           //IsMulti = db.TableType.FirstOrDefault(t => t.ID == task.TableTypeID && t.IsDel != true) == null ? false : db.TableType.FirstOrDefault(t => t.ID == task.TableTypeID && t.IsDel != true).IsMulti,//是否多表
                           IsVis = tabletype.IsMulti == true ? true : (taskflow.ApplyType == ApplyTypeEnums.未交 ? true : false)
                       };
            int total = list.Count();
            if (total > 0)
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


        #region 是否隐藏按钮
        public bool GetVisible(object sender, object sender1)
        {
            try
            {
                string applytype = sender.ToString();
                string IsMulti = sender1.ToString();
                if (IsMulti == "False")
                {
                    if (applytype == ApplyTypeEnums.未交.ToString())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}