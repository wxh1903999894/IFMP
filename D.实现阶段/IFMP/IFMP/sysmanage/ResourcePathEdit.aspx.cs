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
    public partial class ResourcePathEdit : PageBase
    {
        #region 参数集合
        public int ParentPathID
        {
            get
            {
                return GetQueryString<int>("parent", 0);
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

                InfoBind();

            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                ResourcePath ResourcePath = db.ResourcePath.FirstOrDefault(t => t.ID == ResourcePathID);
                if (ResourcePath != null)
                {
                    this.txt_Name.Text = ResourcePath.Name;
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
                        ResourcePath ResourcePath = db.ResourcePath.FirstOrDefault(t => t.ID == ResourcePathID);
                        List<ResourcePath> FullResourcePathList = db.ResourcePath.ToList();
                        if (ResourcePath == null)
                        {
                            ResourcePath = new ResourcePath();
                            ResourcePath.CreateDate = DateTime.Now;
                            ResourcePath.IsDel = false;
                            ResourcePath.Name = txt_Name.Text;
                            ResourcePath.ParentID = ParentPathID;

                            db.ResourcePath.Add(ResourcePath);
                            db.SaveChanges();

                            //在这个位置添加一个文件夹
                            //string path = "../Resource";
                            string subPath = System.Web.HttpContext.Current.Server.MapPath(ResourceDAO.GetPathByPathID(ParentPathID, FullResourcePathList)) + "/" + ResourcePath.Name + "/"; 
                            if (false == System.IO.Directory.Exists(subPath))
                            {
                                //创建pic文件夹
                                System.IO.Directory.CreateDirectory(subPath);
                            }

                            new SysLogDAO().AddLog(LogType.操作日志_添加, "添加资源路径", UserID);
                        }
                        else
                        {
                            ResourcePath.Name = txt_Name.Text;
                            db.SaveChanges();
                            new SysLogDAO().AddLog(LogType.操作日志_修改, "修改资源路径", UserID);
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