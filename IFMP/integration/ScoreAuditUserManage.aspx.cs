/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月23日 15时40分47秒
** 描    述:     审核人员管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;

namespace IFMP.integration
{
    public partial class ScoreAuditUserManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<ScoreAuditUserType>(this.ddl_UserType, "-2");
                DataListBind();
            }
        }
        #endregion


        #region 数据绑定
        private void DataListBind()
        {
            int type = Convert.ToInt32(this.ddl_UserType.SelectedValue.ToString());
            string name = this.txt_Name.Text;

            List<ScoreAuditUser> scoreAuditUserList = db.ScoreAuditUser.Where(t => db.User.Where(m => m.RealName.Contains(name)).Select(m => m.ID).Contains(t.UserID) && (type == -2 || t.ScoreAuditUserType == (ScoreAuditUserType)type)).OrderBy(t => t.ScoreAuditUserType).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            int total = db.ScoreAuditUser.Count(t => (t.ScoreAuditUserType == (ScoreAuditUserType)type || type == -2));

            if (scoreAuditUserList.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = scoreAuditUserList;
            Pager.RecordCount = total;
            this.rp_List.DataBind();
            this.hf_CheckIDS.Value = "";
        }
        #endregion


        #region 查询事件
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            DataListBind();
        }
        #endregion


        #region 删除事件
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    string ids = this.hf_CheckIDS.Value.ToString();
                    ids = ids.TrimEnd(',').TrimStart(',');
                    foreach (string id in ids.Split(','))
                    {
                        int iid = Convert.ToInt32(id);
                        ScoreAuditUser auser = db.ScoreAuditUser.FirstOrDefault(t => t.ID == iid);
                        if (auser != null)
                        {
                            db.ScoreAuditUser.Remove(auser);
                        }
                    }
                    db.SaveChanges();
                    new SysLogDAO().AddLog(LogType.操作日志_删除, "成功删除审核人员信息", UserID);
                }
                catch
                {
                    ShowMessage("删除失败");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
            }
            DataListBind();
            this.hf_CheckIDS.Value = "";
        }
        #endregion


        #region 分页事件
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataListBind();
        }
        #endregion


        #region 获取名称
        public string GetUserName(object sender)
        {
            try
            {
                int uid = Convert.ToInt32(sender.ToString());
                User model = db.User.FirstOrDefault(t => t.IsDel != true && t.ID == uid);
                return model.RealName;
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}