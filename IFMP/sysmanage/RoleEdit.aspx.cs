/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 11时10分19秒
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
using System.Text;
using System.Data;
using System.Configuration;
using System.IO;


using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;

namespace IFMP.sysmanage
{
    public partial class RoleEdit : PageBase
    {
        #region 参数集合
        public int RoleID
        {
            get
            {
                return GetQueryString<int>("id", 0);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (RoleID != 0)
                {
                    InfoBind();
                }
            }
        }
        #endregion


        #region 初始化数据
        private void InfoBind()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                Role Role = db.Role.FirstOrDefault(t => t.ID == RoleID);
                txt_Name.Text = Role.Name;
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
                    //这里应该有权限，很重要
                    Role Role = db.Role.FirstOrDefault(t => t.ID == RoleID);
                    if (Role == null)
                    {
                        Role = new Role();
                        Role.CreateDate = DateTime.Now;
                        Role.IsBase = false;
                        Role.IsDel = false;
                        Role.Name = this.txt_Name.Text;
                        if (db.Role.FirstOrDefault(t => t.Name == Role.Name) != null)
                        {
                            ShowMessage("权限名称重复");
                            return;
                        }
                        db.Role.Add(Role);
                        db.SaveChanges();
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "添加权限", UserID);
                    }
                    else
                    {
                        Role.Name = this.txt_Name.Text;
                        if (db.Role.FirstOrDefault(t => t.Name == Role.Name && t.ID != Role.ID) != null)
                        {
                            ShowMessage("权限名称重复");
                            return;
                        }
                        db.SaveChanges();
                        new SysLogDAO().AddLog(LogType.操作日志_修改, "修改权限", UserID);
                    }
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