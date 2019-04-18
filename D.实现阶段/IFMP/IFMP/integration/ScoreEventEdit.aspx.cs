/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月18日 14时25分47秒
** 描    述:      积分事件管理添加修改页面
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
    public partial class ScoreEventEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int SEID
        {
            get
            {
                return GetQueryString<int>("id", 0);
            }
        }
        public int SFlag
        {
            get
            {
                return GetQueryString<int>("sflag", 0);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CommonFunction.BindEnum<CommonEnum.IsorNot>(this.rbl_IsSpecializedUser);
                //this.rbl_IsSpecializedUser.SelectedIndex = 0;
                //this.ddl_FirstAduitUser.Items.Add(new ListItem("--请选择--", "-2"));
                //this.ddl_LastAduitUser.Items.Add(new ListItem("--请选择--", "-2"));
                //List<User> UserList = db.User.Where(t => db.ScoreAuditUser.Where(m => m.ScoreAuditUserType == ScoreAuditUserType.初审人).Select(m => m.UserID).Contains(t.ID) && t.IsDel != true).ToList();
                //if (UserList.Count > 0)
                //{
                //    for (int i = 0; i < UserList.Count; i++)
                //    {
                //        this.ddl_FirstAduitUser.Items.Add(new ListItem(UserList[i].RealName.ToString(), UserList[i].ID.ToString()));
                //    }
                //}
                //List<User> UserLast = db.User.Where(t => db.ScoreAuditUser.Where(m => m.ScoreAuditUserType == ScoreAuditUserType.终审人).Select(m => m.UserID).Contains(t.ID) && t.IsDel != true).ToList();
                //if (UserLast.Count > 0)
                //{
                //    for (int i = 0; i < UserLast.Count; i++)
                //    {
                //        this.ddl_LastAduitUser.Items.Add(new ListItem(UserLast[i].RealName.ToString(), UserLast[i].ID.ToString()));
                //    }
                //}
                ETypeBind();
                if (SEID != 0)
                {
                    BindInfo();
                }
            }
        }
        #endregion


        #region 递归类型菜单
        /// <summary>
        /// 类型
        /// </summary>
        private void ETypeBind()
        {
            if (SFlag == 2)
            {
                this.ddl_EType.Items.Add(new ListItem("--请选择--", "0"));
                ModelParent(0, this.ddl_EType, "");
            }
            else
            {
                //DataTable dt = SysDataBLL.GetList((int)CommonEnum.Deleted.未删除, -5);
                //CommonFunction.DDlTypeBind(this.ddl_EType, dt, "SDID", "DataName", "-2");
            }
        }
        private void ModelParent(int parentid, DropDownList ddl, string str)
        {
            string str_;
            List<ScoreEventType> scoreEventList = db.ScoreEventType.Where(t => t.IsDel != true && t.ParentID == parentid).OrderBy(t => t.CreateDate).ToList();
            for (int i = 0; i < scoreEventList.Count; i++)
            {
                if (scoreEventList[i].ParentID == 0)
                {
                    str_ = "";
                }
                else
                {
                    str_ = "├";
                }
                ListItem item = new ListItem();
                item.Text = str + str_ + scoreEventList[i].Name.ToString();     //Bind text
                item.Value = scoreEventList[i].ID.ToString();                                //Bind value
                int parent_id = Convert.ToInt32(item.Value.ToString());
                ddl.Items.Add(item);
                ModelParent(parent_id, ddl, str + "..");
            }
        }
        #endregion


        #region 初始化用户数据
        public void BindInfo()
        {
            ScoreEvent model = db.ScoreEvent.FirstOrDefault(t => t.ID == SEID && t.IsDel != true);
            if (model != null)
            {
                this.txt_BSCore.Text = model.BScore.ToString();
                this.txt_EventMark.Text = model.Content;
                this.txt_EventName.Text = model.Name;
                //this.ddl_FirstAduitUser.SelectedValue = model.FirstAuditUserID.ToString();
                //this.ddl_LastAduitUser.SelectedValue = model.LastAuditUserID.ToString();
                this.ddl_EType.SelectedValue = model.ScoreEventTypeID.ToString();
                //this.rbl_IsSpecializedUser.SelectedValue = model.IsSpecialUserAudit.ToString();
            }
        }
        #endregion


        #region 提交
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                ScoreEvent model = db.ScoreEvent.FirstOrDefault(t => t.ID == SEID && t.IsDel != true);
                if (model == null)
                {
                    model = new ScoreEvent();
                    model.Name = this.txt_EventName.Text.Trim();
                    model.AScore = 0;
                    model.BScore = Convert.ToInt32(this.txt_BSCore.Text);
                    model.Content = this.txt_EventMark.Text.Trim();
                    model.ScoreEventTypeID = Convert.ToInt32(this.ddl_EType.SelectedValue);
                    //model.IsSpecialUserAudit = Convert.ToBoolean(Convert.ToInt32(this.rbl_IsSpecializedUser.SelectedValue));
                    //model.FirstAuditUserID = Convert.ToInt32(this.ddl_FirstAduitUser.SelectedValue);
                    //model.LastAuditUserID = Convert.ToInt32(this.ddl_LastAduitUser.SelectedValue);
                    model.ScoreEventType = (ScoreEventTypeEnum)SFlag;
                    model.CreateUserID = UserID;
                    model.CreateDate = DateTime.Now;
                    model.IsDel = Convert.ToBoolean((int)CommonEnum.Deleted.未删除);
                    if (db.ScoreEvent.FirstOrDefault(t => t.Name == model.Name && t.IsDel == model.IsDel) != null)
                    {
                        ShowMessage("事件名称不能重复");
                        return;
                    }
                    db.ScoreEvent.Add(model);
                }
                else
                {
                    model.Name = this.txt_EventName.Text.Trim();
                    model.AScore = 0;
                    model.BScore = Convert.ToInt32(this.txt_BSCore.Text);
                    model.Content = this.txt_EventMark.Text.Trim();
                    model.ScoreEventTypeID = Convert.ToInt32(this.ddl_EType.SelectedValue);
                    //model.IsSpecialUserAudit = Convert.ToBoolean(Convert.ToInt32(this.rbl_IsSpecializedUser.SelectedValue));
                    //model.FirstAuditUserID = Convert.ToInt32(this.ddl_FirstAduitUser.SelectedValue);
                    //model.LastAuditUserID = Convert.ToInt32(this.ddl_LastAduitUser.SelectedValue);
                    if (db.ScoreEvent.FirstOrDefault(t => t.Name == model.Name && t.IsDel == model.IsDel && t.ID != model.ID) != null)
                    {
                        ShowMessage("事件名称不能重复");
                        return;
                    }
                }

                db.SaveChanges();
                ShowMessage();
                LogType log = (SEID == 0 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                new SysLogDAO().AddLog(log, (SEID == 0 ? "增加" : "修改") + "名称为" + model.Name + "的事件信息");
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message);
                ShowMessage(ex.Message);
            }
        }
        #endregion
    }
}