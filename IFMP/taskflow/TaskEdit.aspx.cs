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
using System.Text;
using System.Transactions;

namespace IFMP.taskflow
{
    public partial class TaskEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int TaskID
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


                if (TaskID != -1)
                {
                    InfoBind();
                }
            }
        }
        #endregion


        #region 前台js绑定数据
        private string MList(int flowid)
        {
            //List<BaseFlowRole> flowrole = db.BaseFlowRole.Where(t => t.FlowID == flowid).ToList();
            //这个可以综合查询的         
            List<UserRole> rolelist = db.UserRole.Where(t => db.BaseFlowRole.Where(f => f.FlowID == flowid).Select(f => f.RoleID).Contains(t.RoleID)).ToList();
            List<object> list = new List<object>();
            List<User> userlist = db.User.ToList();
            if (rolelist.Count > 0)
            {
                foreach (UserRole role in rolelist)
                {
                    list.Add(new
                    {
                        role.UserID,
                        RealName = userlist.FirstOrDefault(t => t.ID == role.UserID).RealName
                    });
                }
            }
            else
            {
                userlist = userlist.Where(t => t.IsDel != true && t.UserState != UserState.离职).ToList();
                foreach (User user in userlist)
                {
                    list.Add(new
                    {
                        UserID = user.ID,
                        user.RealName
                    });
                }
            }

            string name = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    dynamic temp = list[i];
                    name += "{\"id\":\"" + temp.UserID.ToString() +
                       "\",\"text\":\"" + temp.RealName.ToString() + "\"";
                    name += "},";
                }
            }
            else
            {
                name = "[]";
            }
            sb.Append(name.ToString().TrimEnd(','));
            return sb.ToString();
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            Task model = db.Task.FirstOrDefault(t => t.ID == TaskID);
            if (model != null)
            {
                this.ddl_ClassType.Enabled = false;
                this.ddl_TableType.Enabled = false;
                this.ddl_ClassType.SelectedValue = Convert.ToInt32(model.ClassType).ToString();
                this.ddl_TableType.SelectedValue = Convert.ToInt32(model.TableTypeID).ToString();
                this.txt_TaskName.Text = model.TaskName.ToString();
                this.rp_List.Visible = true;
                DataBindList();
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    CheckBoxList chk_ClassList;
                    HiddenField hf_FlowID;
                    TextBox txt_BeginDate;
                    TextBox txt_EndDate;
                    TextBox txt_RemindDate;
                    TextBox txt_SysUser;

                    Task task = db.Task.FirstOrDefault(t => t.ID == TaskID);
                    if (task == null)
                    {
                        task = new Task();
                        task.TaskName = this.txt_TaskName.Text.ToString();
                        task.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue.ToString());
                        task.TableTypeID = Convert.ToInt32(this.ddl_TableType.SelectedValue.ToString());
                        task.IsDel = false;
                        task.CreateDate = DateTime.Now;
                        task.CreateUserID = UserID;
                        if (db.Task.FirstOrDefault(t => t.TaskName == task.TaskName && t.IsDel != true) != null)
                        {
                            ShowMessage("任务名称已存在，请修改后重新提交");
                            return;
                        }

                        //string[] cid=classid.TrimEnd(',').Split(',');
                        //for (int i = 0; i < cid.Length; i++)
                        //{
                        //    task.BaseClassID = Convert.ToInt32(cid[i].ToString());
                        db.Task.Add(task);
                        db.SaveChanges();
                        //}

                        string classid = "";
                        for (int i = 0; i < this.cbl_Select.Items.Count; i++)
                        {
                            if (this.cbl_Select.Items[i].Selected)
                            {
                                classid += this.cbl_Select.Items[i].Value + ",";
                            }
                        }
                        if (classid == "")
                        {
                            ShowMessage("请至少选择一个基础班次");
                            ts.Dispose();
                            return;
                        }

                        for (int i = 0; i < this.rp_List.Items.Count; i++)
                        {
                            chk_ClassList = (CheckBoxList)rp_List.Items[i].FindControl("chk_ClassList");
                            hf_FlowID = (HiddenField)rp_List.Items[i].FindControl("hf_FlowID");
                            int flowid = Convert.ToInt32(hf_FlowID.Value.ToString());
                            txt_BeginDate = (TextBox)rp_List.Items[i].FindControl("txt_BeginDate");
                            txt_EndDate = (TextBox)rp_List.Items[i].FindControl("txt_EndDate");
                            txt_RemindDate = (TextBox)rp_List.Items[i].FindControl("txt_RemindDate");
                            txt_SysUser = (TextBox)rp_List.Items[i].FindControl("txt_SysUser");

                            TaskFlow taskflow = new TaskFlow();
                            taskflow.TaskID = task.ID;
                            taskflow.FlowID = flowid;
                            taskflow.ApplyType = ApplyTypeEnums.未交;
                            taskflow.IsReminded = false;
                            if (txt_BeginDate.Text.ToString() == "" || txt_EndDate.Text.ToString() == "" || txt_RemindDate.Text.ToString() == "")
                            {
                                ShowMessage("时间设置有误，请手动修改时间或在【基础时间设置】模块设置好后重新提交");
                                ts.Dispose();
                                return;
                            }
                            taskflow.BeginDate = Convert.ToDateTime(txt_BeginDate.Text.ToString());
                            taskflow.EndDate = Convert.ToDateTime(txt_EndDate.Text.ToString());
                            taskflow.RemindDate = Convert.ToDateTime(txt_RemindDate.Text.ToString());
                            for (int j = 0; j < chk_ClassList.Items.Count; j++)
                            {
                                if (chk_ClassList.Items[j].Selected)
                                {
                                    int baseclassid = Convert.ToInt32(chk_ClassList.Items[j].Value.ToString());
                                    List<BaseClassUser> classuserlist = db.BaseClassUser.Where(t => t.BaseClassID == baseclassid && t.FlowID == flowid).ToList();
                                    if (classuserlist.Count > 0)
                                    {
                                        foreach (BaseClassUser classuser in classuserlist)
                                        {
                                            taskflow.UserID = classuser.UserID;
                                            taskflow.BaseClassID = baseclassid;

                                            db.TaskFlow.Add(taskflow);
                                            db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        Flow flow = db.Flow.FirstOrDefault(t => t.ID == flowid);
                                        BaseClass baseclass = db.BaseClass.FirstOrDefault(t => t.ID == baseclassid);
                                        ShowMessage("流程：【" + flow.Name + "】班次：【" + baseclass.Name + "】人员未设置,请在【基础班次设置】模块设置好后重新提交");
                                        ts.Dispose();
                                        return;
                                    }
                                }
                            }
                            if (txt_SysUser.Text != "")
                            {
                                foreach (string id in txt_SysUser.Text.Split(','))
                                {
                                    int uid = Convert.ToInt32(id);
                                    taskflow.UserID = uid;
                                    //taskflow.BaseClassID = -1;
                                    db.TaskFlow.Add(taskflow);
                                    db.SaveChanges();
                                }
                            }
                        }
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "成功添加任务信息", UserID);
                    }
                    else
                    {
                        //string classid = "";
                        task.TaskName = this.txt_TaskName.Text.ToString();
                        //for (int i = 0; i < this.cbl_Select.Items.Count; i++)
                        //{
                        //    if (this.cbl_Select.Items[i].Selected)
                        //    {
                        //        classid += this.cbl_Select.Items[i].Value + ",";
                        //    }
                        //}
                        //task.BaseClassID = Convert.ToInt32(classid);
                        task.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue.ToString());
                        task.TableTypeID = Convert.ToInt32(this.ddl_TableType.SelectedValue.ToString());

                        if (db.Task.FirstOrDefault(t => t.TaskName == task.TaskName && t.IsDel != true && t.ID != TaskID) != null)
                        {
                            ShowMessage("任务名称已存在，请修改后重新提交");
                            return;
                        }

                        db.SaveChanges();

                        //删除原有taskflow数据
                        db.TaskFlow.RemoveRange(db.TaskFlow.Where(t => t.TaskID == TaskID));
                        for (int i = 0; i < this.rp_List.Items.Count; i++)
                        {
                            chk_ClassList = (CheckBoxList)rp_List.Items[i].FindControl("chk_ClassList");
                            hf_FlowID = (HiddenField)rp_List.Items[i].FindControl("hf_FlowID");
                            int flowid = Convert.ToInt32(hf_FlowID.Value.ToString());
                            txt_BeginDate = (TextBox)rp_List.Items[i].FindControl("txt_BeginDate");
                            txt_EndDate = (TextBox)rp_List.Items[i].FindControl("txt_EndDate");
                            txt_RemindDate = (TextBox)rp_List.Items[i].FindControl("txt_RemindDate");
                            txt_SysUser = (TextBox)rp_List.Items[i].FindControl("txt_SysUser");

                            TaskFlow taskflow = new TaskFlow();
                            taskflow.TaskID = task.ID;
                            taskflow.FlowID = flowid;
                            taskflow.ApplyType = ApplyTypeEnums.未交;
                            taskflow.IsReminded = false;
                            taskflow.BeginDate = Convert.ToDateTime(txt_BeginDate.Text.ToString());
                            taskflow.EndDate = Convert.ToDateTime(txt_EndDate.Text.ToString());
                            taskflow.RemindDate = Convert.ToDateTime(txt_RemindDate.Text.ToString());
                            for (int j = 0; j < chk_ClassList.Items.Count; j++)
                            {
                                if (chk_ClassList.Items[j].Selected)
                                {
                                    int baseclassid = Convert.ToInt32(chk_ClassList.Items[j].Value.ToString());
                                    List<BaseClassUser> classuserlist = db.BaseClassUser.Where(t => t.BaseClassID == baseclassid && t.FlowID == flowid).ToList();
                                    if (classuserlist.Count > 0)
                                    {
                                        foreach (BaseClassUser classuser in classuserlist)
                                        {
                                            taskflow.UserID = classuser.UserID;
                                            taskflow.BaseClassID = baseclassid;

                                            db.TaskFlow.Add(taskflow);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }
                            if (txt_SysUser.Text != "")
                            {
                                foreach (string id in txt_SysUser.Text.Split(','))
                                {
                                    int uid = Convert.ToInt32(id);
                                    taskflow.UserID = uid;
                                    db.TaskFlow.Add(taskflow);
                                    db.SaveChanges();
                                }
                            }
                        }

                        new SysLogDAO().AddLog(LogType.操作日志_修改, "成功修改任务信息", UserID);
                    }
                    ts.Complete();
                    ShowMessage();

                }
                catch (Exception ex)
                {
                    ShowMessage("提交失败请检查填写数据");
                    ts.Dispose();
                    new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                }
            }


        }
        #endregion


        #region 下拉框绑定事件
        protected void ddl_TableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rp_List.Visible = true;
            DataBindList();
        }
        protected void ddl_ClassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.ddl_TableType.SelectedValue) != -2)
            {
                DataBindList();
            }
        }
        #endregion


        #region 流程信息绑定
        private void DataBindList()
        {
            int tabtype = Convert.ToInt32(this.ddl_TableType.SelectedValue.ToString());
            List<Flow> flowlist = db.Flow.Where(t => t.TableTypeID == tabtype).OrderBy(t => t.ParentID).ToList();
            if (flowlist.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = flowlist;
            this.rp_List.DataBind();

            this.trnull.Visible = true;
            int clatype = Convert.ToInt32(this.ddl_ClassType.SelectedValue.ToString());//班次类型
            List<BaseClass> classlist = db.BaseClass.Where(t => t.ClassType == (ClassTypeEnums)clatype && t.IsDel != true).ToList();
            if (classlist.Count > 0)
            {
                cbl_Select.DataTextField = "Name";
                cbl_Select.DataValueField = "ID";
                cbl_Select.DataSource = classlist;
                cbl_Select.DataBind();
            }

            // List<TaskFlow> taskflowlist = db.TaskFlow.Where(t => t.TaskID == TaskID).ToList();
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
                int clatype = Convert.ToInt32(this.ddl_ClassType.SelectedValue.ToString());//班次类型
                int tabtype = Convert.ToInt32(this.ddl_TableType.SelectedValue.ToString());//表单类型
                HiddenField hfFlowID = (HiddenField)e.Item.FindControl("hf_FlowID");
                int flowid = Convert.ToInt32(hfFlowID.Value);
                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chk_ClassList");
                TextBox txt_SysUser = (TextBox)e.Item.FindControl("txt_SysUser");
                Literal ltl_Content = (Literal)e.Item.FindControl("ltl_Content");
                TextBox txt_BeginDate = (TextBox)e.Item.FindControl("txt_BeginDate");
                TextBox txt_EndDate = (TextBox)e.Item.FindControl("txt_EndDate");
                TextBox txt_RemindDate = (TextBox)e.Item.FindControl("txt_RemindDate");

                //根据班次类型获取所有班次信息
                List<BaseClass> classlist = db.BaseClass.Where(t => t.ClassType == (ClassTypeEnums)clatype && t.IsDel != true).ToList();
                List<BaseClassUser> bassclassuserlist = db.BaseClassUser.ToList();
                List<object> list = new List<object>();
                if (classlist.Count > 0)
                {
                    List<User> UserList = db.User.Where(t => t.IsDel != true).ToList();
                    foreach (BaseClass bclass in classlist)
                    {
                        //根据班次ID和流程ID获取班次人员信息,绑定CheckBoxlist
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

                StringBuilder sb = new StringBuilder("");
                string a = MList(flowid);
                sb.Append("<script type='text/javascript'>");
                sb.Append(" $(function () {");
                sb.Append(" $('#" + txt_SysUser.ClientID + "').combotree({");
                sb.Append(" data: [ ");
                sb.Append(a);
                sb.Append("],");
                sb.Append("multiple: true,");
                sb.Append("multiline: true,");
                sb.Append("onlyLeafCheck:'true',");
                sb.Append("lines: true,");
                sb.Append("});");
                sb.Append(" }); </script>");
                ltl_Content.Text = sb.ToString();

                //添加绑定
                if (TaskID == -1)
                {
                    BaseDateFlow dateflow = db.BaseDateFlow.FirstOrDefault(t => t.FlowID == flowid && t.ClassType == (ClassTypeEnums)clatype && t.TableTypeID == tabtype);
                    if (dateflow != null)
                    {
                        txt_BeginDate.Text = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(1), dateflow.BeginDate).ToString("yyyy-MM-dd HH:mm:ss");
                        txt_EndDate.Text = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(1), dateflow.EndDate).ToString("yyyy-MM-dd HH:mm:ss");
                        txt_RemindDate.Text = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(1), dateflow.RemindDate).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else
                {//编辑绑定
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
                                //txt_BeginDate.Text = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(1), taskflow.BeginDate).ToString("yyyy-MM-dd HH:mm:ss");
                                //txt_EndDate.Text = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(1), taskflow.EndDate).ToString("yyyy-MM-dd HH:mm:ss");
                                //txt_RemindDate.Text = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(1), taskflow.RemindDate).ToString("yyyy-MM-dd HH:mm:ss");

                                txt_BeginDate.Text = taskflow.BeginDate.ToString("yyyy-MM-dd HH:mm:ss");
                                txt_EndDate.Text = taskflow.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                                txt_RemindDate.Text = taskflow.RemindDate.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        string ids = "";
                        List<TaskFlow> flowlist = taskflowlist.Where(t => t.BaseClassID == null).ToList();
                        foreach (TaskFlow taskflow in flowlist)
                        {
                            ids += taskflow.UserID + ",";
                        }
                        txt_SysUser.Text = ids.TrimEnd(',').TrimStart(',');
                    }
                }
            }
        }
        #endregion


        #region 复选框选择事件
        protected void cbl_Select_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rp_List.Items.Count > 0)
            {
                CheckBoxList chk_ClassList;
                for (int i = 0; i < this.rp_List.Items.Count; i++)//循环repeater数据
                {
                    chk_ClassList = (CheckBoxList)rp_List.Items[i].FindControl("chk_ClassList");
                    for (int j = 0; j < chk_ClassList.Items.Count; j++)
                    {
                        for (int m = 0; m < this.cbl_Select.Items.Count; m++)//循环班次
                        {
                            if (chk_ClassList.Items[j].Value == cbl_Select.Items[m].Value && cbl_Select.Items[m].Selected == true)
                            {
                                chk_ClassList.Items[j].Selected = true;
                                break;
                            }
                            if (chk_ClassList.Items[j].Value == cbl_Select.Items[m].Value && cbl_Select.Items[m].Selected == false)
                            {
                                chk_ClassList.Items[j].Selected = false;
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}