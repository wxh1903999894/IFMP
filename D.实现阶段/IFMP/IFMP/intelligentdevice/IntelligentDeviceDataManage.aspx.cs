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
    public partial class IntelligentDeviceDataManage : PageBase
    {

        #region 参数集合
        public int DeviceID
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
                CommonFunction.BindEnum<DeviceType>(this.ddl_DeviceType, "-2");
                CommonFunction.BindEnum<DeviceDataType>(this.ddl_DeviceDataType, "-2");


                if (DeviceID != -1)
                {
                    InfoBind();
                }
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                IntelligentDevice IntelligentDevice = db.IntelligentDevice.FirstOrDefault(t => t.ID == DeviceID);
                if (IntelligentDevice != null)
                {
                    txt_Name.Text = IntelligentDevice.Name;
                    ddl_DeviceType.SelectedValue = ((int)IntelligentDevice.DeviceType).ToString();
                }
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["Name"] = this.txt_Name.Text;
            ViewState["DeviceType"] = this.ddl_DeviceType.SelectedValue;
            ViewState["DeviceDataType"] = this.ddl_DeviceDataType.SelectedValue;
            ViewState["Begin"] = this.txt_BeginDate.Text == "" ? "1900-01-01" : this.txt_BeginDate.Text.ToString().Trim();
            ViewState["End"] = this.txt_EndDate.Text == "" ? "9999-12-31" : this.txt_EndDate.Text.ToString().Trim();
        }
        #endregion


        #region 数据绑定
        private void DataBindList()
        {
            string Name = ViewState["Name"].ToString();
            int DeviceType = Convert.ToInt32(ViewState["DeviceType"]);
            int DeviceDataType = Convert.ToInt32(ViewState["DeviceDataType"]);
            DateTime Begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["Begin"]), true);
            DateTime End = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["End"]), false);

            using (IFMPDBContext db = new IFMPDBContext())
            {


                var list = from intelligentdevicedata in db.IntelligentDeviceData
                           join intelligentdevice in db.IntelligentDevice on intelligentdevicedata.IntelligentDeviceID equals intelligentdevice.ID
                           where intelligentdevice.IsDel != true
                           && intelligentdevice.Name.Contains(Name)
                           && (DeviceType == -2 || intelligentdevice.DeviceType == (DeviceType)DeviceType)
                           && (DeviceDataType == -2 || intelligentdevicedata.DeviceDataType == (DeviceDataType)DeviceDataType)
                           && intelligentdevicedata.CreateDate > Begin && intelligentdevicedata.CreateDate < End
                           select new
                           {
                               intelligentdevicedata.ID,
                               intelligentdevice.Name,
                               intelligentdevice.DeviceType,
                               intelligentdevicedata.Data,
                               intelligentdevicedata.IsAlert,
                               intelligentdevicedata.DeviceDataType,
                               intelligentdevicedata.CreateDate
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


    }
}