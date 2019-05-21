/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月10日 13时50分19秒
** 描    述:      我的任务审核页面
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


namespace IFMP.taskflow
{
    public partial class TableDataDetails : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        /// <summary>
        /// 表单ID
        /// </summary>
        public int TableID
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
                InfoBind();
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            List<TableData> TableDataList = db.TableData.Where(t => t.TableID == TableID).ToList();
            List<TableColumn> TableColumnList = db.TableColumn.ToList();
            List<Dictionary> DictionaryList = db.Dictionary.ToList();
            List<DictionaryData> DictionaryDataList = db.DictionaryData.ToList();

            List<object> returnlist = new List<object>();
            foreach (TableData TableData in TableDataList)
            {
                TableColumn TableColumn = TableColumnList.FirstOrDefault(t => t.ID == TableData.TableColumnID);
                //string colname = TableColumn.ColumnName;
                if (TableColumn.DictionaryID == null)
                {
                    returnlist.Add(new
                    {
                        ColumnName = TableColumn.ColumnName,
                        Data = TableData.Data,
                        HintData = "",
                        IsAlert = TableData.IsAlert
                    });
                }
                else
                {
                    Dictionary Dictionary = DictionaryList.FirstOrDefault(t => t.ID == TableColumn.DictionaryID);
                    if (Dictionary.DisplayType == DictionaryTypeEnums.填写)
                    {
                        if (TableColumn.HintDictionaryID != null)
                        {
                            Dictionary HintDictionary = DictionaryList.FirstOrDefault(t => t.ID == TableColumn.HintDictionaryID);

                            if (HintDictionary.RegexType == RegexType.有范围的数字)
                            {
                                returnlist.Add(new
                                {
                                    ColumnName = TableColumn.ColumnName,
                                    Data = TableData.Data,
                                    HintData = HintDictionary.RegexData.Split('|')[0] + "-" + HintDictionary.RegexData.Split('|')[1],
                                    IsAlert = TableData.IsAlert
                                });
                            }
                            else
                            {
                                returnlist.Add(new
                                {
                                    ColumnName = TableColumn.ColumnName,
                                    Data = TableData.Data,
                                    HintData = "",
                                    IsAlert = TableData.IsAlert
                                });
                            }
                        }
                        else
                        {
                            returnlist.Add(new
                            {
                                ColumnName = TableColumn.ColumnName,
                                Data = TableData.Data,
                                HintData = "",
                                IsAlert = TableData.IsAlert
                            });
                        }

                    }
                    else
                    {
                        int DictionaryDataID = Convert.ToInt32(TableData.Data);
                        returnlist.Add(new
                        {
                            ColumnName = TableColumn.ColumnName,
                            Data = DictionaryDataList.FirstOrDefault(t => t.ID == DictionaryDataID).Data,
                            HintData = "默认为：" + TableColumn.DefaultData,
                            IsAlert = TableData.IsAlert
                        });
                    }
                }
            }

            rp_ColList.DataSource = returnlist;
            rp_ColList.DataBind();
        }
        #endregion


    }
}