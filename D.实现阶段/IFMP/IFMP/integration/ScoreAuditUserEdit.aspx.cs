/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月23日 16时29分47秒
** 描    述:      审核人员添加修改页面
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

namespace IFMP.integration
{
    public partial class ScoreAuditUserEdit : PageBase
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
                CommonFunction.BindEnum<ScoreAuditUserType>(this.ddl_UserType, "-99");
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
            ScoreAuditUser model = db.ScoreAuditUser.FirstOrDefault(t => t.ID == SUID);
            if (model != null)
            {
                this.ddl_UserType.SelectedValue = Convert.ToInt32(model.ScoreAuditUserType).ToString();
                this.hf_CID.Value = model.UserID.ToString();
                User user = db.User.FirstOrDefault(t => t.ID == model.UserID);
                this.txt_SysID.Text = user.RealName.ToString();
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
                        ScoreAuditUser model = db.ScoreAuditUser.FirstOrDefault(t => t.ID == SUID);
                        if (model == null)
                        {
                            string uids = this.txt_SysID.Text.ToString();
                            uids = uids.TrimEnd(',').TrimStart(',');
                            int uid = -1;
                            foreach (string id in uids.Split(','))
                            {
                                model = new ScoreAuditUser();
                                if (!string.IsNullOrEmpty(id) && int.TryParse(id, out uid))
                                {
                                    model.UserID = uid;
                                    model.ScoreAuditUserType = (ScoreAuditUserType)Convert.ToInt32(this.ddl_UserType.SelectedValue.ToString());
                                    if (db.ScoreAuditUser.FirstOrDefault(t => t.UserID == model.UserID && t.ScoreAuditUserType == model.ScoreAuditUserType) != null)
                                    {
                                        ShowMessage("当前信息重复，请检查后重新添加");
                                        return;
                                    }
                                    db.ScoreAuditUser.Add(model);
                                }
                            }
                        }
                        db.SaveChanges();

                        ShowMessage();
                        LogType log = (SUID == -1 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                        new SysLogDAO().AddLog(log, (SUID == -1 ? "增加" : "修改") + "审核人员信息");
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