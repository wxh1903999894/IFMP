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
    public partial class NoPMUserGroupEdit : PageBase
    {
        #region 参数集合
        public int NoScoreUserID
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

                    this.ckl_Groups.DataSource = DepartmentList;
                    this.ckl_Groups.DataValueField = "ID";
                    this.ckl_Groups.DataTextField = "Name";
                    this.ckl_Groups.DataBind();
                }

                if (NoScoreUserID != 0)
                {
                    //this.btn_plancom.Visible = false;
                    this.txt_SysID.Enabled = false;
                    BindInfo();
                }
            }
        }


        public void BindInfo()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                NoScoreUser NoScoreUser = db.NoScoreUser.FirstOrDefault(t => t.ID == NoScoreUserID);
                if (NoScoreUser != null)
                {
                    //this.hf_CID.Value = NoScoreUser.ID.ToString();
                    //this.txt_SysID.Text = db.User.FirstOrDefault(t => t.ID == NoScoreUser.UserID).RealName;
                    this.txt_SysID.Text = NoScoreUser.UserID.ToString();
                    List<NoScoreUserDepartment> NoScoreUserDepartmentList = db.NoScoreUserDepartment.Where(t => t.NoScoreUserID == NoScoreUser.ID).ToList();
                    foreach (NoScoreUserDepartment NoScoreUserDepartment in NoScoreUserDepartmentList)
                    {
                        for (int i = 0; i < this.ckl_Groups.Items.Count;i++ )
                        {
                            if(NoScoreUserDepartment.DepartmentID.ToString()==ckl_Groups.Items[i].Value)
                            {
                                ckl_Groups.Items[i].Selected = true;
                            }
                        }
                    }
                }
            }


            //NoPMUserGroupEntity model = NoPMUserGroupBLL.GetObjByID(NoScoreUserID);
            //if (model != null)
            //{
            //    this.hf_CID.Value = model.SysID;
            //    this.txt_SysID.Text = model.SysIDName;
            //    string[] arr = model.Groups.Split(',');
            //    for (int i = 0; i < arr.Length; i++)
            //    {
            //        for (int j = 0; j < this.ckl_Groups.Items.Count; j++)
            //        {
            //            if (arr[i] == ckl_Groups.Items[j].Value)
            //            {
            //                ckl_Groups.Items[j].Selected = true;
            //                break;
            //            }
            //        }
            //    }
            //}
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
                    NoScoreUser NoScoreUser = db.NoScoreUser.FirstOrDefault(t => t.ID == NoScoreUserID);
                    string message = "";
                    if (NoScoreUser == null)
                    {
                        foreach (string id in this.txt_SysID.Text.ToString().TrimEnd(',').Split(','))
                        {
                            int selid = Convert.ToInt32(id);
                            NoScoreUser = new NoScoreUser();
                            NoScoreUser.UserID = selid;
                            if (db.NoScoreUser.FirstOrDefault(t => t.UserID == NoScoreUser.UserID) == null)
                            {
                                db.NoScoreUser.Add(NoScoreUser);
                                db.SaveChanges();
                            }
                            else
                            {
                                NoScoreUser = db.NoScoreUser.FirstOrDefault(t => t.UserID == selid);
                            }

                            for (int i = 0; i < this.ckl_Groups.Items.Count; i++)
                            {
                                if (ckl_Groups.Items[i].Selected)
                                {
                                    int departmentid = Convert.ToInt32(ckl_Groups.Items[i].Value);
                                    NoScoreUserDepartment NoScoreUserDepartment = db.NoScoreUserDepartment.FirstOrDefault(t => t.NoScoreUserID == NoScoreUser.ID && t.DepartmentID == departmentid);
                                    if (NoScoreUserDepartment == null)
                                    {
                                        NoScoreUserDepartment = new NoScoreUserDepartment();
                                        NoScoreUserDepartment.NoScoreUserID = NoScoreUser.ID;
                                        NoScoreUserDepartment.DepartmentID = departmentid;
                                        db.NoScoreUserDepartment.Add(NoScoreUserDepartment);
                                    }
                                }
                            }
                            db.SaveChanges();
                        }
                        message = "添加不排名分组人员";
                        new SysLogDAO().AddLog(LogType.操作日志_添加, message, UserID);
                    }
                    else
                    {
                        //这里应该是要默认的静止选人的功能
                        List<NoScoreUserDepartment> NoScoreUserDepartmentList = db.NoScoreUserDepartment.Where(t => t.NoScoreUserID == NoScoreUser.ID).ToList();
                        db.NoScoreUserDepartment.RemoveRange(NoScoreUserDepartmentList);
                        db.SaveChanges();

                        for (int i = 0; i < this.ckl_Groups.Items.Count; i++)
                        {
                            if (ckl_Groups.Items[i].Selected)
                            {
                                NoScoreUserDepartment NoScoreUserDepartment = new NoScoreUserDepartment();
                                NoScoreUserDepartment.NoScoreUserID = NoScoreUser.ID;
                                NoScoreUserDepartment.DepartmentID = Convert.ToInt32(ckl_Groups.Items[i].Value);
                                db.NoScoreUserDepartment.Add(NoScoreUserDepartment);
                            }
                        }
                        db.SaveChanges();
                    }
                    message = "修改不排名分组人员";
                    new SysLogDAO().AddLog(LogType.操作日志_修改, message, UserID);
                }

                ShowMessage();

            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                ShowMessage(ex.Message);
                return;
            }
        }
    }
}