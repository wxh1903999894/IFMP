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
    public partial class BaseDateList : PageBase
    {
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
                }

                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["TableType"] = this.ddl_TableType.SelectedValue;
            ViewState["ClassType"] = this.ddl_ClassType.SelectedValue;
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            //DateTime begin = Convert.ToDateTime(ViewState["begin"]);
            //DateTime end = Convert.ToDateTime(ViewState["end"]);
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"]));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"]), false);
            int tabletype = Convert.ToInt32(ViewState["TableType"]);
            int classtype = Convert.ToInt32(ViewState["ClassType"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                var basedateflowlist = from basedateflow in db.BaseDateFlow
                                       join tabletypedata in db.TableType on basedateflow.TableTypeID equals tabletypedata.ID
                                       where basedateflow.EndDate >= begin && basedateflow.EndDate <= end
                                        && (tabletype == -2 || basedateflow.TableTypeID == tabletype) && (classtype == -2 || basedateflow.ClassType == (ClassTypeEnums)classtype)
                                       //orderby basedateflow.TableTypeID, basedateflow.EndDate.TimeOfDay
                                       select new
                                       {
                                           basedateflow.ID,
                                           basedateflow.Name,
                                           basedateflow.ClassType,
                                           basedateflow.BeginDate,
                                           basedateflow.EndDate,
                                           basedateflow.RemindDate,
                                           basedateflow.TableTypeID,
                                           TableTypeName = tabletypedata.Name
                                       };

                if (basedateflowlist.Count() > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                rp_List.DataSource = basedateflowlist.ToList().OrderBy(t => t.TableTypeID).ThenBy(t => t.EndDate.TimeOfDay).Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList();
                Pager.RecordCount = basedateflowlist.Count();
                rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }

        }
        #endregion


        #region 删除
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                string ids = this.hf_CheckIDS.Value.TrimEnd(',');
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    try
                    {
                        foreach (string id in ids.Split(','))
                        {
                            int selid = Convert.ToInt32(id);
                            BaseDateFlow BaseDateFlow = db.BaseDateFlow.FirstOrDefault(t => t.ID == selid);
                            db.BaseDateFlow.Remove(BaseDateFlow);
                        }
                        db.SaveChanges();

                        new SysLogDAO().AddLog(LogType.操作日志_删除, "删除基础时间设置信息", UserID);
                        ShowMessage("删除成功");
                    }
                    catch
                    {
                        ShowMessage("删除失败");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                ShowMessage(ex.Message);
            }
            this.hf_CheckIDS.Value = "";
            DataBindList();
        }
        #endregion


        #region 分页
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 查询事件
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion
    }
}