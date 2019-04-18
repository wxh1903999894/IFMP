/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月27日 11时29分19秒
** 描    述:      字典信息管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;

namespace IFMP.dictionary
{
    public partial class DictionaryManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<DictionaryTypeEnums>(this.ddl_Type, "-2");
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["Name"] = CommonFunction.GetCommoneString(this.txt_Name.Text.ToString());
            ViewState["Type"] = this.ddl_Type.SelectedValue;
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string name = ViewState["Name"].ToString();
            int type = Convert.ToInt32(ViewState["Type"].ToString());
            List<Dictionary> list = db.Dictionary.Where(t => t.IsDel != true && t.Name.Contains(name) && (t.DisplayType == (DictionaryTypeEnums)type || type == -2)).OrderBy(t => t.CreateDate).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            int total = db.Dictionary.Count(t => t.IsDel != true && t.Name.Contains(name) && (t.DisplayType == (DictionaryTypeEnums)type || type == -2));
            if (list.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = list;
            Pager.RecordCount = total;
            this.rp_List.DataBind();
        }
        #endregion


        #region 查询事件
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 分页事件
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 删除事件
        /// <summary>
        /// 删除时会删除字典所附属选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                string ids = this.hf_CheckIDS.Value.ToString();
                ids = ids.TrimEnd(',').TrimStart(',');
                foreach (string id in ids.Split(','))
                {
                    int selid = Convert.ToInt32(id);
                    Dictionary dictionary = db.Dictionary.FirstOrDefault(t => t.ID == selid);

                    if (dictionary != null)
                    {
                        dictionary.IsDel = true;
                        List<DictionaryData> datalist = db.DictionaryData.Where(t => t.DictionaryID == dictionary.ID).ToList();
                        foreach (DictionaryData data in datalist)
                        {
                            data.IsDel = true;
                        }
                    }
                    else
                    {
                        ShowMessage("删除失败");
                        return;
                    }
                    db.SaveChanges();
                }
                ShowMessage("删除成功");
                new SysLogDAO().AddLog(LogType.操作日志_删除, "删除字典信息", UserID);
                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
            }
        }
        #endregion
    }
}