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
using System.Web.UI.WebControls;


using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using System.Text;
using System.Data;


namespace IFMP.sysmanage
{
    public partial class ResourceManage : PageBase
    {
        #region 参数集合
        public int ResourcePathID
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
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        private void GetCondition()
        {
            ViewState["Name"] = CommonFunction.GetCommoneString(this.txt_Name.Text.ToString().Trim());
        }
        #endregion

        public class ResourceFull
        {
            public int ID { get; set; }
            public String Name { get; set; }
            //1 path 2 data
            public string Type { get; set; }
            public string Path { get; set; }
            public string IsCarousel { get; set; }
        }

        #region 数据绑定
        private void DataBindList()
        {
            string Name = ViewState["Name"].ToString();


            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<ResourceFull> returnobj = new List<ResourceFull>();
                List<ResourcePath> FullResourcePathList = db.ResourcePath.ToList();
                List<ResourcePath> ResourcePathList = FullResourcePathList.Where(t => t.ParentID == ResourcePathID && t.IsDel != true).ToList();

                if (ResourcePathID != 0)
                {
                    ResourceFull ResourceFull = new ResourceManage.ResourceFull();
                    ResourceFull.ID = FullResourcePathList.FirstOrDefault(t => t.ID == ResourcePathID).ParentID;
                    ResourceFull.Name = "返回";
                    //是按这个排序的，所以z开头
                    ResourceFull.Type = "最后";
                    ResourceFull.Path = "";
                    ResourceFull.IsCarousel = "";
                    returnobj.Add(ResourceFull);
                }

                foreach (ResourcePath ResourcePath in ResourcePathList)
                {
                    ResourceFull ResourceFull = new ResourceManage.ResourceFull();
                    ResourceFull.ID = ResourcePath.ID;
                    ResourceFull.Name = ResourcePath.Name;
                    ResourceFull.Type = "文件夹";
                    ResourceFull.Path = "";
                    ResourceFull.IsCarousel = "";
                    returnobj.Add(ResourceFull);
                }

                List<ResourceData> ResourceDataList = db.ResourceData.Where(t => t.ResourcePathID == ResourcePathID && t.IsDel != true).ToList();
                foreach (ResourceData ResourceData in ResourceDataList)
                {
                    ResourceFull ResourceFull = new ResourceManage.ResourceFull();
                    ResourceFull.ID = ResourceData.ID;
                    ResourceFull.Name = ResourceData.Name;
                    ResourceFull.Type = "图片";
                    ResourceFull.Path = ResourceDAO.GetPath(ResourceData, FullResourcePathList) + "/" + ResourceData.Name;
                    ResourceFull.IsCarousel = ResourceData.IsCarousel == true ? "是" : "否";
                    returnobj.Add(ResourceFull);
                }

                this.rp_List.DataSource = returnobj.OrderByDescending(t => t.Type).Skip(Pager.PageSize * (Pager.CurrentPageIndex - 1)).Take(Pager.PageSize).ToList();
                Pager.RecordCount = returnobj.Count;
                if (returnobj.Count > 0)
                {
                    tr_null.Visible = false;
                }
                else
                {
                    tr_null.Visible = true;
                }
                this.rp_List.DataBind();
                this.hf_CheckIDS.Value = "";

            }
        }
        #endregion


        #region 分页事件
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
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
            try
            {

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    LinkButton alter = (LinkButton)sender;   //delete为LinkButton的ID  
                    string data = alter.CommandArgument;
                    string type = data.Split('_')[0];
                    int id = Convert.ToInt32(data.Split('_')[1]);
                    if (type == "文件夹")
                    {
                        ResourcePath ResourcePath = db.ResourcePath.FirstOrDefault(t => t.ID == id);
                        ResourcePath.IsDel = true;
                        //将该路径下所有的文件均置为删除
                        //List<ResourceData> ResourceDataList = db.ResourceData.Where(t=>t.ResourcePathID==ResourcePath.ID).ToList();
                        new ResourceDAO().DeleteFullChildren(id);


                        db.SaveChanges();
                    }
                    else if (type == "图片")
                    {
                        ResourceData ResourceData = db.ResourceData.FirstOrDefault(t => t.ID == id);
                        if (ResourceData != null)
                        {
                            ResourceData.IsDel = true;
                            db.SaveChanges();
                        }
                    }
                }
                ShowMessage("删除成功");
                new SysLogDAO().AddLog(LogType.操作日志_删除, "删除资源", UserID);
                DataBindList();
                //this.hf_CheckIDS.Value = "";
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