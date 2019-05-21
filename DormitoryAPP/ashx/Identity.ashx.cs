using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DormitoryAPP.ashx
{
    /// <summary>
    /// Identity 的摘要说明
    /// </summary>
    public class Identity : IHttpHandler
    {
        StringBuilder sb = new StringBuilder("");
        public void ProcessRequest(HttpContext context)
        {
            string UID = context.Request.Params["UserID"];
            if (!string.IsNullOrEmpty(UID))
            {
                sb.Append("{\"result\":\"true\"}");
            }
            else
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