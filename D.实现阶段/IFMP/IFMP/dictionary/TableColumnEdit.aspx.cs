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
using System.Transactions;

namespace IFMP.dictionary
{
    public partial class TableColumnEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int TID
        {
            get
            {
                return GetQueryString<int>("id", -1);
            }
        }

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
                List<Dictionary> dictlist = db.Dictionary.Where(t => t.IsDel != true).ToList();
                if (dictlist.Count > 0)
                {
                    this.ddl_DictionaryID.DataTextField = "Name";
                    this.ddl_DictionaryID.DataValueField = "ID";
                    this.ddl_DictionaryID.DataSource = dictlist;
                    this.ddl_DictionaryID.DataBind();

                    this.ddl_HintDictionaryID.DataTextField = "Name";
                    this.ddl_HintDictionaryID.DataValueField = "ID";
                    this.ddl_HintDictionaryID.DataSource = dictlist;
                    this.ddl_HintDictionaryID.DataBind();
                }
                this.ddl_DictionaryID.Items.Insert(0, new ListItem("--请选择--", "-2"));
                this.ddl_HintDictionaryID.Items.Insert(0, new ListItem("--请选择--", "-2"));
                CommonFunction.BindEnum<ColumnShowType>(this.ddl_ColumnShowType, "-2");

                //ddl_ColumnStatType
                CommonFunction.BindEnum<ColumnStatType>(this.ddl_ColumnStatType, "-2");

                TableColumn TableColumn = db.TableColumn.Where(t => t.TableTypeID == TType).OrderByDescending(t => t.Order).FirstOrDefault();
                this.txt_Order.Text = (TableColumn == null ? 1 : TableColumn.Order + 1).ToString();

