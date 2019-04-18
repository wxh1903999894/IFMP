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
    public partial class BaseClassEdit : PageBase
    {
        #region 参数集合
        public int BaseClassID
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
                    if (BaseClassID != 0)
                    {
                        BindInfo();
                    }
                }
            }
        }


        public void BindInfo()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                BaseClass BaseClass = db.BaseClass.FirstOrDefault(t => t.ID == BaseClassID);

                if (BaseClass != null)
                {
                    this.ddl_ClassType.SelectedValue = BaseClass.ClassType.ToString();
                    this.txt_Name.Text = BaseClass.Name;
                }
            }

        }


        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    BaseClass BaseClass = db.BaseClass.FirstOrDefault(t => t.ID == BaseClassID);

                    if (BaseClass == null)
                    {
                        BaseClass = new BaseClass();
                        BaseClass.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue);
                        BaseClass.Name = this.txt_Name.Text;
                        BaseClass.CreateDate = DateTime.Now;
                        BaseClass.IsDel = false;

                        if (db.BaseClass.FirstOrDefault(t => t.Name == BaseClass.Name && t.IsDel != true) != null)
                        {
                            ShowMessage("基础班次名称重复");
                            return;
                        }

                        db.BaseClass.Add(BaseClass);
                        ShowMessage();
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "添加基础班次", UserID);
                        db.SaveChanges();
                    }
                    else
                    {
                        BaseClass.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue);
                        BaseClass.Name = this.txt_Name.Text;


                        if (db.BaseClass.FirstOrDefault(t => t.Name == BaseClass.Name && t.IsDel != true && t.ID != BaseClass.ID) != null)
                        {
                            ShowMessage("基础班次名称重复");
                            return;
                        }
                    }

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