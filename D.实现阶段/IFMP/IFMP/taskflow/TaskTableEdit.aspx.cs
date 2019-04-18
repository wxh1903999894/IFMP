/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月27日 11时29分19秒
** 描    述:      字典信息管理页面
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
using System.Transactions;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Utils;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;

namespace IFMP.taskflow
{
    public partial class TaskTableEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        /// <summary>
        /// 表单类型
        /// </summary>
        public int TType
        {
            get
            {
                return GetQueryString<int>("tabletype", -1);
            }
        }
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

        public int TaskFlowID
        {
            get
            {
                return GetQueryString<int>("taskflowid", -1);
            }
        }
        /// <summary>
        /// 流程ID
        /// </summary>
        public int FlowID
        {
            get
            {
                return GetQueryString<int>("flowid", -1);
            }
        }

        /// <summary>
        /// 1:表单预览
        /// </summary>
        public int Flag
        {
            get
            {
                return GetQueryString<int>("flag", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Flag == 1)
                {
                    this.trvis.Visible = false;
                }
                this.ltl_TableType.Text = db.TableType.FirstOrDefault(t => t.ID == TType).Name;
                DataBindList();
            }
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            List<TableColumn> columnlist = db.TableColumn.Where(t => t.TableTypeID == TType).OrderBy(t => t.Order).ThenBy(t => t.ID).ToList();
            List<object> list = new List<object>();
            List<Dictionary> DictionaryList = db.Dictionary.Where(t => t.IsDel != true).ToList();
            if (columnlist.Count > 0)
            {
                foreach (TableColumn column in columnlist)
                {
                    list.Add(new
                    {
                        column.ID,
                        TableType = column.TableTypeID,
                        column.ColumnName,
                        column.DictionaryID,
                        DType = DictionaryList.FirstOrDefault(t => t.ID == column.DictionaryID) == null ? -2 : (int)DictionaryList.FirstOrDefault(t => t.ID == column.DictionaryID).DisplayType,
                        column.HintDictionaryID,
                        RegexData = DictionaryList.FirstOrDefault(t => t.ID == column.HintDictionaryID) == null ? "" : DictionaryList.FirstOrDefault(t => t.ID == column.HintDictionaryID).RegexData,
                        RegexType = DictionaryList.FirstOrDefault(t => t.ID == column.HintDictionaryID && t.DisplayType == DictionaryTypeEnums.填写) == null ? -2 : (int)DictionaryList.FirstOrDefault(t => t.ID == column.HintDictionaryID).RegexType.Value,
                        column.DefaultData,
                        column.Order,
                        column.IsStats
                    });
                }

                this.rp_List.DataSource = list;
                this.rp_List.DataBind();
            }
        }
        #endregion


        #region 数据绑定事件
        protected void rp_List_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                TextBox textName = (TextBox)e.Item.FindControl("txt_TextName");
                RadioButtonList rdoName = (RadioButtonList)e.Item.FindControl("rdo_Name");
                HiddenField DictionaryID = (HiddenField)e.Item.FindControl("hf_DictionaryID");
                HiddenField DType = (HiddenField)e.Item.FindControl("hf_DType");
                HiddenField hf_HintDictionaryID = (HiddenField)e.Item.FindControl("hf_HintDictionaryID");
                //HiddenField DefaultData = (HiddenField)e.Item.FindControl("hf_DefaultData");

                Literal ltl_RegexData = (Literal)e.Item.FindControl("ltl_RegexData");
                int dType = Convert.ToInt32(DType.Value);
                int TableColumnID = Convert.ToInt32(((HiddenField)e.Item.FindControl("hf_TableColumnID")).Value);
                TableColumn TableColumn = db.TableColumn.FirstOrDefault(t => t.ID == TableColumnID);
                if ((DictionaryTypeEnums)dType == DictionaryTypeEnums.单选)
                {
                    rdoName.Visible = true;
                    textName.Visible = false;
                    //DefaultData.Value = TableColumn.DefaultData.ToString();
                }
                else
                {
                    rdoName.Visible = false;
                    textName.Visible = true;
                    textName.Text = TableColumn.DefaultData;
                }

                int hintdictID = 0;
                if (!string.IsNullOrEmpty(hf_HintDictionaryID.Value) && int.TryParse(hf_HintDictionaryID.Value, out hintdictID))
                {
                    Dictionary hintdiction = db.Dictionary.FirstOrDefault(t => t.ID == hintdictID);
                    if (hintdiction != null)
                    {
                        if (hintdiction.RegexData != "" && hintdiction.DisplayType == DictionaryTypeEnums.填写)
                        {
                            ltl_RegexData.Visible = true;
                            ltl_RegexData.Text = "数据范围为：" + hintdiction.RegexData.Replace("|", "-");
                        }
                    }
                }

