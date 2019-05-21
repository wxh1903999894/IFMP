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
    public partial class BaseFlowRoleEdit : PageBase
    {
        #region 参数集合
        public int BaseFlowRoleID
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
                    List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();
                    this.ddl_TableType.DataSource = TableTypeList;
                    this.ddl_TableType.DataValueField = "ID";
                    this.ddl_TableType.DataTextField = "Name";
                    this.ddl_TableType.DataBind();

                    if (BaseFlowRoleID != 0)
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
                BaseFlowRole BaseFlowRole = db.BaseFlowRole.FirstOrDefault(t => t.ID == BaseFlowRoleID);

                if (BaseFlowRole != null)
                {
                    Flow Flow = db.Flow.FirstOrDefault(t => t.ID == BaseFlowRole.FlowID);

                    this.ddl_TableType.SelectedValue = ((int)Flow.TableTypeID).ToString();
                    this.txt_Role.Text = db.Role.FirstOrDefault(t => t.ID == BaseFlowRole.RoleID).Name;
                    this.hf_RoleID.Value = BaseFlowRole.RoleID.ToString();
                }
            }

        }


        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {

                    int FlowID = Convert.ToInt32(this.ddl_Flow.SelectedValue);

                    if (db.Flow.FirstOrDefault(t => t.ID == FlowID) == null)
                    {
                        ShowMessage("请选择正确的流程");
                        return;
                    }

                    string rolearray = this.hf_RoleID.Value.TrimEnd(',').TrimStart(',').Trim();

                    foreach (string role in rolearray.Split(','))
                    {
                        int roleid = Convert.ToInt32(role);
                        BaseFlowRole BaseFlowRole = db.BaseFlowRole.FirstOrDefault(t => t.RoleID == roleid && t.FlowID == FlowID);
                        if (BaseFlowRole == null && db.Role.FirstOrDefault(t => t.ID == roleid) != null)
                        {
                            BaseFlowRole = new BaseFlowRole();
                            BaseFlowRole.RoleID = roleid;
                            BaseFlowRole.FlowID = FlowID;
                            db.BaseFlowRole.Add(BaseFlowRole);
                        }

                    }

                    ShowMessage();
                    new SysLogDAO().AddLog(LogType.操作日志_添加, "添加基础流程权限", UserID);
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