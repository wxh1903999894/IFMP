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
    public partial class BaseClassUserEdit : PageBase
    {
        #region 参数集合
        public int TableTypeID
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


                    int tabletype = Convert.ToInt32(this.ddl_TableType.SelectedValue);

                    List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == tabletype).OrderBy(t => t.ParentID).ToList();

                    rp_List.DataSource = FlowList;
                    rp_List.DataBind();

                    if (TableTypeID != 0)
                    {
                        ddl_TableType.Enabled = false;
                        BindInfo();
                    }
                }
            }
        }

        public void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList userddl = (DropDownList)e.Item.FindControl("ddl_UserList");
            HiddenField flowhf = (HiddenField)e.Item.FindControl("hf_FlowID");
            int FlowID = Convert.ToInt32(flowhf.Value);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> UserList = db.User.Where(t => t.IsDel != true
                    && db.UserRole.Where(l => db.Role.Where(m => m.IsDel != true && (db.BaseFlowRole.Where(k => k.FlowID == FlowID).Count() == 0 || db.BaseFlowRole.Where(k => k.FlowID == FlowID).Select(k => k.RoleID).Contains(m.ID))).Select(m => m.ID).Contains(l.RoleID)
                    ).Select(l => l.UserID).Contains(t.ID)
                    ).ToList();


                userddl.DataSource = UserList;
                userddl.DataValueField = "ID";
                userddl.DataTextField = "RealName";
                userddl.DataBind();
            }



        }


        public void BindInfo()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                //BaseClass BaseClass = db.BaseClass.FirstOrDefault(t => t.ID == BaseClassID);

                //if (BaseClass != null)
                //{
                //    this.ddl_ClassType.SelectedValue = BaseClass.ClassType.ToString();
                //    this.txt_Name.Text = BaseClass.Name;
                //}
            }

        }

        protected void ddl_TableType_Change(object sender, EventArgs e)
        {

        }

        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //BaseClass BaseClass = db.BaseClass.FirstOrDefault(t => t.ID == BaseClassID);

                    //if (BaseClass == null)
                    //{
                    //    BaseClass = new BaseClass();
                    //    BaseClass.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue);
                    //    BaseClass.Name = this.txt_Name.Text;
                    //    BaseClass.CreateDate = DateTime.Now;
                    //    BaseClass.IsDel = false;

                    //    if (db.BaseClass.FirstOrDefault(t => t.Name == BaseClass.Name && t.IsDel != true) != null)
                    //    {
                    //        ShowMessage("基础班次名称重复");
                    //        return;
                    //    }

                    //    db.BaseClass.Add(BaseClass);
                    //    ShowMessage();
                    //    new SysLogDAO().AddLog(LogType.操作日志_添加, "添加基础班次", UserID);
                    //    db.SaveChanges();
                    //}
                    //else
                    //{
                    //    BaseClass.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue);
                    //    BaseClass.Name = this.txt_Name.Text;


                    //    if (db.BaseClass.FirstOrDefault(t => t.Name == BaseClass.Name && t.IsDel != true && t.ID != BaseClass.ID) != null)
                    //    {
                    //        ShowMessage("基础班次名称重复");
                    //        return;
                    //    }
                    //}

                    ShowMessage();
                    new SysLogDAO().AddLog(LogType.操作日志_修改, "修改基础班次", UserID);
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