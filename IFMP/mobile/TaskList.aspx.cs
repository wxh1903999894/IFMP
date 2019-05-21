/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月27日 10时53分
** 描    述:      用户信息管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Data;
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

namespace IFMP.mobile
{
    public partial class TaskList : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定当前班次用户
                List<User> UserList = db.User.Where(t => t.IsDel != true && t.UserState != UserState.离职 && db.BaseClassUser.Where(m => m.BaseClassID == BaseClassID).Select(m => m.UserID).Contains(t.ID)).ToList();

                this.ddl_User.DataSource = UserList;
                this.ddl_User.DataValueField = "ID";
                this.ddl_User.DataTextField = "RealName";
                this.ddl_User.DataBind();
                this.ddl_User.Items.Insert(0, new ListItem("--请选择--", "-1"));

                if (!string.IsNullOrEmpty(GetCookie<string>("TableUserID")))
                {
                    this.ddl_User.SelectedValue = GetCookie<string>("TableUserID");
                }

                //this.ddl_User.DataSource = UserList.ToList();
                //this.ddl_User.DataBind();

                DataBindList();
            }
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            int SelUserID = Convert.ToInt32(ddl_User.SelectedValue);
            //考虑经常不按时上下班
            //不能以结束时间为界限
            //方案1 加一个固定的时间，比如2小时
            //方案2 找到同flow的下一个的开始时间之前,若不存在下一个flow就一直显示
            DateTime ShowDate = DateTime.Now.AddHours(-8);
            //DateTime ShowDate = DateTime.Now.AddDays(-10);

            List<TaskFlow> taskflowlist = db.TaskFlow.Where(t => (SelUserID == -1 || t.UserID == SelUserID)
                && t.BaseClassID == BaseClassID
                && t.BeginDate < DateTime.Now
                //&& (db.TaskFlow.FirstOrDefault(m => m.FlowID == t.FlowID && m.TaskID != t.FlowID && m.EndDate > t.EndDate) != null ? DateTime.Now < db.TaskFlow.FirstOrDefault(m => m.FlowID == t.FlowID && m.TaskID != t.FlowID && m.EndDate > t.EndDate).EndDate : true)
                && t.EndDate > ShowDate
                && (t.ApplyDate == null || db.Flow.Where(m => db.TableType.Where(l => l.IsMulti == true).Select(l => l.ID).Contains(m.TableTypeID)).Select(m => m.ID).Contains(t.FlowID))
                ).OrderBy(t => t.EndDate).ToList();

            List<Task> tasklist = db.Task.Where(m => db.TaskFlow.Where(t => (SelUserID == -1 || t.UserID == SelUserID)
                && t.BaseClassID == BaseClassID
                && t.BeginDate < DateTime.Now
                //&& (db.TaskFlow.FirstOrDefault(k => k.FlowID == t.FlowID && k.TaskID != t.FlowID && k.EndDate > t.EndDate) != null ? DateTime.Now < db.TaskFlow.FirstOrDefault(k => k.FlowID == t.FlowID && k.TaskID != t.FlowID && k.EndDate > t.EndDate).EndDate : true)
                && t.EndDate > ShowDate
                ).Select(t => t.TaskID).Contains(m.ID)).ToList();

            List<User> UserList = db.User.ToList();
            List<object> list = new List<object>();
            List<TableType> tabletypelist = db.TableType.ToList();

            int times = 1;
            string username = "";
            foreach (Task task in tasklist)
            {
                List<Flow> FlowList = db.Flow.ToList();
                FlowList = FlowList.Where(t => taskflowlist.Where(m => m.TaskID == task.ID).Select(m => m.FlowID).Contains(t.ID)).ToList();
                foreach (Flow Flow in FlowList)
                {
                    username = "";
                    List<TaskFlow> seltaskflowlist = taskflowlist.Where(t => t.FlowID == Flow.ID && t.TaskID == task.ID).ToList();
                    if (seltaskflowlist.Count > 0)
                    {
                        DateTime enddate = seltaskflowlist.FirstOrDefault().EndDate;
                        foreach (TaskFlow seltaskflow in seltaskflowlist)
                        {
                            username = username + UserList.FirstOrDefault(t => t.ID == seltaskflow.UserID).RealName + ",";
                        }

                        list.Add(new
                        {
                            TaskID = task.ID,
                            TType = (int)task.TableTypeID,
                            task.TaskName,
                            TableTypeName = db.TableType.FirstOrDefault(t => t.ID == task.TableTypeID) == null ? "" : db.TableType.FirstOrDefault(t => t.ID == task.TableTypeID).Name,
                            FlowID = Flow.ID,
                            FlowName = Flow.Name,
                            JoinUser = username.TrimEnd(','),
                            EndDate = enddate,
                            IsAudit = Flow.IsAudit,
                            times = times,
                            IsEnd = seltaskflowlist.FirstOrDefault(t => t.ApplyDate == null) == null ? !tabletypelist.FirstOrDefault(t => t.ID == task.TableTypeID).IsMulti : false,
                            IsMulti = tabletypelist.FirstOrDefault(t => t.ID == task.TableTypeID).IsMulti,
                            IsMultiAccept = tabletypelist.FirstOrDefault(t => t.ID == task.TableTypeID).IsMulti ? (seltaskflowlist.FirstOrDefault(t => t.ApplyDate == null) == null ? true : false) : false,

                        });
                        times++;
                    }
                }
            }
            if (list.Count > 0)
            {
                this.div_null.Visible = false;
                this.rp_List.Visible = true;
                this.rp_List.DataSource = list.ToList();
                this.rp_List.DataBind();
            }
            else
            {
                div_null.Visible = true;
                this.rp_List.Visible = false;
                this.rp_List.DataSource = list.ToList();
                this.rp_List.DataBind();
            }
        }
        #endregion


        #region 数据绑定事件
        protected void ddl_User_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCookie("TableUserID", ddl_User.SelectedValue);
            //Context.Request.Cookies["TableUserID"].Value = ddl_User.SelectedValue;
            DataBindList();
        }
        #endregion
    }
}