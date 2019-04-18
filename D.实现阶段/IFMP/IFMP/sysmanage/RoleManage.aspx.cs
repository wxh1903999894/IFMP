/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 15时13分19秒
** 描    述:      用户编辑页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;


using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;


namespace IFMP.sysmanage
{
    public partial class RoleManage : PageBase
    {


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Name"] = CommonFunction.GetCommoneString(this.txt_Name.Text.ToString().Trim());
                //string str = GetTableContent();
                //this.ltl_Content.Text = str.ToString();
                DataBindList();
            }
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string name = ViewState["Name"].ToString();
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<Role> RoleList = db.Role.Where(t => t.IsDel != true).ToList();

                if (RoleList.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                this.rp_List.DataSource = RoleList.OrderBy(t => t.IsBase).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
                Pager.RecordCount = RoleList.Count;
                this.rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }
        }
        #endregion


        #region 查询事件
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            ViewState["Name"] = CommonFunction.GetCommoneString(this.txt_Name.Text.ToString().Trim());
            DataBindList();
        }
        #endregion


        #region 分页事件
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 删除事件
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                string ids = this.hf_CheckIDS.Value.ToString();
                ids = ids.TrimEnd(',').TrimStart(',');
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (string id in ids.Split(','))
                    {
                        int selid = Convert.ToInt32(id);

                        Role Role = db.Role.FirstOrDefault(t => t.ID == selid);
                        if (Role != null)
                        {
                            if (Role.IsBase)
                            {
                                ShowMessage("基础权限无法删除");
                                return;
                            }
                            else
                            {
                                Role.IsDel = true;
                            }
                        }
                        else
                        {
                            ShowMessage("删除失败");
                            return;
                        }

                        db.SaveChanges();
                        ShowMessage("删除成功");
                        new SysLogDAO().AddLog(LogType.操作日志_删除, "删除权限", UserID);
                    }
                }

                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
            }
        }
        #endregion

    }
}