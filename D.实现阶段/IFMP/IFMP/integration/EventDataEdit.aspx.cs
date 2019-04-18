/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月18日 8时33分47秒
** 描    述:     积分制事件类型编辑页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;

namespace IFMP.integration
{
    public partial class EventDataEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int SEID
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
                this.ddl_Type.Items.Add(new ListItem("--请选择--", "-2"));
                List<ScoreEventType> scoreEventList = db.ScoreEventType.Where(t => t.IsDel != true && t.ParentID == 0).OrderBy(t => t.CreateDate).ToList();
                if (scoreEventList.Count > 0)
                {
                    for (int i = 0; i < scoreEventList.Count; i++)
                    {
                        this.ddl_Type.Items.Add(new ListItem(scoreEventList[i].Name.ToString(), scoreEventList[i].ID.ToString()));
                    }
                }
                if (SEID != -1)
                {
                    this.ddl_Type.Enabled = false;
                    InfoBind();
                }
            }
        }
        #endregion


        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        protected void InfoBind()
        {
            ScoreEventType model = db.ScoreEventType.FirstOrDefault(t => t.ID == SEID && t.IsDel != true);
            if (model != null)
            {
                this.ddl_Type.SelectedValue = model.ParentID.ToString();
                this.txt_DataName.Text = model.Name.Trim();
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                ScoreEventType model = db.ScoreEventType.FirstOrDefault(t => t.ID == SEID);
                if (model == null)
                {
                    model = new ScoreEventType();
                    model.Name = this.txt_DataName.Text.ToString().Trim();
                    model.ParentID = int.Parse(this.ddl_Type.SelectedValue) == -2 ? 0 : int.Parse(this.ddl_Type.SelectedValue);
                    model.CreateDate = DateTime.Now;
                    model.IsDel = false;
                    if (db.ScoreEventType.FirstOrDefault(t => t.Name == model.Name && t.IsDel == model.IsDel && t.ParentID == model.ParentID) != null)
                    {
                        ShowMessage("数据名称不能重复");
                        return;
                    }
                    db.ScoreEventType.Add(model);
                }
                else
                {
                    model.Name = this.txt_DataName.Text.ToString().Trim();
                    if (db.ScoreEventType.FirstOrDefault(t => t.Name == model.Name && t.IsDel == model.IsDel && t.ParentID == model.ParentID && t.ID != model.ID) != null)
                    {
                        ShowMessage("数据名称不能重复");
                        return;
                    }
                }
                db.SaveChanges();
                ShowMessage();
                LogType log = (SEID == -1 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                new SysLogDAO().AddLog(log, (SEID == -1 ? "增加" : "修改") + "日常事件类型");
            }
            catch (Exception error)
            {
                ShowMessage(error.Message);
                new SysLogDAO().AddLog(LogType.系统日志, error.Message, UserID);
            }
        }
        #endregion
    }
}