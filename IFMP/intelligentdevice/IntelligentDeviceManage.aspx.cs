/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 8时49分19秒
** 描    述:      用户信息管理页面
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
using System.Text;
using System.Data;


namespace IFMP.intelligentdevice
{
    public partial class IntelligentDeviceManage : PageBase
    {

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<DeviceType>(this.ddl_DeviceType, "-2");

                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["Name"] = this.txt_Name.Text;
            ViewState["DeviceType"] = this.ddl_DeviceType.SelectedValue;

        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string Name = ViewState["Name"].ToString();
            int DeviceType = Convert.ToInt32(ViewState["DeviceType"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {


                var list = from intelligentdevice in db.IntelligentDevice
                           join user in db.User on intelligentdevice.UserID equals user.ID
                           where intelligentdevice.IsDel != true
                           && intelligentdevice.Name.Contains(Name)
                           && (DeviceType == -2 || intelligentdevice.DeviceType == (DeviceType)DeviceType)
                           select new
                           {
                               intelligentdevice.ID,
                               intelligentdevice.Name,
                               intelligentdevice.Identity,
                               intelligentdevice.BeginDate,
                               intelligentdevice.EndDate,
                               intelligentdevice.DeviceType,
                               intelligentdevice.Place,
                               user.RealName,
                               intelligentdevice.CreateDate
                           };

                int total = list.Count();

                if (total > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }

                this.rp_List.DataSource = list.OrderByDescending(t => t.CreateDate).Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList(); ;
                Pager.RecordCount = total;
                this.rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }
        }
        #endregion


        #region 分页事件
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 查询事件
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 删除事件
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            string ids = hf_CheckIDS.Value.ToString();

            try
            {
                ids = ids.TrimEnd(',').TrimStart(',');

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (string id in ids.Split(','))
                    {
                        int selid = Convert.ToInt32(id);
                        IntelligentDevice IntelligentDevice = db.IntelligentDevice.FirstOrDefault(t => t.ID == selid);
                        //Leave Leave = db.Leave.FirstOrDefault(t => t.ID == selid && t.LeaveState == LeaveState.未审核);

                        if (IntelligentDevice != null)
                        {
                            IntelligentDevice.IsDel = true;
                        }
                        else
                        {
                            ShowMessage("删除智能设备失败");
                            return;
                        }
                    }
                    db.SaveChanges();
                    ShowMessage("删除成功");
                    new SysLogDAO().AddLog(LogType.操作日志_删除, "删除智能设备", UserID);
                }
                DataBindList();
                this.hf_CheckIDS.Value = "";
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}