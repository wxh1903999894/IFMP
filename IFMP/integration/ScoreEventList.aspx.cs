/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月18日 11时17分47秒
** 描    述:     积分事件管理页面
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
using IFMPLibrary.Utils;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.DAO;

namespace IFMP.integration
{
    public partial class ScoreEventList : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
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
                this.hf_sflag.Value = SFlag.ToString();
                ETypeBind();
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["EventName"] = CommonFunction.GetCommoneString(this.txt_EventName.Text.Trim());//姓名
            ViewState["EType"] = this.ddl_EType.SelectedValue;
            ViewState["begin"] = this.txt_Begin.Text.Trim() == "" ? "1900-01-01" : this.txt_Begin.Text.Trim();
            ViewState["end"] = this.txt_End.Text.Trim() == "" ? "9999-12-31" : this.txt_End.Text.Trim();
        }
        #endregion

        #region 递归类型菜单
        /// <summary>
        /// 类型
        /// </summary>
        private void ETypeBind()
        {
            this.ddl_EType.Items.Add(new ListItem("--请选择--", "0"));
            ModelParent(0, this.ddl_EType, "");
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


        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void DataBindList()
        {
            string name = ViewState["EventName"].ToString();
            int etype = Convert.ToInt32(ViewState["EType"].ToString());
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"].ToString()), false);

            List<ScoreEvent> eventList = db.ScoreEvent.Where(t => t.IsDel != true && t.ScoreEventType == (ScoreEventTypeEnum)SFlag && t.Name.Contains(name) && (t.ScoreEventTypeID == etype || etype == 0) && t.CreateDate > begin && t.CreateDate < end).OrderBy(t => t.CreateDate).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            int total = db.ScoreEvent.Count(t => t.IsDel != true && (int)t.ScoreEventType == SFlag && t.Name.Contains(name) && (t.ScoreEventTypeID == etype || etype == 0) && t.CreateDate > begin && t.CreateDate < end);
            if (eventList.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = eventList;
            Pager.RecordCount = total;
            rp_List.DataBind();
            this.hf_CheckIDS.Value = "";
        }
        #endregion


        #region 分页事件
        /// <summary>
        /// 分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 查询事件
        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 删除事件
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            string ids = hf_CheckIDS.Value.ToString();
            try
            {
                ids = ids.TrimEnd(',').TrimStart(',');
                foreach (string id in ids.Split(','))
                {
                    int iddata = Convert.ToInt32(id);
                    ScoreEvent model = db.ScoreEvent.FirstOrDefault(t => t.ID == iddata);
                    if (model != null)
                    {
                        model.IsDel = true;
                    }
                    else
                    {
                        ShowMessage("删除失败");
                        return;
                    }
                }
                db.SaveChanges();
                new SysLogDAO().AddLog(LogType.操作日志_删除, "成功删除事件信息");
                ShowMessage("删除成功");
                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message.ToString());
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
            }
        }
        #endregion


        #region 获取名称
        public string GetName(object id)
        {
            try
            {
                ScoreEventType model = new ScoreEventType();
                int seid = Convert.ToInt32(id.ToString());
                model = db.ScoreEventType.FirstOrDefault(t => t.IsDel != true && t.ID == seid);
                if (model.ParentID == 0)
                {
                    model = db.ScoreEventType.FirstOrDefault(t => t.IsDel != true && t.ID == seid);
                    return model.Name;
                }
                else
                {
                    string str = "";
                    model = db.ScoreEventType.FirstOrDefault(t => t.IsDel != true && t.ID == model.ParentID);
                    str = "(" + model.Name + ")";
                    model = db.ScoreEventType.FirstOrDefault(t => t.IsDel != true && t.ID == seid);
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