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
    public partial class BaseClassList : PageBase
    {
        #region 页面初始化

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<ClassTypeEnums>(this.ddl_ClassType, "-2");

                GetCondition();
                DataBindList();
            }
        }
        #endregion

        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["ClassType"] = this.ddl_ClassType.SelectedValue;
            ViewState["Name"] = this.txt_Name.Text;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            string Name = ViewState["Name"].ToString();
            int classtype = Convert.ToInt32(ViewState["ClassType"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<BaseClass> BaseClassList = db.BaseClass.Where(t => t.IsDel != true && t.Name.Contains(Name) && (classtype == -2 || t.ClassType == (ClassTypeEnums)classtype)).ToList();

                if (BaseClassList.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                rp_List.DataSource = BaseClassList.OrderBy(t => t.ClassType).ThenBy(t => t.CreateDate).Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList();
                Pager.RecordCount = BaseClassList.Count;
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
                            BaseClass BaseClass = db.BaseClass.FirstOrDefault(t => t.ID == selid);
                            BaseClass.IsDel = true;
                        }
                        db.SaveChanges();

                        new SysLogDAO().AddLog(LogType.操作日志_删除, "删除基础班次设置信息", UserID);
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