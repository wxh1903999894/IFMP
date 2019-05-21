using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using IFMPLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace DormitoryAPP.ashx
{
    /// <summary>
    /// GwtUser 的摘要说明
    /// </summary>
    public class GetUser : IHttpHandler
    {
        private StringBuilder sb = new StringBuilder("");
        public void ProcessRequest(HttpContext context)
        {
            GetUserData(context);
        }

        public void GetUserData(HttpContext context)
        {
            string code = context.Request.Params["code"].ToString();
            string userid = "";
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //string accesstoken = new DDUtils().GetAccessToken();
                    string dduserid = new DDUtils().GetUserByCode(code);
                    User User = db.User.FirstOrDefault(t => t.DDID == dduserid);
                    if (User != null)
                    {
                        userid = User.ID.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            context.Response.Clear();

            context.Response.Write(sb.Append("{\"result\":\"true\",\"data\":\"" + userid + "\"}"));
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