using System.Web;
using System.Data;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.UI.WebControls;
using System.IO;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using IFMPLibrary.DAO;

namespace JFZAPP.ashx
{
    /// <summary>
    /// Identity 的摘要说明
    /// </summary>
    public class Identity : IHttpHandler
    {
        StringBuilder sb = new StringBuilder("");
        public void ProcessRequest(HttpContext context)
        {
            try
            {

                int UID = Convert.ToInt32(context.Request.Params["UserID"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    if (db.User.FirstOrDefault(t => t.ID == UID) != null)
                    {
                        sb.Append("{\"result\":\"true\"}");
                    }
                    else
                    {
                        sb.Append("{\"result\":\"false\"}");
                    }
                }
            }
            catch
            {
                sb.Append("{\"result\":\"false\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
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