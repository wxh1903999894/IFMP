﻿/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:     汪笑寒
** 创建日期:     2019年4月8日 
** 描    述:     员工宿舍管理页面
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
    public partial class ProblemList : PageBase
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
            string name = this.txt_Name.Text;
            List<SpotSelectProblem> SpotSelectProblemList = db.SpotSelectProblem.Where(t => t.Problem.Contains(name) && t.IsDel != true).ToList();

            if (SpotSelectProblemList.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = SpotSelectProblemList.OrderBy(t => t.Order).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            Pager.RecordCount = SpotSelectProblemList.Count;
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
                        SpotSelectProblem SpotSelectProblem = db.SpotSelectProblem.FirstOrDefault(t => t.ID == iid);
                        if (SpotSelectProblem == null)
                        {
                            ShowMessage("找不到选择的点检问题");
                            return;
                        }
                        SpotSelectProblem.IsDel = true;
                    }
                    db.SaveChanges();
                    new SysLogDAO().AddLog(LogType.操作日志_删除, "成功删除点检问题信息", UserID);
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
  
    }
}