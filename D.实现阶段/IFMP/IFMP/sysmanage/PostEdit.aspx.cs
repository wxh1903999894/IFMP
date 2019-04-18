/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月13日 10时39分19秒
** 描    述:      部门信息管理页面
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
    public partial class PostEdit : PageBase
    {
        #region 参数集合
        public int PostID
        {
            get
            {
                return GetQueryString<int>("id", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (PostID != -1)
                {
                    InfoBind();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                Post Post = db.Post.FirstOrDefault(t => t.ID == PostID && t.IsDel != true);
                if (Post != null)
                {
                    this.txt_Name.Text = Post.Name;
                    this.txt_Description.Text = Post.Description;
                    this.txt_Order.Text = Post.Order.ToString();
                }
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    string message = "";
                    Post Post = db.Post.FirstOrDefault(t => t.ID == PostID);
                    if (Post == null)
                    {
                        Post = new Post();
                        Post.CreateDate = DateTime.Now;
                        Post.CreateUserID = UserID;
                        Post.IsDel = false;
                        Post.Description = this.txt_Description.Text;
                        Post.Name = this.txt_Name.Text;
                        if (db.Post.FirstOrDefault(t => t.Name == Post.Name) != null)
                        {
                            ShowMessage("岗位名称已存在，请修改后重新添加");
                            return;
                        }
                        Post.Order = Convert.ToInt32(this.txt_Order.Text);
                        message = "添加名称为【" + Post.Name + "】的岗位信息";
                        db.Post.Add(Post);
                        new SysLogDAO().AddLog(LogType.操作日志_添加, message, UserID);
                    }
                    else
                    {
                        Post.Description = this.txt_Description.Text;
                        Post.Name = this.txt_Name.Text;
                        if (db.Post.FirstOrDefault(t => t.Name == Post.Name && t.ID != Post.ID) != null)
                        {
                            ShowMessage("岗位名称已存在，请修改后重新添加");
                            return;
                        }
                        Post.Order = Convert.ToInt32(this.txt_Order.Text);
                        message = "修改名称为【" + Post.Name + "】的岗位信息";
                        new SysLogDAO().AddLog(LogType.操作日志_修改, message, UserID);
                    }
                    db.SaveChanges();
                    ShowMessage();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}