                tr_IsFill.Visible = false;
                if (TID != -1)
                {
                    tr_IsFill.Disabled = false;
                    InfoBind();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            TableColumn column = db.TableColumn.FirstOrDefault(t => t.ID == TID);
            if (column != null)
            {
                this.txt_ColumnName.Text = column.ColumnName;
                if (column.DictionaryID != null)
                    this.ddl_DictionaryID.SelectedValue = column.DictionaryID.ToString();
                this.ddl_HintDictionaryID.SelectedValue = column.HintDictionaryID.ToString();
                this.txt_Order.Text = column.Order.ToString();
                this.rdo_IsStats.SelectedValue = Convert.ToInt32(column.IsStats).ToString();
                this.txt_DefaultData.Text = column.DefaultData.ToString();
                this.rdo_IsFill.SelectedValue = column.IsFill ? "1" : "0";



                if (column.IsFill)
                {
                    tr_IsFill.Visible = false;
                    tr_DefaultData.Visible = true;
                }
                else
                {
                    tr_IsFill.Visible = true;
                    tr_DefaultData.Visible = false;
                    ddl_ColumnShowType.SelectedValue = ((int)column.ColumnShowType.Value).ToString();
                    string ranage = string.Join(",", db.TableColumnRange.Where(t => t.TableColumnID == column.ID).Select(t => t.SourceID).ToArray());

                    txt_Range.Text = ranage.TrimEnd(',');


                }

            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    TableColumn model = db.TableColumn.FirstOrDefault(t => t.ID == TID);
                    if (model == null)
                    {
                        model = new TableColumn();
                        model.TableTypeID = TType;
                        model.ColumnName = this.txt_ColumnName.Text.ToString().Trim();

                        if (db.TableColumn.FirstOrDefault(t => t.ColumnName == model.ColumnName && t.TableTypeID == TType) != null)
                        {
                            ShowMessage("字段名称已存在，请检查后重新提交");
                            return;
                        }
                        model.DefaultData = this.txt_DefaultData.Text.ToString();
                        if (new BaseUtils().GetRegex(this.txt_Order.Text.ToString().Trim(), RegexType.正整数) == false)
                        {
                            ShowMessage("排序请填写正确的正整数！");
                            return;
                        }
                        else
                        {
                            model.Order = Convert.ToInt32(this.txt_Order.Text.ToString().Trim());
                        }
                        model.IsStats = Convert.ToBoolean(Convert.ToInt32(this.rdo_IsStats.SelectedValue.ToString()));

                        if (Convert.ToInt32(this.ddl_DictionaryID.SelectedValue) > 0)
                            model.DictionaryID = Convert.ToInt32(this.ddl_DictionaryID.SelectedValue);
                        if (Convert.ToInt32(this.ddl_HintDictionaryID.SelectedValue) > 0)
                            model.HintDictionaryID = Convert.ToInt32(this.ddl_HintDictionaryID.SelectedValue);

                        if (rdo_IsFill.SelectedValue == "1")
                        {
                            model.IsFill = true;
                            db.TableColumn.Add(model);
                            db.SaveChanges();
                        }
                        else
                        {
                            model.ColumnShowType = (ColumnShowType)Convert.ToInt32(ddl_ColumnShowType.SelectedValue);
                            model.IsFill = false;

                            db.TableColumn.Add(model);
                            db.SaveChanges();

                            if (!new ColumnDAO().ValidateColumnRange(txt_Range.Text, model.ColumnShowType.Value, TType))
                            {
                                ShowMessage("选择的表单字段不满足计算的类型");
                                ts.Dispose();
                                return;
                            }


                            string ColumnRange = txt_Range.Text;
                            if (ColumnRange.Split(',').Count() == 0)
                            {
                                ShowMessage("请至少选择一项数据");
                                ts.Dispose();
                                return;
                            }

                            foreach (string ColumnID in ColumnRange.Split(','))
                            {
                                TableColumnRange TableColumnRange = new TableColumnRange();
                                TableColumnRange.SourceID = Convert.ToInt32(ColumnID);
                                TableColumnRange.TableColumnID = model.ID;
                                db.TableColumnRange.Add(TableColumnRange);
                            }
                            db.SaveChanges();
                        }
                        new SysLogDAO().AddLog(LogType.操作日志_添加, "成功添加表单字段信息", UserID);
                    }
                    else
                    {
                        model.ColumnName = this.txt_ColumnName.Text.ToString().Trim();
                        model.DefaultData = this.txt_DefaultData.Text.ToString();
                        if (new BaseUtils().GetRegex(this.txt_Order.Text.ToString().Trim(), RegexType.正整数) == false)
                        {
                            ShowMessage("排序请填写正确的正整数！");
                            return;
                        }
                        else
                        {
                            model.Order = Convert.ToInt32(this.txt_Order.Text.ToString().Trim());
                        }
                        model.IsStats = Convert.ToBoolean(Convert.ToInt32(this.rdo_IsStats.SelectedValue.ToString()));

                        if (db.TableColumn.FirstOrDefault(t => t.ColumnName == model.ColumnName && t.TableTypeID != TType && model.ID != TID) != null)
                        {
                            ShowMessage("字段名称已存在，请检查后重新提交");
                            return;
                        }

                        if (Convert.ToInt32(this.ddl_DictionaryID.SelectedValue) > 0)
                        {
                            model.DictionaryID = Convert.ToInt32(this.ddl_DictionaryID.SelectedValue);
                        }
                        else
                        {
                            model.DictionaryID = null;
                        }
                        if (Convert.ToInt32(this.ddl_HintDictionaryID.SelectedValue) > 0)
                        {
                            model.HintDictionaryID = Convert.ToInt32(this.ddl_HintDictionaryID.SelectedValue);
                        }
                        else
                        {
                            model.HintDictionaryID = null;
                        }

                        if (rdo_IsFill.SelectedValue == "1")
                        {
                            model.IsFill = true;
                            db.SaveChanges();
                        }
                        else
                        {
                            model.ColumnShowType = (ColumnShowType)Convert.ToInt32(ddl_ColumnShowType.SelectedValue);

                            if (!new ColumnDAO().ValidateColumnRange(txt_Range.Text, model.ColumnShowType.Value, TType))
                            {
                                ShowMessage("选择的表单字段不满足计算的类型");
                                ts.Dispose();
                                return;
                            }

                            string ColumnRange = txt_Range.Text;
                            if (ColumnRange.Split(',').Count() == 0)
                            {
                                ShowMessage("请至少选择一项数据");
                                ts.Dispose();
                                return;
                            }

                            db.TableColumnRange.RemoveRange(db.TableColumnRange.Where(t => t.TableColumnID == model.ID));
                            foreach (string ColumnID in ColumnRange.Split(','))
                            {
                                TableColumnRange TableColumnRange = new TableColumnRange();
                                TableColumnRange.SourceID = Convert.ToInt32(ColumnID);
                                TableColumnRange.TableColumnID = model.ID;
                                db.TableColumnRange.Add(TableColumnRange);
                            }
                            model.IsFill = false;
                            db.SaveChanges();
                        }
                        new SysLogDAO().AddLog(LogType.操作日志_修改, "成功修改表单字段信息", UserID);
                    }
                    ts.Complete();
                    ShowMessage();
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                    new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                    ts.Dispose();
                    return;
                }
            }
        }
        #endregion


        #region 切换事件
        protected void rdo_IsFill_Changed(object sender, EventArgs e)
        {
            if (rdo_IsFill.SelectedValue == "1")
            {
                tr_IsFill.Visible = false;
                //tr_Dicntionary.Visible = true;
                tr_DefaultData.Visible = true;
            }
            else
            {
                tr_IsFill.Visible = true;
                //tr_Dicntionary.Visible = false;
                tr_DefaultData.Visible = false;
            }
        }
        #endregion
    }
}