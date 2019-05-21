/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      汪笑寒
** 创建日期:      2019年4月18日 
** 描    述:      宿舍管理添加修改页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Linq;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using System.Transactions;

namespace IFMP.dormitory
{
    public partial class ProblemEdit : PageBase
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
                else
                {
                    this.txt_Order.Text = db.SpotSelectProblem.Count(t => t.IsDel != true) == 0 ? "1" : (db.SpotSelectProblem.Where(t => t.IsDel != true).Max(t => t.Order) + 1).ToString();
                }
            }
        }
        #endregion

        #region 初始化用户数据
        private void InfoBind()
        {
            SpotSelectProblem SpotSelectProblem = db.SpotSelectProblem.FirstOrDefault(t => t.ID == ID);
            if (SpotSelectProblem != null)
            {
                this.txt_Problem.Text = SpotSelectProblem.Problem;
                this.txt_Order.Text = SpotSelectProblem.Order.ToString();
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

                        SpotSelectProblem SpotSelectProblem = db.SpotSelectProblem.FirstOrDefault(t => t.ID == ID);
                        if (SpotSelectProblem == null)
                        {
                            SpotSelectProblem = new SpotSelectProblem();
                            SpotSelectProblem.Problem = txt_Problem.Text;
                            if (string.IsNullOrEmpty(SpotSelectProblem.Problem))
                            {
                                ShowMessage("请输入问题描述");
                                return;
                            }
                            if (db.SpotSelectProblem.FirstOrDefault(x => x.Problem == this.txt_Problem.Text.ToString()) != null)
                            {
                                ShowMessage("该问题已存在，请检查后重新添加");
                                return;
                            }
                            SpotSelectProblem.Order = Convert.ToInt32(txt_Order.Text);
                            SpotSelectProblem.IsDel = false;
                            db.SpotSelectProblem.Add(SpotSelectProblem);
                            db.SaveChanges();
                        }
                        else
                        {
                            SpotSelectProblem.Problem = txt_Problem.Text;
                            if (string.IsNullOrEmpty(SpotSelectProblem.Problem))
                            {
                                ShowMessage("请输入问题描述");
                                ts.Dispose();
                                return;
                            }
                            if (db.SpotSelectProblem.FirstOrDefault(x => x.Problem == this.txt_Problem.Text.ToString() && x.ID != SpotSelectProblem.ID) != null)
                            {
                                ShowMessage("该问题已存在，请检查后重新添加");
                                return;
                            }
                            SpotSelectProblem.Order = Convert.ToInt32(txt_Order.Text);
                            db.SaveChanges();
                        }
                        ShowMessage();
                        LogType log = (ID == -1 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                        new SysLogDAO().AddLog(log, (ID == -1 ? "增加" : "修改") + "点检问题信息");
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