                int dictid = 0;
                if (!string.IsNullOrEmpty(DictionaryID.Value) && int.TryParse(DictionaryID.Value, out dictid))
                {
                    if ((DictionaryTypeEnums)dType == DictionaryTypeEnums.单选 && rdoName != null)//单选选项内容需在字典中设置，字典ID不会为null
                    {
                        List<DictionaryData> datalist = db.DictionaryData.Where(t => t.DictionaryID == dictid).ToList();
                        if (datalist.Count > 0)
                        {
                            rdoName.DataSource = datalist;
                            rdoName.DataValueField = "ID";
                            rdoName.DataTextField = "Data";
                            rdoName.DataBind();
                            //判断是否有默认值，有的话取第一个
                            string SelDefaultData = TableColumn.DefaultData;
                            if (!string.IsNullOrEmpty(SelDefaultData))
                            {
                                string FirstData = SelDefaultData.Split('|')[0];

                                rdoName.SelectedValue = datalist.FirstOrDefault(t => t.Data == FirstData).ID.ToString();
                            }
                            else
                            {

                                rdoName.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        //先看一下时候是否在日期内
                        TaskFlow TaskFlow = db.TaskFlow.FirstOrDefault(t => t.ID == TaskFlowID);
                        //if (TaskFlow == null || DateTime.Now < TaskFlow.BeginDate || DateTime.Now > TaskFlow.EndDate)
                        if (TaskFlow == null || DateTime.Now < TaskFlow.BeginDate)
                        {
                            ShowMessage("不在规定的时间内，无法操作!");
                            return;
                        }

                        if (this.rp_List.Items.Count > 0)
                        {
                            IFMPLibrary.Entities.Table model = new IFMPLibrary.Entities.Table();
                            model.TaskID = TaskID;
                            model.TableTypeID = TType;
                            model.CreateDate = DateTime.Now;
                            model.CreateUserID = UserID;
                            model.FlowID = FlowID;
                            db.Table.Add(model);
                            db.SaveChanges();

                            TextBox txt_TextName;
                            RadioButtonList rdo_Name;
                            HiddenField hf_DictionaryID;
                            HiddenField hf_HintDictionaryID;
                            HiddenField hf_DType;
                            HiddenField hf_TableColumnID;
                            Literal ltl_RegexData;

                            //设置报警的审核人
                            List<User> UserList = db.User.Where(t => t.IsDel != true
                                 && t.UserState != UserState.离职
                                 && db.TaskFlow.Where(m => m.TaskID == TaskID && db.Flow.Where(l => l.IsAudit == true && l.TableTypeID == TType).Select(l => l.ID).Contains(m.FlowID)).Select(m => m.UserID).Contains(t.ID)).Distinct().ToList();

                            List<TableColumn> TableColumnList = db.TableColumn.Where(t => t.TableTypeID == TType).ToList();
                            TableType TableType = db.TableType.FirstOrDefault(t => t.ID == TType);
                            BaseClass BaseClass = db.BaseClass.FirstOrDefault(t => t.ID == BaseClassID);
                            User User = db.User.FirstOrDefault(t => t.ID == UserID);
                            List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == TType).ToList();
                            List<TaskFlow> TaskFlowList = db.TaskFlow.Where(t => t.TaskID == TaskID).ToList();


                            for (int i = 0; i < this.rp_List.Items.Count; i++)
                            {
                                TableData tabledata = new TableData();

                                hf_DType = (HiddenField)rp_List.Items[i].FindControl("hf_DType");
                                int displaytype = Convert.ToInt32(hf_DType.Value);
                                hf_TableColumnID = (HiddenField)rp_List.Items[i].FindControl("hf_TableColumnID");
                                int columnid = Convert.ToInt32(hf_TableColumnID.Value);
                                ltl_RegexData = (Literal)rp_List.Items[i].FindControl("ltl_RegexData");


                                TableColumn tablecolumn = TableColumnList.FirstOrDefault(t => t.ID == columnid);

                                if ((DictionaryTypeEnums)displaytype == DictionaryTypeEnums.单选)
                                {
                                    rdo_Name = (RadioButtonList)rp_List.Items[i].FindControl("rdo_Name");
                                    tabledata.Data = rdo_Name.SelectedValue.ToString();
                                    //这里也要加一个验证                                 
                                    hf_HintDictionaryID = (HiddenField)rp_List.Items[i].FindControl("hf_HintDictionaryID");
                                    if (!string.IsNullOrEmpty(hf_HintDictionaryID.Value))
                                    {
                                        int hintdictID = Convert.ToInt32(hf_HintDictionaryID.Value);
                                        int selid = Convert.ToInt32(tabledata.Data);
                                        string defaultdata = db.TableColumn.FirstOrDefault(t => t.ID == columnid).DefaultData;
                                        string seldata = db.DictionaryData.FirstOrDefault(t => t.ID == selid).Data;
                                        if (defaultdata.Length > 0)
                                        {
                                            if (!defaultdata.Split('|').Contains(seldata))
                                            {
                                                tabledata.IsAlert = true;
                                                if (UserList.Count > 0)
                                                {
                                                    foreach (User NoticeUser in UserList)
                                                    {
                                                        new NoticeDAO().SendDDNotice(
                                                        NoticeUser.DDID,
                                                        new NoticeDAO().GetUrl(Request.Url.AbsoluteUri.ToString(), TaskID, TaskFlowList.FirstOrDefault(t => t.UserID == NoticeUser.ID).FlowID),
                                                        Request.Url.AbsoluteUri.ToString(),
                                                        "报警",
                                                        tablecolumn.ColumnName + "存在异常",
                                                        "请及时去现场查看情况",
                                                        new NoticeDAO().BuildFormList(TableType.Name, tablecolumn.ColumnName, tabledata.Data, BaseClass.Name, User.RealName));
                                                    }
                                                }
                                            }
                                        }
                                        //List<DictionaryData> DictionaryDataList = db.DictionaryData.Where(t => t.DictionaryID == hintdictID).ToList();
                                    }
                                }
                                else
                                {
                                    txt_TextName = (TextBox)rp_List.Items[i].FindControl("txt_TextName");
                                    tabledata.Data = txt_TextName.Text.ToString();
                                    if (string.IsNullOrEmpty(tabledata.Data))
                                    {
                                        ShowMessage("请确认已填写所有的数据");
                                        ts.Dispose();
                                        return;
                                    }
                                    //验证合法性
                                    hf_DictionaryID = (HiddenField)rp_List.Items[i].FindControl("hf_DictionaryID");
                                    if (!string.IsNullOrEmpty(hf_DictionaryID.Value))
                                    {
                                        int dictID = Convert.ToInt32(hf_DictionaryID.Value);
                                        Dictionary diction = db.Dictionary.FirstOrDefault(t => t.ID == dictID);
                                        if (diction != null)
                                        {
                                            if (new BaseUtils().GetRegex(tabledata.Data, diction.RegexType.Value) == false)
                                            {
                                                ShowMessage("【" + tabledata.Data + "】数据填写错误，请填写【" + diction.RegexType + "】");
                                                ts.Dispose();
                                                return;
                                            }
                                        }
                                    }

                                    hf_HintDictionaryID = (HiddenField)rp_List.Items[i].FindControl("hf_HintDictionaryID");
                                    if (!string.IsNullOrEmpty(hf_HintDictionaryID.Value))
                                    {
                                        int hintdictID = Convert.ToInt32(hf_HintDictionaryID.Value);
                                        Dictionary hintdiction = db.Dictionary.FirstOrDefault(t => t.ID == hintdictID);

                                        if (hintdiction != null)
                                        {
                                            if (hintdiction.RegexType == RegexType.有范围的数字 || hintdiction.RegexType == RegexType.有范围的度数)
                                            {
                                                if (new BaseUtils().GetRegex(tabledata.Data.Split('°')[0], hintdiction.RegexType.Value, hintdiction.RegexData) == false)
                                                {
                                                    tabledata.IsAlert = true;
                                                    if (UserList.Count > 0)
                                                    {
                                                        foreach (User NoticeUser in UserList)
                                                        {
                                                            new NoticeDAO().SendDDNotice(
                                                            NoticeUser.DDID,
                                                            new NoticeDAO().GetUrl(Request.Url.AbsoluteUri.ToString(), TaskID, TaskFlowList.FirstOrDefault(t => t.UserID == NoticeUser.ID).FlowID),
                                                            Request.Url.AbsoluteUri.ToString(),
                                                            "报警",
                                                            tablecolumn.ColumnName + "存在异常",
                                                            "请及时去现场查看情况",
                                                            new NoticeDAO().BuildFormList(TableType.Name, tablecolumn.ColumnName, tabledata.Data, BaseClass.Name, User.RealName));
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (new BaseUtils().GetRegex(tabledata.Data, hintdiction.RegexType.Value) == false)
                                                {
                                                    tabledata.IsAlert = true;
                                                    if (UserList.Count > 0)
                                                    {
                                                        foreach (User NoticeUser in UserList)
                                                        {
                                                            new NoticeDAO().SendDDNotice(
                                                            NoticeUser.DDID,
                                                            new NoticeDAO().GetUrl(Request.Url.AbsoluteUri.ToString(), TaskID, TaskFlowList.FirstOrDefault(t => t.UserID == NoticeUser.ID).FlowID),
                                                            Request.Url.AbsoluteUri.ToString(),
                                                            "报警",
                                                            tablecolumn.ColumnName + "存在异常",
                                                            "请及时去现场查看情况",
                                                            new NoticeDAO().BuildFormList(TableType.Name, tablecolumn.ColumnName, tabledata.Data, BaseClass.Name, User.RealName));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                tabledata.TableID = model.ID;
                                tabledata.TableColumnID = columnid;
                                tabledata.CreateDate = DateTime.Now;
                                tabledata.CreateUserID = UserID;

                                db.TableData.Add(tabledata);
                                db.SaveChanges();
                            }

                            //添加可能存在的不填写内容
                            List<TableColumn> NotFillColumnList = db.TableColumn.Where(t => t.IsFill == false && t.TableTypeID == model.TableTypeID).ToList();
                            foreach (TableColumn NotFillColumn in NotFillColumnList)
                            {
                                TableData tabledata = new TableData();
                                tabledata.CreateDate = DateTime.Now;
                                tabledata.CreateUserID = UserID;
                                tabledata.TableColumnID = NotFillColumn.ID;
                                tabledata.TableID = model.ID;
                                //获取数值和合法性
                                tabledata.Data = new ColumnDAO().GetData(model.ID, NotFillColumn).ToString();
                                //需要报警
                                tabledata.IsAlert = false;
                                if (NotFillColumn.HintDictionaryID != null)
                                {
                                    Dictionary hintdiction = db.Dictionary.FirstOrDefault(t => t.ID == NotFillColumn.HintDictionaryID);

                                    if (hintdiction != null)
                                    {
                                        if (hintdiction.RegexType == RegexType.有范围的数字 || hintdiction.RegexType == RegexType.有范围的度数)
                                        {
                                            if (new BaseUtils().GetRegex(tabledata.Data.Split('°')[0], hintdiction.RegexType.Value, hintdiction.RegexData) == false)
                                            {
                                                tabledata.IsAlert = true;
                                                if (UserList.Count > 0)
                                                {
                                                    foreach (User NoticeUser in UserList)
                                                    {
                                                        new NoticeDAO().SendDDNotice(
                                                        NoticeUser.DDID,
                                                        new NoticeDAO().GetUrl(Request.Url.AbsoluteUri.ToString(), TaskID, TaskFlowList.FirstOrDefault(t => t.UserID == NoticeUser.ID).FlowID),
                                                        Request.Url.AbsoluteUri.ToString(),
                                                        "报警",
                                                        NotFillColumn.ColumnName + "存在异常",
                                                        "请及时去现场查看情况",
                                                        new NoticeDAO().BuildFormList(TableType.Name, NotFillColumn.ColumnName, tabledata.Data, BaseClass.Name, User.RealName));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (new BaseUtils().GetRegex(tabledata.Data, hintdiction.RegexType.Value) == false)
                                            {
                                                tabledata.IsAlert = true;
                                                if (UserList.Count > 0)
                                                {
                                                    foreach (User NoticeUser in UserList)
                                                    {
                                                        new NoticeDAO().SendDDNotice(
                                                        NoticeUser.DDID,
                                                        new NoticeDAO().GetUrl(Request.Url.AbsoluteUri.ToString(), TaskID, TaskFlowList.FirstOrDefault(t => t.UserID == NoticeUser.ID).FlowID),
                                                        Request.Url.AbsoluteUri.ToString(),
                                                        "报警",
                                                        NotFillColumn.ColumnName + "存在异常",
                                                        "请及时去现场查看情况",
                                                        new NoticeDAO().BuildFormList(TableType.Name, NotFillColumn.ColumnName, tabledata.Data, BaseClass.Name, User.RealName));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                db.TableData.Add(tabledata);
                                db.SaveChanges();
                            }

                            #region 更新taskflow表信息
                            TaskFlow taskflow = db.TaskFlow.FirstOrDefault(t => t.ID == TaskFlowID && t.FlowID == FlowID && t.UserID == UserID);
                            if (taskflow != null)
                            {
                                taskflow.ApplyDate = DateTime.Now;
                                if (DateTime.Now > taskflow.EndDate)
                                {
                                    taskflow.ApplyType = ApplyTypeEnums.迟交;
                                }
                                else
                                {
                                    taskflow.ApplyType = ApplyTypeEnums.正常;
                                }
                            }
                            db.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            ShowMessage("暂无表单信息，无需提交");
                            ts.Dispose();
                            return;
                        }
                        ShowMessage();
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "成功填写【" + this.ltl_TableType.Text + "】表单信息", UserID);
                        ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("提交失败：" + ex.Message);
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "填写【" + this.ltl_TableType.Text + "】表单信息时出错", UserID);
                        ts.Dispose();
                        return;
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
    }
}