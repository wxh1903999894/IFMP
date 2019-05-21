using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using System.Web;
using IFMPLibrary.Utils;

namespace IFMPLibrary.DAO
{
    public class ResourceDAO
    {
        public static string GetPath(ResourceData ResourceData, List<ResourcePath> ResourcePathList)
        {
            string path = "../Resource";

            if (ResourceData.ResourcePathID != 0)
            {
                string treepath = "";
                ResourcePath ResourcePath = ResourcePathList.FirstOrDefault(t => t.ID == ResourceData.ResourcePathID);
                treepath = "/" + ResourcePath.Name + treepath;
                while (ResourcePath.ParentID != 0)
                {
                    ResourcePath = ResourcePathList.FirstOrDefault(t => t.ID == ResourcePath.ParentID);
                    treepath = "/" + ResourcePath.Name + treepath;
                }


                path = path + treepath;
            }


            return path;
        }


        public static string GetPathByPathID(int ID, List<ResourcePath> ResourcePathList)
        {
            string path = "../Resource";
            string treepath = "";
            ResourcePath ResourcePath = ResourcePathList.FirstOrDefault(t => t.ID == ID);
            if (ResourcePath != null)
            {
                treepath = "/" + ResourcePath.Name + treepath;
                while (ResourcePath.ParentID != 0)
                {
                    ResourcePath = ResourcePathList.FirstOrDefault(t => t.ID == ResourcePath.ParentID);
                    treepath = "/" + ResourcePath.Name + treepath;
                }

                path = path + treepath;
            }

            return path;
        }

        public void DeleteFullChildren(int ID)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                //ResourcePath ResourcePath = db.ResourcePath.FirstOrDefault(t => t.ID == ID);
                List<ResourcePath> ResourcePathList = db.ResourcePath.Where(t => t.ParentID == ID).ToList();
                foreach (ResourcePath ResourcePath in ResourcePathList)
                {
                    ResourcePath.IsDel = true;
                    DeleteFullChildren(ResourcePath.ID);
                }

                List<ResourceData> ResourceDataList = db.ResourceData.Where(t => t.ResourcePathID == ID).ToList();
                foreach (ResourceData ResourceData in ResourceDataList)
                {
                    ResourceData.IsDel = true;
                }

                db.SaveChanges();
            }
        }


    }
}
