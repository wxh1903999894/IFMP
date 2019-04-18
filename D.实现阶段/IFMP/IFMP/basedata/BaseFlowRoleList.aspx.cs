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
    public partial class BaseFlowRoleList : PageBase
    {
        #region 页面初始化

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
                    this.ddl_TableType.Items.Insert(0, new ListItem("--请选择--", "-2"));

                    List<Role> RoleList = db.Role.Where(t => t.IsDel != true).OrderBy(t => t.CreateDate).ToList();
                    this.ddl_Role.DataSource = RoleList;
                    this.ddl_Role.DataValueField = "ID";
                    this.ddl_Role.DataTextField = "Name";
                    this.ddl_Role.DataBind();
                    this.ddl_Role.Items.Insert(0, new ListItem("--请选择--", "-2"));
                    this.ddl_Role.SelectedValue = "-2";
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
            ViewState["Role"] = this.ddl_Role.SelectedValue;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            int tabletype = Convert.ToInt32(ViewState["TableType"]);
            int selrole = Convert.ToInt32(ViewState["Role"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                var list = from baseflowrole in db.BaseFlowRole
                           join role in db.Role on baseflowrole.RoleID equals role.ID
                           join flow in db.Flow on baseflowrole.FlowID equals flow.ID
                           join tabletypedata in db.TableType on flow.TableTypeID equals tabletypedata.ID
                           where (selrole == -2 || role.ID == selrole) && (tabletype == -2 || flow.TableTypeID == tabletype)
                           select new
                           {
                               baseflowrole.ID,
                               RoleName = role.Name,
                               FlowName = flow.Name,
                               baseflowrole.FlowID,
                               TableTypeName = tabletypedata.Name,
                               baseflowrole.RoleID
                           };


                int total = list.Count();
                if (total > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                rp_List.DataSource = list.OrderBy(t => t.FlowID).ThenBy(t => t.RoleID).Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList();
                Pager.RecordCount = total;
                rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }

        }
        #endregion


        #region 查询
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
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
                            BaseFlowRole BaseFlowRole = db.BaseFlowRole.FirstOrDefault(t => t.ID == selid);

                            db.BaseFlowRole.Remove(BaseFlowRole);
                        }
                        db.SaveChanges();

                        new SysLogDAO().AddLog(LogType.操作日志_删除, "删除基础流程权限设置信息", UserID);
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
    }
}