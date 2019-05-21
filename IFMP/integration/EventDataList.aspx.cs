/*****************************************************************
** Copyright (c)   芜湖市高科电子有限公司
** 创 建 人:       樊紫红
** 创建日期:       2018年7月17日 16时19分47秒
** 描    述:       事件类型管理页面
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
    public partial class EventDataList : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ETypeBind();
                ViewState["SDID"] = this.ddl_Type.SelectedValue;//类型
                DataBindList();
            }
        }
        #endregion


        #region 递归类型菜单
        /// <summary>
        /// 类型
        /// </summary>
        private void ETypeBind()
        {
            this.ddl_Type.Items.Add(new ListItem("--请选择--", "0"));
            ModelParent(0, this.ddl_Type, "");

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
            int sdid = Convert.ToInt32(ViewState["SDID"].ToString());
            List<ScoreEventType> scoreEventList = db.ScoreEventType.Where(t => t.IsDel != true && (t.ID == sdid || sdid == 0)).OrderBy(t => t.CreateDate).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            int total = db.ScoreEventType.Count(t => t.IsDel != true && (t.ID == sdid || sdid == 0));
            if (scoreEventList.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = scoreEventList;
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
            ViewState["SDID"] = this.ddl_Type.SelectedValue;//类型
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
                    ScoreEventType model = db.ScoreEventType.FirstOrDefault(t => t.ID == iddata);
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
                new SysLogDAO().AddLog(LogType.操作日志_删除, "成功删除日常事件类型");
                ShowMessage("删除成功");
                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message.ToString());
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message);
            }
        }
        #endregion


        #region 获取名称
        public string GetName(object pid, object id)
        {
            try
            {
                ScoreEventType model = new ScoreEventType();
                int parentid = Convert.ToInt32(pid.ToString());
                int seid = Convert.ToInt32(id.ToString());
                if (parentid == 0)
                {
                    model = db.ScoreEventType.FirstOrDefault(t => t.IsDel != true && t.ID == seid);
                    return model.Name;
                }
                else
                {
                    string str = "";
                    model = db.ScoreEventType.FirstOrDefault(t => t.IsDel != true && t.ID == parentid);
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
    }
}