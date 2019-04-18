using IFMPLibrary.DAO;
using IFMPLibrary.DBContext;
using IFMPLibrary.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFMP.ashx
{
    /// <summary>
    /// Resource 的摘要说明
    /// </summary>
    public class Resource : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Params["method"];
            switch (method)
            {
                case "AddResourcePath":
                    AddResourcePath(context);
                    break;
                case "AddResourceData":
                    AddResourceData(context);
                    break;
                case "GetResourcePath":
                    GetResourcePath(context);
                    break;
                case "GetFullResourceData":
                    GetFullResourceData(context);
                    break;
                case "GetParentResourcePath":
                    GetParentResourcePath(context);
                    break;
                default:
                    break;
            }
        }

        public void AddResourcePath(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int ParentID = Convert.ToInt32(context.Request["parentid"]);
                    string Name = context.Request["name"];

                    ResourcePath ResourcePath = new ResourcePath();
                    ResourcePath.CreateDate = DateTime.Now;
                    ResourcePath.IsDel = false;
                    ResourcePath.Name = Name;
                    ResourcePath.ParentID = ParentID;
                    db.ResourcePath.Add(ResourcePath);
                    db.SaveChanges();

                    returnobj.Add("result", "success");
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        public void AddResourceData(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int ResourcePathID = Convert.ToInt32(context.Request["pathid"]);
                    string Name = context.Request["name"];

                    ResourceData ResourceData = new ResourceData();
                    ResourceData.CreateDate = DateTime.Now;
                    ResourceData.IsDel = false;
                    ResourceData.Name = Name;
                    ResourceData.ResourcePathID = ResourcePathID;
                    db.ResourceData.Add(ResourceData);
                    db.SaveChanges();
                    returnobj.Add("result", "success");
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        /// <summary>
        /// 获取该路径下所有的文件和文件夹
        /// </summary>
        /// <param name="context"></param>
        public void GetResourcePath(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int ResourcePathID = Convert.ToInt32(context.Request["pathid"]);
                    List<ResourcePath> FullResourcePathList = db.ResourcePath.ToList();
                    List<ResourcePath> ResourcePathList = FullResourcePathList.Where(t => t.ParentID == ResourcePathID && t.IsDel != true).OrderBy(t => t.CreateDate).ToList();
                    JArray PathArray = new JArray();
                    foreach (ResourcePath ResourcePath in ResourcePathList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("Name", ResourcePath.Name);
                        jobject.Add("ID", ResourcePath.ID);
                        jobject.Add("ParentID", ResourcePath.ParentID);
                        jobject.Add("CreateDate", ResourcePath.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        PathArray.Add(jobject);
                    }

                    returnobj.Add("ResourcePath", PathArray);

                    List<ResourceData> ResourceDataList = db.ResourceData.Where(t => t.ResourcePathID == ResourcePathID && t.IsDel != true).OrderBy(t => t.CreateDate).ToList();
                    JArray DataArray = new JArray();
                    foreach (ResourceData ResourceData in ResourceDataList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("Name", ResourceData.Name);
                        jobject.Add("ID", ResourceData.ID);
                        jobject.Add("ResourcePathID", ResourceData.ResourcePathID);
                        jobject.Add("CreateDate", ResourceData.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        jobject.Add("Path", ResourceDAO.GetPath(ResourceData, FullResourcePathList) + "/" + ResourceData.Name);
                        DataArray.Add(jobject);
                    }

                    returnobj.Add("ResourceData", DataArray);

                    returnobj.Add("result", "success");
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        /// <summary>
        /// 获取所有展示图片
        /// </summary>
        /// <param name="context"></param>
        public void GetFullResourceData(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {

                    List<ResourceData> ResourceDataList = db.ResourceData.Where(t => t.IsDel != true && t.IsCarousel == true).OrderBy(t => t.CreateDate).ToList();
                    List<ResourcePath> ResourcePathList = db.ResourcePath.ToList();
                    JArray DataArray = new JArray();
                    foreach (ResourceData ResourceData in ResourceDataList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("Name", ResourceData.Name);
                        jobject.Add("ID", ResourceData.ID);
                        jobject.Add("ResourcePathID", ResourceData.ResourcePathID);
                        jobject.Add("CreateDate", ResourceData.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        jobject.Add("Path", ResourceDAO.GetPath(ResourceData, ResourcePathList) + "/" + ResourceData.Name);
                        DataArray.Add(jobject);
                    }

                    returnobj.Add("ResourceData", DataArray);
                    returnobj.Add("result", "success");
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }


        public void GetParentResourcePath(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {

                    int ID = Convert.ToInt32(context.Request["id"]);

                    ResourcePath ResourcePath = db.ResourcePath.FirstOrDefault(t => t.ID == ID);

                    returnobj.Add("result", "success");
                    returnobj.Add("ParentID", ResourcePath.ParentID);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}