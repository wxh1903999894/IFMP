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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using System.Text;
using System.Data;


namespace IFMP.sysmanage
{
    public partial class DepartmentManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

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


        //#region 拼接表格
        ///// <summary>
        ///// 拼接表格
        ///// </summary>
        ///// <returns></returns>
        //private string GetTableContent()
        //{
        //    StringBuilder sb = new StringBuilder("");
        //    using (IFMPDBContext db = new IFMPDBContext())
        //    {
        //        List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && t.ParentID == -1).OrderByDescending(t => t.IsAdmin).ToList();
        //        List<User> UserList = db.User.Where(t => t.IsDel != true).ToList();
        //        if (DepartmentList.Count > 0)
        //        {
        //            try
        //            {
        //                foreach (Department Department in DepartmentList)
        //                {
        //                    sb.Append("<div class='layui-colla-item'>");
        //                    sb.Append("<h2 class='layui-colla-title'><a href='#' onclick='return showbox(" +
        //                        Department.ID + ")'>" +
        //                        Department.Name + "(" +
        //                        UserList.FirstOrDefault(t => t.ID == Department.MasterUserID).RealName + ")</a>" +
        //                        "<i class='layui-icon layui-colla-icon'></i></h2>");
        //                    string str = ChildBind(Department.ID, UserList).ToString();
        //                    if (str != "")
        //                    {
        //                        sb.Append("<div class='layui-colla-content'>");
        //                        sb.Append("<div class='layui-collapse' lay-accordion=''>");
        //                        sb.Append(str.ToString());
        //                        sb.Append("</div></div>");
        //                    }
        //                    sb.Append("</div>");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ShowMessage(ex.Message);
        //                //return;
        //            }
        //        }
        //        else
        //        {
        //            sb.Append("<div class='layui-colla-content layui-show'><p>暂无记录</p></div>");
        //        }
        //    }
        //    return sb.ToString();
        //}
        //private string ChildBind(int parentid, List<User> UserList)
        //{
        //    StringBuilder sb = new StringBuilder("");
        //    using (IFMPDBContext db = new IFMPDBContext())
        //    {
        //        List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true
        //           && t.ParentID == parentid).OrderByDescending(t => t.IsAdmin).ToList();
        //        if (DepartmentList.Count > 0)
        //        {
        //            foreach (Department Department in DepartmentList)
        //            {
        //                sb.Append("<div class='layui-colla-item'>");
        //                sb.Append("<h2 class='layui-colla-title'><a href='#' onclick='return showbox(" +
        //                     Department.ID + ")'>" +
        //                    Department.Name + "(" +
        //                    UserList.FirstOrDefault(t => t.ID == Department.MasterUserID).RealName + ")</a>" +
        //                    "<i class='layui-icon layui-colla-icon'></i></h2>");
        //                string str = ChildBind(Department.ID, UserList).ToString();
        //                if (str != "")
        //                {
        //                    sb.Append("<div class='layui-colla-content'>");
        //                    sb.Append("<div class='layui-collapse' lay-accordion=''>");
        //                    sb.Append(str.ToString());
        //                    sb.Append("</div></div>");
        //                }
        //                sb.Append("</div>");
        //            }
        //        }
        //    }
        //    return sb.ToString();
        //}
        //#endregion


        #region 数据绑定
        private void DataBindList()
        {
            string depname = ViewState["Name"].ToString();

            var list = from depart in db.Department.Where(t => t.IsDel != true)
                       join user in db.User.Where(t => t.IsDel != true) on depart.MasterUserID equals user.ID
                       where depart.Name.Contains(depname)
                       orderby depart.Order, depart.CreateDate descending
                       select new
                       {
                           depart.ID,
                           depart.Name,
                           depart.Description,
                           depart.ParentID,
                           depart.IsAdmin,
                           depart.CreateDate,
                           user.RealName
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
            this.rp_List.DataSource = list.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            Pager.RecordCount = total;
            this.rp_List.DataBind();
            this.hf_CheckIDS.Value = "";
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
                foreach (string id in ids.Split(','))
                {
                    int selid = Convert.ToInt32(id);
                    Department dep = db.Department.FirstOrDefault(t => t.ID == selid);

                    if (dep != null)
                    {
                        dep.IsDel = true;
                    }
                    else
                    {
                        ShowMessage("删除失败");
                        return;
                    }
                    db.SaveChanges();
                    ShowMessage("删除成功");
                    new SysLogDAO().AddLog(LogType.操作日志_删除, "删除部门信息", UserID);
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


        #region 获取名称
        public string GetName(object id)
        {
            try
            {
                Department model = new Department();
                int seid = Convert.ToInt32(id.ToString());
                model = db.Department.FirstOrDefault(t => t.IsDel != true && t.ID == seid);
                if (model.ParentID == -1)
                {
                    model = db.Department.FirstOrDefault(t => t.IsDel != true && t.ID == seid);
                    return model.Name;
                }
                else
                {
                    string str = "";
                    model = db.Department.FirstOrDefault(t => t.IsDel != true && t.ID == model.ParentID);
                    str = "(" + model.Name + ")";
                    model = db.Department.FirstOrDefault(t => t.IsDel != true && t.ID == seid);
                    str += model.Name;
                    return str;
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}