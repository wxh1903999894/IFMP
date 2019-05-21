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


namespace IFMP.integration
{
    public partial class UserTypeEdit : PageBase
    {
        #region 参数集合
        public int DepartmentUserID
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

                    List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();
                    this.ddl_UserType.DataSource = DepartmentList;
                    this.ddl_UserType.DataValueField = "ID";
                    this.ddl_UserType.DataTextField = "Name";
                    this.ddl_UserType.DataBind();
                }
                this.ddl_UserType.DataBind();
                if (DepartmentUserID != 0)
                {
                    BindInfo();
                }
            }
        }


        public void BindInfo()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                DepartmentUser DepartmentUser = db.DepartmentUser.FirstOrDefault(t => t.ID == DepartmentUserID);
                if (DepartmentUser != null)
                {
                    this.ddl_UserType.SelectedValue = DepartmentUser.DepartmentID.ToString();
                    this.txt_SysID.Text = DepartmentUser.UserID.ToString();
                    //txt_SysID.Text = db.User.FirstOrDefault(t => t.ID == DepartmentUser.UserID).RealName;
                    //hf_CID.Value = db.User.FirstOrDefault(t => t.ID == DepartmentUser.UserID).ID.ToString();
                    //btn_plancom.s
                }
            }
        }


        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txt_SysID.Text.ToString()))
                {
                    ShowMessage("请至少选择一名人员！！");
                    return;
                }

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int depid = Convert.ToInt32(ddl_UserType.SelectedValue);
                    foreach (string userid in this.txt_SysID.Text.ToString().TrimEnd(',').Split(','))
                    {
                        int selid = Convert.ToInt32(userid);
                        DepartmentUser DepartmentUser = db.DepartmentUser.FirstOrDefault(t => t.UserID == selid && t.DepartmentID == depid);
                        if (DepartmentUser == null)
                        {
                            DepartmentUser = new DepartmentUser();
                            DepartmentUser.DepartmentID = depid;
                            DepartmentUser.UserID = selid;
                            db.DepartmentUser.Add(DepartmentUser);
                            db.SaveChanges();
                        }
                    }
                    ShowMessage();
                }
            }
            catch (Exception error)
            {

                ShowMessage(error.Message);
            }
        }
    }
}