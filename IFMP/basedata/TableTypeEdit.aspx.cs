using System;
using System.Linq;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using System.Transactions;
using System.Collections.Generic;

namespace IFMP.basedata
{
    public partial class TableTypeEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int ID
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

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<ProductionLine> ProductionLineList = db.ProductionLine.Where(t => t.IsDel != true).ToList();

                    this.ddl_ProductionLineID.DataSource = ProductionLineList;
                    this.ddl_ProductionLineID.DataValueField = "ID";
                    this.ddl_ProductionLineID.DataTextField = "Name";
                    this.ddl_ProductionLineID.DataBind();
                    //this.ddl_DepID.Items.Insert(0, new ListItem("--请选择--", "-2"));

                }

                if (ID != -1)
                {
                    InfoBind();
                }
            }
        }
        #endregion

        #region 初始化用户数据
        private void InfoBind()
        {
            TableType model = db.TableType.FirstOrDefault(t => t.ID == ID);
            if (model != null)
            {
                this.txt_Name.Text = model.Name;
                this.rdo_IsMulti.SelectedValue = model.IsMulti ? "1" : "0";
            }
        }
        #endregion

        #region 提交事件
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        TableType model = db.TableType.FirstOrDefault(t => t.ID == ID);
                        if (model == null)
                        {
                            model = new TableType();
                            model.IsDel = false;
                            model.Name = txt_Name.Text;
                            model.ProductionLineID = Convert.ToInt32(ddl_ProductionLineID.SelectedValue);
                            if (rdo_IsMulti.SelectedValue == "1")
                            {
                                model.IsMulti = true;
                            }
                            else
                            {
                                model.IsMulti = false;
                            }

                            if (db.ProductionLine.FirstOrDefault(t => t.Name == model.Name) != null)
                            {
                                ShowMessage("表单名称重复");
                                return;
                            }
                            db.TableType.Add(model);

                        }
                        else
                        {
                            if (db.TableType.FirstOrDefault(t => t.Name == model.Name && t.ID != ID) != null)
                            {
                                ShowMessage("表单名称重复");
                                return;
                            }
                            model.Name = txt_Name.Text;
                            if (rdo_IsMulti.SelectedValue == "1")
                            {
                                model.IsMulti = true;
                            }
                            else
                            {
                                model.IsMulti = false;
                            }
                            model.ProductionLineID = Convert.ToInt32(ddl_ProductionLineID.SelectedValue);
                        }
                        db.SaveChanges();

                        ShowMessage();
                        LogType log = (ID == -1 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                        new SysLogDAO().AddLog(log, (ID == -1 ? "增加" : "修改") + "表单信息");
                        ts.Complete();
                    }
                    catch
                    {
                        ShowMessage("提交失败");
                        ts.Dispose();
                    }
                }
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