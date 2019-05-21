/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      汪笑寒
** 创建日期:      2019年4月19日 
** 描    述:      点检排班添加修改页面
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
    public partial class SchedulingEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int SUID
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

                CommonFunction.BindEnum<WeekDate>(this.ddl_DateType, "-99");
                if (SUID != -1)
                {
                    InfoBind();
                }
            }
        }
        #endregion

        #region 初始化用户数据
        private void InfoBind()
        {
            Scheduling model = db.Scheduling.FirstOrDefault(t => t.ID == SUID);
            if (model != null)
            {
                var value = (int)model.Date;
                CommonFunction.BindEnum<WeekDate>(this.ddl_DateType, value.ToString());
                this.txt_SysID.Text = model.CheckName.ToString();
                ddl_DateType.SelectedItem.Text = model.Date.ToString();
                ddl_DateType.SelectedValue = Convert.ToInt32(model.Date).ToString();
                ddl_DateType.Enabled = false;
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
                        string uids = this.txt_SysID.Text.ToString();
                        Scheduling model = db.Scheduling.FirstOrDefault(t => t.ID == SUID);
                        if (model == null)
                        {

                            var doruser = uids.Split(',');
                            Scheduling NewScheduling = new IFMPLibrary.Entities.Scheduling();

                            NewScheduling.Date = (WeekDate)Convert.ToInt32(ddl_DateType.SelectedValue);
                            var date = Convert.ToInt32(ddl_DateType.SelectedValue);
                            if (db.Scheduling.FirstOrDefault(t => t.Date == (WeekDate)date) != null)
                            {
                                ShowMessage("该日期名称已存在，请检查后重新添加");
                                return;
                            }

                            //if (!string.IsNullOrEmpty(uids))
                            //{
                            //    foreach (var userid in doruser)
                            //    {
                            //        var Scheduling = db.Scheduling.ToList();
                            //        foreach (var checkuser in Scheduling)
                            //        {
                            //            if (checkuser.CheckName.Split(',').Any(x => x == userid))
                            //            {
                            //                var id = int.Parse(userid);
                            //                var nowuser = db.User.FirstOrDefault(x => x.ID == id);
                            //                string message = string.Format("{0}已被录入，请检查后重新添加", nowuser.RealName);
                            //                ShowMessage(message);
                            //                return;
                            //            }
                            //        }
                            //    }
                            //}
                            NewScheduling.CheckName = uids;
                            db.Scheduling.Add(NewScheduling);
                            db.SaveChanges();
                        }
                        else
                        {
                            var date = Convert.ToInt32(ddl_DateType.SelectedValue);
                            if (db.Scheduling.FirstOrDefault(t => t.Date == (WeekDate)date && t.ID != model.ID) != null)
                            {
                                ShowMessage("该日期名称已存在，请检查后重新添加");
                                return;
                            }

                            model.Date = (WeekDate)Convert.ToInt32(ddl_DateType.SelectedValue);

                            var Schedulingmodel = db.Scheduling.FirstOrDefault(x => x.ID == model.ID);
                            //var user = Schedulingmodel.CheckName.Split(',');
                            //var uidslist = uids.Split(',');
                            //var newuser = "";
                            //var res = uidslist.Except(user);
                            //foreach (var v in res)
                            //{
                            //    newuser += v + ",";
                            //}

                            //if (!string.IsNullOrEmpty(uids) && !string.IsNullOrEmpty(newuser))
                            //{
                            //    foreach (var userid in newuser.TrimEnd(',').Split(','))
                            //    {
                            //        var Scheduling = db.Scheduling.ToList();
                            //        foreach (var checkuser in Scheduling)
                            //        {
                            //            if (checkuser.CheckName.Split(',').Any(x => x == userid))
                            //            {
                            //                var id = int.Parse(userid);
                            //                var nowuser = db.User.FirstOrDefault(x => x.ID == id);
                            //                string message = string.Format("{0}已被录入，请检查后重新添加", nowuser.RealName);
                            //                ShowMessage(message);
                            //                return;
                            //            }
                            //        }
                            //    }
                            //}
                            Schedulingmodel.CheckName = uids;
                            db.SaveChanges();
                        }
                        ShowMessage();
                        LogType log = (SUID == -1 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                        new SysLogDAO().AddLog(log, (SUID == -1 ? "增加" : "修改") + "点检排班管理信息");
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