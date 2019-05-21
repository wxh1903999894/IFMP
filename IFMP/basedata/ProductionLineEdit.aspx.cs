
using System;
using System.Linq;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using System.Transactions;

namespace IFMP.basedata
{
    public partial class ProductionLineEdit : PageBase
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
            ProductionLine model = db.ProductionLine.FirstOrDefault(t => t.ID == ID);
            if (model != null)
            {
                this.txt_Name.Text = model.Name;
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
                        ProductionLine model = db.ProductionLine.FirstOrDefault(t => t.ID == ID);
                        if (model == null)
                        {
                            model = new ProductionLine();
                            model.IsDel = false;
                            model.CreateDate = DateTime.Now;
                            model.Name = txt_Name.Text;
                            if (db.ProductionLine.FirstOrDefault(t => t.Name == model.Name) != null)
                            {
                                ShowMessage("生产线名称重复");
                                return;
                            }
                            db.ProductionLine.Add(model);

                        }
                        else
                        {
                            if (db.ProductionLine.FirstOrDefault(t => t.Name == model.Name && t.ID != ID) != null)
                            {
                                ShowMessage("生产线名称重复");
                                return;
                            }
                            model.Name = txt_Name.Text;
                        }
                        db.SaveChanges();

                        ShowMessage();
                        LogType log = (ID == -1 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                        new SysLogDAO().AddLog(log, (ID == -1 ? "增加" : "修改") + "生产线信息");
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