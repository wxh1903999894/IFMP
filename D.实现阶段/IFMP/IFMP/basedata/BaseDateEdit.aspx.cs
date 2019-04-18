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


namespace IFMP.basedata
{
    public partial class BaseDateEdit : PageBase
    {
        #region 参数集合
        public int BaseDateFlowID
        {
            get
            {
                return GetQueryString<int>("id", 0);
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {

                    CommonFunction.BindEnum<ClassTypeEnums>(this.ddl_ClassType, "-99");
                    List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();
                    this.ddl_TableType.DataSource = TableTypeList;
                    this.ddl_TableType.DataValueField = "ID";
                    this.ddl_TableType.DataTextField = "Name";
                    this.ddl_TableType.DataBind();

                    if (BaseDateFlowID != 0)
                    {
                        BindInfo();
                    }

                    int TableType = Convert.ToInt32(this.ddl_TableType.SelectedValue);
                    List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == TableType).OrderBy(t => t.ParentID).ThenBy(t => t.ID).ToList();
                    this.ddl_Flow.DataSource = FlowList;
                    this.ddl_Flow.DataValueField = "ID";
                    this.ddl_Flow.DataTextField = "Name";
                    this.ddl_Flow.DataBind();
                }
            }
        }

        protected void ddl_TableTypeChanged(object sender, EventArgs e)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                int TableType = Convert.ToInt32(this.ddl_TableType.SelectedValue);
                List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == TableType).OrderBy(t => t.ParentID).ThenBy(t => t.ID).ToList();
                this.ddl_Flow.DataSource = FlowList;
                this.ddl_Flow.DataValueField = "ID";
                this.ddl_Flow.DataTextField = "Name";
                this.ddl_Flow.DataBind();
            }
        }


        public void BindInfo()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                BaseDateFlow BaseDateFlow = db.BaseDateFlow.FirstOrDefault(t => t.ID == BaseDateFlowID);

                if (BaseDateFlow != null)
                {
                    this.ddl_TableType.SelectedValue = BaseDateFlow.TableTypeID.ToString();
                    this.ddl_ClassType.SelectedValue = BaseDateFlow.ClassType.ToString();
                    this.ddl_Flow.SelectedValue = BaseDateFlow.FlowID.ToString();
                    this.txt_BeginDate.Text = BaseDateFlow.BeginDate.ToString("HH:mm:ss");
                    this.txt_EndDate.Text = BaseDateFlow.EndDate.ToString("HH:mm:ss");
                    this.txt_RemindDate.Text = BaseDateFlow.RemindDate.ToString("HH:mm:ss");
                }
            }

        }


        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    BaseDateFlow BaseDateFlow = db.BaseDateFlow.FirstOrDefault(t => t.ID == BaseDateFlowID);

                    if (BaseDateFlow == null)
                    {
                        BaseDateFlow = new BaseDateFlow();
                        BaseDateFlow.BeginDate = Convert.ToDateTime(this.txt_BeginDate.Text);
                        BaseDateFlow.EndDate = Convert.ToDateTime(this.txt_EndDate.Text);
                        BaseDateFlow.RemindDate = Convert.ToDateTime(this.txt_RemindDate.Text);

                        if (BaseDateFlow.BeginDate > BaseDateFlow.EndDate || BaseDateFlow.BeginDate > BaseDateFlow.RemindDate || BaseDateFlow.RemindDate > BaseDateFlow.EndDate)
                        {
                            ShowMessage("请至少选择正确的时间(提醒时间需要晚于开始时间，结束时间需要晚于提醒时间)");
                            return;
                        }

                        BaseDateFlow.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue);
                        BaseDateFlow.TableTypeID = Convert.ToInt32(this.ddl_TableType.SelectedValue);
                        BaseDateFlow.FlowID = Convert.ToInt32(this.ddl_Flow.SelectedValue);
                        BaseDateFlow.Name = db.Flow.FirstOrDefault(t => t.ID == BaseDateFlow.FlowID).Name + ":" + Enum.GetName(typeof(ClassTypeEnums), BaseDateFlow.ClassType);

                        //若存在重复的提示,保证同表单同班次只有一个时间
                        if (db.BaseDateFlow.FirstOrDefault(t => t.FlowID == BaseDateFlow.FlowID && t.ClassType == BaseDateFlow.ClassType && t.TableTypeID == BaseDateFlow.TableTypeID) != null)
                        {
                            ShowMessage("该班次时段已被选择，请在基础时间设置内选择修改");
                            return;
                        }

                        db.BaseDateFlow.Add(BaseDateFlow);
                        ShowMessage();
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "添加基础时间设置", UserID);
                        db.SaveChanges();
                    }
                    else
                    {
                        BaseDateFlow.BeginDate = Convert.ToDateTime(this.txt_BeginDate.Text);
                        BaseDateFlow.EndDate = Convert.ToDateTime(this.txt_EndDate.Text);
                        BaseDateFlow.RemindDate = Convert.ToDateTime(this.txt_RemindDate.Text);

                        if (BaseDateFlow.BeginDate > BaseDateFlow.EndDate || BaseDateFlow.BeginDate > BaseDateFlow.RemindDate || BaseDateFlow.RemindDate > BaseDateFlow.EndDate)
                        {
                            ShowMessage("请至少选择正确的时间(提醒时间需要晚于开始时间，结束时间需要晚于提醒时间)");
                            return;
                        }
                        BaseDateFlow.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue);
                        BaseDateFlow.TableTypeID = Convert.ToInt32(this.ddl_TableType.SelectedValue);
                        BaseDateFlow.FlowID = Convert.ToInt32(this.ddl_Flow.SelectedValue);
                        BaseDateFlow.Name = db.Flow.FirstOrDefault(t => t.ID == BaseDateFlow.FlowID).Name + ":" + Enum.GetName(typeof(ClassTypeEnums), BaseDateFlow.ClassType);

                        //若存在重复的提示,保证同表单同班次只有一个时间
                        if (db.BaseDateFlow.FirstOrDefault(t => t.FlowID == BaseDateFlow.FlowID && t.ClassType == BaseDateFlow.ClassType && t.TableTypeID == BaseDateFlow.TableTypeID && t.ID != BaseDateFlow.ID) != null)
                        {
                            ShowMessage("该班次时段已被选择，请在基础时间设置内选择修改");
                            return;
                        }
                    }

                    ShowMessage();
                    new SysLogDAO().AddLog(LogType.操作日志_修改, "修改基础时间设置", UserID);
                    db.SaveChanges();
                }
            }
            catch (Exception error)
            {

                ShowMessage(error.Message);
            }
        }
    }
}