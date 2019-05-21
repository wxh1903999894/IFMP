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
using System.Transactions;

namespace IFMP.sysmanage
{
    public partial class ResourceDataEdit : PageBase
    {
        #region 参数集合
        public int ResourceDataID
        {
            get
            {
                return GetQueryString<int>("id", 0);
            }
        }

        public int ResourcePathID
        {
            get
            {
                return GetQueryString<int>("path", 0);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ddl_IsCarousel.Items.Insert(0, new ListItem("是", "1"));
                this.ddl_IsCarousel.Items.Insert(0, new ListItem("否", "0"));

                if (ResourceDataID != 0)
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
                ResourceData ResourceData = db.ResourceData.FirstOrDefault(t => t.ID == ResourceDataID);
                if (ResourceData != null)
                {
                    List<ResourcePath> FullResourcePathList = db.ResourcePath.ToList();
                    this.txt_Name.Text = ResourceData.Name;
                    this.img.Visible = true;
                    this.img.ImageUrl = ResourceDAO.GetPath(ResourceData, FullResourcePathList) + "/" + ResourceData.Name;
                    this.hf_Photo.Value = this.img.ImageUrl;
                }
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (IFMPDBContext db = new IFMPDBContext())
                    {
                        //这里应该有权限，很重要
                        ResourceData ResourceData = db.ResourceData.FirstOrDefault(t => t.ID == ResourceDataID && t.IsDel != true);
                        List<ResourcePath> FullResourcePathList = db.ResourcePath.ToList();
                        string message = "";
                        if (ResourceData == null)
                        {
                            ResourceData = new ResourceData();

                            //ResourceData.Name = txt_Name.Text;
                            ResourceData.CreateDate = DateTime.Now;
                            ResourceData.IsCarousel = (ddl_IsCarousel.SelectedValue == "0" ? false : true);
                            ResourceData.IsDel = false;
                            ResourceData.ResourcePathID = ResourcePathID;
                            //ResourceData.ResourcePathID
                            #region 上传图片
                            if (this.fl_UpFile.HasFile)
                            {
                                int upsize = 4000000;
                                try
                                {
                                    upsize = Convert.ToInt32(ConfigurationManager.AppSettings["upsize"].ToString());
                                }
                                catch (Exception) { }


                                string path = System.Web.HttpContext.Current.Server.MapPath(ResourceDAO.GetPathByPathID(ResourcePathID, FullResourcePathList));
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                string name = this.fl_UpFile.PostedFile.FileName.ToString();

                                string filepath = path + "\\" + name;

                                if (this.fl_UpFile.PostedFile.ContentLength < upsize)
                                {
                                    this.fl_UpFile.SaveAs(filepath);
                                }

                                ResourceData.Name = name;
                                db.ResourceData.Add(ResourceData);
                                db.SaveChanges();
                                new SysLogDAO().AddLog(LogType.操作日志_添加, "添加资源图片", UserID);
                            }
                            else
                            {
                                ShowMessage("请添加图片");
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            ResourceData.IsCarousel = Convert.ToBoolean(ddl_IsCarousel.SelectedValue);
                            ResourceData.IsDel = false;
                            //ResourceData.ResourcePathID = ResourcePathID;
                            //若和原图相同则不替换
                        }
                        ShowMessage();

                        ts.Complete();
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                    new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                    ts.Dispose();
                    return;
                }
            }
        }
        #endregion
    }
}