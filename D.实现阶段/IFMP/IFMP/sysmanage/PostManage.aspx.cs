/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 8时49分19秒
** 描    述:      用户信息管理页面
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
    public partial class PostManage : PageBase
    {

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["Name"] = CommonFunction.GetCommoneString(this.txt_Name.Text.ToString().Trim());
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string name = ViewState["Name"].ToString();

            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<Post> PostList = db.Post.Where(t => t.IsDel != true && t.Name.Contains(name)).OrderBy(t => t.Order).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();


                if (PostList.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                this.rp_List.DataSource = PostList;
                Pager.RecordCount = db.Post.Count(t => t.IsDel != true && t.Name.Contains(name));
                this.rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }
        }
        #endregion


        #region 分页事件
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


        #region 删除事件
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            string ids = hf_CheckIDS.Value.ToString();

            try
            {
                ids = ids.TrimEnd(',').TrimStart(',');

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (string id in ids.Split(','))
                    {
                        int selid = Convert.ToInt32(id);
                        Post Post = db.Post.FirstOrDefault(t => t.ID == selid);

                        if (Post != null)
                        {
                            Post.IsDel = true;
                        }
                        else
                        {
                            ShowMessage("删除失败");
                            return;
                        }
                        db.SaveChanges();
                        ShowMessage("删除成功");
                        new SysLogDAO().AddLog(LogType.操作日志_删除, "删除岗位信息", UserID);
                    }
                }
                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}