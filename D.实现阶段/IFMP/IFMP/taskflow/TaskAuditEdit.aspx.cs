﻿/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月10日 13时50分19秒
** 描    述:      我的任务审核页面
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
    public partial class TaskAuditEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        /// <summary>
        /// 任务ID
        /// </summary>
        public int TaskID
        {
            get
            {
                return GetQueryString<int>("taskid", -1);
            }
        }

        /// <summary>
        /// 任务流程ID
        /// </summary>
        public int TaskFlowID
        {
            get
            {
                return GetQueryString<int>("taskflowid", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InfoBind();
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            Task model = db.Task.FirstOrDefault(t => t.ID == TaskID);
            if (model != null)
            {
                this.ltl_ClassType.Text = model.ClassType.ToString();
                this.ltl_TableType.Text = db.TableType.FirstOrDefault(t => t.ID == model.TableTypeID).Name.ToString();
                this.ltl_TaskName.Text = model.TaskName.ToString();
                this.rp_List.Visible = true;
                DataBindList();
            }
        }
        #endregion


        #region 流程信息绑定
        private void DataBindList()
        {
            Task model = db.Task.FirstOrDefault(t => t.ID == TaskID);
            //Flow flow = db.Flow.FirstOrDefault(t => t.ID == db.TaskFlow.FirstOrDefault(m => m.ID == TaskFlowID).FlowID);//获取当前流程信息
            //List<Flow> flowlist = db.Flow.Where(t => t.TableTypeID == model.TableTypeID && t.ParentID <= flow.ParentID).OrderBy(t => t.ParentID).ToList();

            List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == model.TableTypeID).ToList();
            FlowList = new FlowDAO().GetFlowLevel(FlowList);
            Flow flow = FlowList.FirstOrDefault(t => t.ID == db.TaskFlow.FirstOrDefault(m => m.ID == TaskFlowID).FlowID);
            FlowList = FlowList.Where(t => t.Level <= flow.Level).ToList();

            if (FlowList.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = FlowList;
            this.rp_List.DataBind();

            int clatype = Convert.ToInt32(model.ClassType);//班次类型
            List<BaseClass> classlist = db.BaseClass.Where(t => t.ClassType == (ClassTypeEnums)clatype && t.IsDel != true).ToList();
            if (classlist.Count > 0)
            {
                cbl_Select.DataTextField = "Name";
                cbl_Select.DataValueField = "ID";
                cbl_Select.DataSource = classlist;
                cbl_Select.DataBind();
            }

            var testlist = db.TaskFlow.Where(t => t.BaseClassID != null && t.TaskID == TaskID).Select(t => t.BaseClassID).Distinct().ToList();
            if (testlist.Count > 0)
            {
                for (int i = 0; i < testlist.Count; i++)
                {
                    for (int j = 0; j < cbl_Select.Items.Count; j++)
                    {
                        if (cbl_Select.Items[j].Value == testlist[i].Value.ToString())
                        {
                            cbl_Select.Items[j].Selected = true;
                        }
                    }
                }
            }
        }
        #endregion


        #region 数据绑定事件
        protected void rp_List_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Task model = db.Task.FirstOrDefault(t => t.ID == TaskID);
                int clatype = Convert.ToInt32(model.ClassType);//班次类型
                int tabtype = Convert.ToInt32(model.TableTypeID);//表单类型
                HiddenField hfFlowID = (HiddenField)e.Item.FindControl("hf_FlowID");
                int flowid = Convert.ToInt32(hfFlowID.Value);
                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chk_ClassList");
                Literal ltl_SysUser = (Literal)e.Item.FindControl("ltl_SysUser");
                Literal ltl_BeginDate = (Literal)e.Item.FindControl("ltl_BeginDate");
                Literal ltl_EndDate = (Literal)e.Item.FindControl("ltl_EndDate");
                Literal ltl_RemindDate = (Literal)e.Item.FindControl("ltl_RemindDate");

                //根据班次类型获取所有班次信息
                List<BaseClass> classlist = db.BaseClass.Where(t => t.ClassType == (ClassTypeEnums)clatype && t.IsDel != true).ToList();
                List<BaseClassUser> bassclassuserlist = db.BaseClassUser.ToList();
                List<object> list = new List<object>();
                if (classlist.Count > 0)
                {
                    List<User> UserList = db.User.Where(t => t.IsDel != true).ToList();
                    foreach (BaseClass bclass in classlist)
                    {//根据班次ID和流程ID获取班次人员信息,绑定CheckBoxlist
                        string name = "";
                        List<BaseClassUser> classuserlist = bassclassuserlist.Where(t => t.BaseClassID == bclass.ID && t.FlowID == flowid).ToList();
                        if (classuserlist.Count > 0)
                        {
                            foreach (BaseClassUser classuser in classuserlist)
                            {
                                name += UserList.FirstOrDefault(t => t.ID == classuser.UserID) == null ? "" : (UserList.FirstOrDefault(t => t.ID == classuser.UserID).RealName + ",");
                            }
                        }
                        name = name.TrimEnd(',').TrimStart(',');
                        list.Add(new
                        {
                            Name = bclass.Name + "(" + name + ")",
                            bclass.ID
                        });
                    }
                }
                if (list.Count > 0)
                {
                    chk.DataTextField = "Name";
                    chk.DataValueField = "ID";
                    chk.DataSource = list;
                    chk.DataBind();
                }

                List<TaskFlow> taskflowlist = db.TaskFlow.Where(t => t.TaskID == TaskID && t.FlowID == flowid).ToList();
                if (taskflowlist.Count > 0)
                {
                    //获取taskflow表的班次信息
                    var testlist = taskflowlist.Where(t => t.BaseClassID != null).Select(t => t.BaseClassID).Distinct().ToList();
                    for (int i = 0; i < testlist.Count; i++)
                    {
                        int classid = Convert.ToInt32(testlist[i].ToString());
                        for (int j = 0; j < chk.Items.Count; j++)
                        {
                            if (chk.Items[j].Value == classid.ToString())
                            {
                                chk.Items[j].Selected = true;
                            }
                        }
                        TaskFlow taskflow = taskflowlist.FirstOrDefault(t => t.BaseClassID == classid);
                        if (taskflow != null)
                        {
                            ltl_BeginDate.Text = taskflow.BeginDate.ToString("yyyy-MM-dd HH:mm:ss");
                            ltl_EndDate.Text = taskflow.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                            ltl_RemindDate.Text = taskflow.RemindDate.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    string name = "";
                    List<TaskFlow> flowlist = taskflowlist.Where(t => t.BaseClassID == null).ToList();
                    foreach (TaskFlow taskflow in flowlist)
                    {
                        name += db.User.FirstOrDefault(t => t.ID == taskflow.UserID && t.IsDel != true) == null ? "" : (db.User.FirstOrDefault(t => t.ID == taskflow.UserID && t.IsDel != true).RealName + ",");
                    }
                    ltl_SysUser.Text = name.TrimEnd(',').TrimStart(',');
                }

                Repeater rp_TableList = (Repeater)e.Item.FindControl("rp_TableList");
                List<object> lists = new List<object>();
                if (taskflowlist != null)
                {
                    foreach (TaskFlow tflist in taskflowlist)
                    {
                        IFMPLibrary.Entities.Table table = db.Table.FirstOrDefault(t => t.TaskID == TaskID && t.TableTypeID == tabtype && t.CreateUserID == tflist.UserID);
                        if (table != null)
                        {
                            lists.Add(new
                            {
                                table.ID,
                                CreateUser = table.CreateUserID,
                                UserName = db.User.FirstOrDefault(t => t.ID == table.CreateUserID).RealName,
                                table.CreateDate
                            });
                        }
                        else
                        {
                            lists.Add(new
                            {
                                ID = -2,
                                CreateUser = tflist.UserID,
                                UserName = db.User.FirstOrDefault(t => t.ID == tflist.UserID).RealName,
                                CreateDate = ""
                            });
                        }
                    }
                }
                if (lists.Count() > 0)
                {
                    rp_TableList.DataSource = lists.ToList();
                    rp_TableList.DataBind();
                }
            }
        }

        protected void rp_TableList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField hfFlowID = (HiddenField)e.Item.FindControl("hf_FID");
                int flowid = Convert.ToInt32(hfFlowID.Value);

                HiddenField hf_TableID = (HiddenField)e.Item.FindControl("hf_TableID");
                HiddenField hf_CreateUser = (HiddenField)e.Item.FindControl("hf_CreateUser");
                Repeater rp_ColList = (Repeater)e.Item.FindControl("rp_ColList");
                Repeater rp_AuditList = (Repeater)e.Item.FindControl("rp_AuditList");
                System.Web.UI.HtmlControls.HtmlTableRow trnull = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trnull");
                System.Web.UI.HtmlControls.HtmlTableRow tr_result = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr_result");
                System.Web.UI.HtmlControls.HtmlTableRow tr_message = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr_message");
                int tableid = Convert.ToInt32(hf_TableID.Value);
                int createuser = Convert.ToInt32(hf_CreateUser.Value);

                Flow flowmodel = db.Flow.FirstOrDefault(t => t.ID == flowid);
                if (flowmodel.IsAudit == true)//审核流程
                {
                    rp_ColList.Visible = false;
                    TaskFlow taskflowmodel = db.TaskFlow.FirstOrDefault(t => t.TaskID == TaskID && t.FlowID == flowid && t.UserID == createuser && t.ApplyType != ApplyTypeEnums.未交);

                    Literal ltl_AuditResult = (Literal)e.Item.FindControl("ltl_AuditResult");
                    Literal ltl_AuditMessage = (Literal)e.Item.FindControl("ltl_AuditMessage");

                    if (taskflowmodel != null)
                    {
                        tr_result.Visible = true;
                        tr_message.Visible = true;
                        ltl_AuditResult.Text = taskflowmodel.AuditResult.ToString();
                        ltl_AuditMessage.Text = taskflowmodel.AuditMessage;
                        trnull.Visible = false;
                    }
                    else
                    {
                        tr_result.Visible = false;
                        tr_message.Visible = false;
                        trnull.Visible = true;
                    }
                }
                else//填写流程
                {
                    var list = from tabledata in db.TableData
                               join tablecolumn in db.TableColumn on tabledata.TableColumnID equals tablecolumn.ID
                               //join dictionary in db.Dictionary on tablecolumn.DictionaryID equals dictionary.ID
                               where tabledata.TableID == tableid && tabledata.CreateUserID == createuser
                               orderby tablecolumn.Order
                               select new
                               {
                                   Data = db.Dictionary.FirstOrDefault(t => t.ID == tablecolumn.DictionaryID) == null ? tabledata.Data : db.Dictionary.FirstOrDefault(t => t.ID == tablecolumn.DictionaryID).DisplayType == DictionaryTypeEnums.单选 ? db.DictionaryData.FirstOrDefault(t => t.ID.ToString() == tabledata.Data).Data : tabledata.Data,
                                   tablecolumn.ColumnName,
                               };
                    if (list.Count() > 0)
                    {
                        trnull.Visible = false;
                    }
                    else
                    {
                        trnull.Visible = true;
                    }
                    rp_ColList.DataSource = list.ToList();
                    rp_ColList.DataBind();
                }
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                TaskFlow model = db.TaskFlow.FirstOrDefault(t => t.ID == TaskFlowID);
                if (model != null)
                {
                    if (DateTime.Now > model.EndDate)
                    {
                        model.ApplyType = ApplyTypeEnums.迟交;
                    }
                    else
                    {
                        model.ApplyType = ApplyTypeEnums.正常;
                    }

                    model.AuditMessage = this.txt_AuditMessage.Text.ToString();
                    model.AuditResult = (AuditResult)Convert.ToInt32(this.rdo_AuditResult.SelectedValue.ToString());
                    model.ApplyDate = DateTime.Now;
                    db.SaveChanges();

                    ShowMessage();
                    new SysLogDAO().AddLog(LogType.操作日志_其他, "审核" + model.AuditResult + "任务信息", UserID);
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