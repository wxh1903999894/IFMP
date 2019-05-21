/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 11时10分19秒
** 描    述:      用户编辑页面
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
using System.Text;
using System.Data;
using System.Configuration;
using System.IO;


using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;

namespace IFMP.intelligentdevice
{
    public partial class IntelligentDeviceEdit : PageBase
    {
        #region 参数集合
        public int IntelligentDeviceID
        {
            get
            {
                return GetQueryString<int>("id", 0);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<DeviceType>(this.ddl_DeviceType, "-99");

                if (IntelligentDeviceID != 0)
                {
                    InfoBind();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                IntelligentDevice IntelligentDevice = db.IntelligentDevice.FirstOrDefault(t => t.ID == IntelligentDeviceID);
                if (IntelligentDevice != null)
                {
                    txt_Name.Text = IntelligentDevice.Name;
                    ddl_DeviceType.SelectedValue = ((int)IntelligentDevice.DeviceType).ToString();
                    txt_Place.Text = IntelligentDevice.Place;
                    txt_Identity.Text = IntelligentDevice.Identity;
                    txt_BeginDate.Text = IntelligentDevice.BeginDate == null ? "" : IntelligentDevice.BeginDate.Value.ToString("HH:mm:ss");
                    txt_EndDate.Text = IntelligentDevice.EndDate == null ? "" : IntelligentDevice.EndDate.Value.ToString("HH:mm:ss");
                    txt_Master.Text = IntelligentDevice.UserID.ToString();
                    ddl_DeviceType.Enabled = false;
                }
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //这里应该有权限，很重要
                    IntelligentDevice IntelligentDevice = db.IntelligentDevice.FirstOrDefault(t => t.ID == IntelligentDeviceID);

                    if (IntelligentDevice == null)
                    {
                        IntelligentDevice = new IntelligentDevice();
                        IntelligentDevice.Name = txt_Name.Text.Trim();
                        if (db.IntelligentDevice.FirstOrDefault(t => t.IsDel != true && t.Name == IntelligentDevice.Name) != null)
                        {
                            ShowMessage("设备名称重复！");
                            return;
                        }
                        IntelligentDevice.DeviceType = (DeviceType)Convert.ToInt32(ddl_DeviceType.SelectedValue);
                        IntelligentDevice.Identity = txt_Identity.Text.Trim();
                        if (db.IntelligentDevice.FirstOrDefault(t => t.IsDel != true
                            && t.DeviceType == IntelligentDevice.DeviceType
                            && t.Identity == IntelligentDevice.Identity
                            ) != null)
                        {
                            ShowMessage("设备标识重复！");
                            return;
                        }

                        if (!string.IsNullOrEmpty(txt_BeginDate.Text.Trim()))
                            IntelligentDevice.BeginDate = Convert.ToDateTime(txt_BeginDate.Text.Trim());

                        if (!string.IsNullOrEmpty(txt_EndDate.Text.Trim()))
                            IntelligentDevice.EndDate = Convert.ToDateTime(txt_EndDate.Text.Trim());

                        IntelligentDevice.CreateDate = DateTime.Now;
                        IntelligentDevice.IsDel = false;
                        IntelligentDevice.Place = txt_Place.Text;
                        IntelligentDevice.UserID = Convert.ToInt32(txt_Master.Text);
                        db.IntelligentDevice.Add(IntelligentDevice);

                        new SysLogDAO().AddLog(LogType.操作日志_添加, "添加智能设备", UserID);
                    }
                    else
                    {
                        IntelligentDevice.Name = txt_Name.Text.Trim();

                        if (db.IntelligentDevice.FirstOrDefault(t => t.IsDel != true
                            && t.Name == IntelligentDevice.Name
                            && t.ID != IntelligentDevice.ID) != null)
                        {
                            ShowMessage("设备名称重复！");
                            return;
                        }

                        IntelligentDevice.Identity = txt_Identity.Text.Trim();

                        if (db.IntelligentDevice.FirstOrDefault(t => t.IsDel != true
                            && t.DeviceType == IntelligentDevice.DeviceType
                            && t.Identity == IntelligentDevice.Identity
                            && t.ID != IntelligentDevice.ID
                            ) != null)
                        {
                            ShowMessage("设备标识重复！");
                            return;
                        }

                        if (!string.IsNullOrEmpty(txt_BeginDate.Text.Trim()))
                            IntelligentDevice.BeginDate = Convert.ToDateTime(txt_BeginDate.Text.Trim());

                        if (!string.IsNullOrEmpty(txt_EndDate.Text.Trim()))
                            IntelligentDevice.EndDate = Convert.ToDateTime(txt_EndDate.Text.Trim());

                        IntelligentDevice.Place = txt_Place.Text;

                        new SysLogDAO().AddLog(LogType.操作日志_修改, "修改智能设备", UserID);
                    }

                    db.SaveChanges();
                    ShowMessage();

                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}