﻿/*****************************************************************
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
    public partial class DormitroyEdit : PageBase
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
                if (SUID != -1)
                {
                    InfoBind();
                }
                else
                {
                    CommonFunction.BindEnum<CommonEnum.IsorNot>(this.rbl_IsCheck);
                    this.rbl_IsCheck.SelectedIndex = 1;
                }
            }
        }
        #endregion

        #region 初始化用户数据
        private void InfoBind()
        {
            Dormitory model = db.Dormitory.FirstOrDefault(t => t.ID == SUID);
            DormitoryUser usermodel = db.DormitoryUser.FirstOrDefault(x => x.DormitoryID == model.ID);
            if (model != null)
            {
                this.txt_User.Text = model.DormiName.ToString();
                this.txt_code.Text = model.DormiCode.ToString();
                this.txt_memo.Text = model.DormiDesc.ToString();
                if (usermodel != null)
                    this.txt_SysID.Text = usermodel.UserID.ToString();
                CommonFunction.BindEnum<CommonEnum.IsorNot>(this.rbl_IsCheck);
                this.rbl_IsCheck.SelectedIndex = Convert.ToInt32(model.IsCheck);
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
                        Dormitory model = db.Dormitory.FirstOrDefault(t => t.ID == SUID);
                        if (model == null)
                        {

                            var doruser = uids.Split(',');
                            Dormitory NewDormitory = new IFMPLibrary.Entities.Dormitory();
                            DormitoryUser DorUser = new IFMPLibrary.Entities.DormitoryUser();
                            NewDormitory.DormiDesc = this.txt_memo.Text.ToString();
                            if (this.txt_User.Text.ToString() == "")
                            {
                                ShowMessage("请填写宿舍名称");
                                return;
                            }
                            NewDormitory.DormiName = this.txt_User.Text.ToString();
                            if (this.txt_code.Text.ToString() == "")
                            {
                                ShowMessage("请填写宿舍编号");
                                return;
                            }
                            NewDormitory.DormiCode = this.txt_code.Text.ToString();
                            int iisreward = Convert.ToInt32(this.rbl_IsCheck.SelectedValue.ToString());
                            NewDormitory.IsCheck = Convert.ToBoolean(iisreward);
                            NewDormitory.CreateUser = UserID.ToString();
                            NewDormitory.CreateDate = DateTime.Now;
                            if (db.Dormitory.FirstOrDefault(t => t.DormiName == this.txt_User.Text.ToString()) != null)
                            {
                                ShowMessage("该宿舍名称已存在，请检查后重新添加");
                                return;
                            }
                            if (db.Dormitory.FirstOrDefault(t => t.DormiCode == this.txt_code.Text.ToString()) != null)
                            {
                                ShowMessage("该宿舍编号已存在，请检查后重新添加");
                                return;
                            }
                            if (!string.IsNullOrEmpty(uids))
                            {
                                foreach (var userid in doruser)
                                {
                                    var dormitoryuser = db.DormitoryUser.ToList();
                                    foreach (var dormiuser in dormitoryuser)
                                    {
                                        if (dormiuser.UserID.Split(',').Any(x => x == userid))
                                        {
                                            var id = int.Parse(userid);
                                            var nowuser = db.User.FirstOrDefault(x => x.ID == id);
                                            string message = string.Format("{0}已被录入，请检查后重新添加", nowuser.RealName);
                                            ShowMessage(message);
                                            return;
                                        }
                                    }
                                }
                            }

                            db.Dormitory.Add(NewDormitory);
                            db.SaveChanges();
                            //保存user到中间表
                            DorUser.DormitoryID = NewDormitory.ID;
                            DorUser.UserID = uids;

                            db.DormitoryUser.Add(DorUser);
                            db.SaveChanges();
                        }
                        else
                        {
                            if (db.Dormitory.FirstOrDefault(t => t.DormiName == this.txt_User.Text.ToString() && t.ID != model.ID) != null)
                            {
                                ShowMessage("该宿舍名称已存在，请检查后重新添加");
                                return;
                            }
                            if (db.Dormitory.FirstOrDefault(t => t.DormiCode == this.txt_code.Text.ToString() && t.ID != model.ID) != null)
                            {
                                ShowMessage("该宿舍编号已存在，请检查后重新添加");
                                return;
                            }
                            if (this.txt_User.Text.ToString() == "")
                            {
                                ShowMessage("请填写宿舍名称");
                                return;
                            }
                            if (this.txt_code.Text.ToString() == "")
                            {
                                ShowMessage("请填写宿舍编号");
                                return;
                            }

                            model.DormiName = this.txt_User.Text.ToString();
                            model.DormiDesc = this.txt_memo.Text.ToString();
                            model.DormiCode = this.txt_code.Text.ToString();
                            int iisreward = Convert.ToInt32(this.rbl_IsCheck.SelectedValue.ToString());
                            model.IsCheck = Convert.ToBoolean(iisreward);
                            model.CreateUser = UserID.ToString(); ;
                            model.CreateDate = DateTime.Now;
                            DormitoryUser dorusermodel = db.DormitoryUser.FirstOrDefault(x => x.DormitoryID == model.ID);
                            if (dorusermodel != null)
                            {
                                var user = dorusermodel.UserID.Split(',');
                                var uidslist = uids.Split(',');
                                var newuser = "";
                                var res = uidslist.Except(user);
                                foreach (var v in res)
                                {
                                    newuser += v + ",";
                                }

                                if (!string.IsNullOrEmpty(uids) && !string.IsNullOrEmpty(newuser))
                                {
                                    foreach (var userid in newuser.TrimEnd(',').Split(','))
                                    {
                                        var dormitoryuser = db.DormitoryUser.ToList();
                                        foreach (var dormiuser in dormitoryuser)
                                        {
                                            if (dormiuser.UserID.Split(',').Any(x => x == userid))
                                            {
                                                var id = int.Parse(userid);
                                                var nowuser = db.User.FirstOrDefault(x => x.ID == id);
                                                string message = string.Format("{0}已被录入，请检查后重新添加", nowuser.RealName);
                                                ShowMessage(message);
                                                return;
                                            }
                                        }
                                    }
                                }
                                dorusermodel.UserID = uids;
                            }
                            else
                            {
                                dorusermodel = new DormitoryUser();
                                dorusermodel.DormitoryID = model.ID;
                                //var user = dorusermodel.UserID.Split(',');
                                var uidslist = uids.Split(',');
                                var newuser = "";
                                var res = uidslist;
                                foreach (var v in res)
                                {
                                    newuser += v + ",";
                                }

                                if (!string.IsNullOrEmpty(uids) && !string.IsNullOrEmpty(newuser))
                                {
                                    foreach (var userid in newuser.TrimEnd(',').Split(','))
                                    {
                                        var dormitoryuser = db.DormitoryUser.ToList();
                                        foreach (var dormiuser in dormitoryuser)
                                        {
                                            if (dormiuser.UserID.Split(',').Any(x => x == userid))
                                            {
                                                var id = int.Parse(userid);
                                                var nowuser = db.User.FirstOrDefault(x => x.ID == id);
                                                string message = string.Format("{0}已被录入，请检查后重新添加", nowuser.RealName);
                                                ShowMessage(message);
                                                return;
                                            }
                                        }
                                    }
                                }
                                dorusermodel.UserID = uids;
                                db.DormitoryUser.Add(dorusermodel);
                            }

                            db.SaveChanges();
                        }
                        ShowMessage();
                        LogType log = (SUID == -1 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                        new SysLogDAO().AddLog(log, (SUID == -1 ? "增加" : "修改") + "宿舍管理信息");
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