/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:     汪笑寒
** 创建日期:     2019年4月19日 
** 描    述:     点检排班管理页面
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

namespace IFMP.dormitory
{
    public partial class SchedulingList : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataListBind();
            }
        }
        #endregion

        #region 数据绑定
        private void DataListBind()
        {
            List<Scheduling> SchedulingList = db.Scheduling.ToList();

            if (SchedulingList.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = SchedulingList.OrderBy(t => t.Date).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            Pager.RecordCount = SchedulingList.Count;
            this.rp_List.DataBind();
            this.hf_CheckIDS.Value = "";
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
                        Scheduling Scheduling = db.Scheduling.FirstOrDefault(t => t.ID == iid);
                        if (Scheduling != null)
                        {
                            db.Scheduling.Remove(Scheduling);
                        }
                    }
                    db.SaveChanges();
                    new SysLogDAO().AddLog(LogType.操作日志_删除, "成功删除点检排班信息", UserID);
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
                int ID = Convert.ToInt32(sender);
                var Schedulinguserid = db.Scheduling.FirstOrDefault(x => x.ID == ID).CheckName.ToString().Split(',');

                string username = null;
                foreach (var userid in Schedulinguserid)
                {
                    var id = int.Parse(userid);
                    User user = db.User.FirstOrDefault(t => t.ID == id);

                    username += user.RealName.ToString() + ",";
                }
                return username.TrimEnd(',');
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}