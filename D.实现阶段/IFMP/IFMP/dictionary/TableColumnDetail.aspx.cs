/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月1日 9时08分19秒
** 描    述:      表单字段编辑页面
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

namespace IFMP.dictionary
{
    public partial class TableColumnDetail : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        /// <summary>
        /// 表单类型
        /// </summary>
        public int TType
        {
            get
            {
                return GetQueryString<int>("type", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindList();
            }
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            List<TableColumn> columnList = db.TableColumn.Where(t => t.TableTypeID == TType).OrderBy(t => t.Order).ToList();
            List<object> list = new List<object>();
            if (columnList.Count > 0)
            {
                foreach (TableColumn column in columnList)
                {
                    list.Add(new
                    {
                        column.ID,
                        column.ColumnName,
                        column.DefaultData,
                        column.IsStats,
                        column.Order,
                        DictName = db.Dictionary.FirstOrDefault(t => t.ID == column.DictionaryID) == null ? "" : db.Dictionary.FirstOrDefault(t => t.ID == column.DictionaryID).Name,
                        HintName = db.Dictionary.FirstOrDefault(t => t.ID == column.HintDictionaryID) == null ? "" : db.Dictionary.FirstOrDefault(t => t.ID == column.HintDictionaryID).Name,
                    });
                }
            }

            if (list.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = list;
            this.rp_List.DataBind();
        }
        #endregion


        protected void lbtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn = (LinkButton)sender;
                int id = Convert.ToInt32(lbtn.CommandArgument);
                TableColumn column = db.TableColumn.FirstOrDefault(t => t.ID == id);
                if (column != null)
                {
                    db.TableColumn.Remove(column);
                }
                else
                {
                    ShowMessage("删除失败");
                    return;
                }
                db.SaveChanges();
                ShowMessage("删除成功");
                DataBindList();
                new SysLogDAO().AddLog(LogType.系统日志, "成功删除字段信息", UserID);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
    }
}