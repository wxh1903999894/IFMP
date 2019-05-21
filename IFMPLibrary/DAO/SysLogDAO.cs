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
    public class SysLogDAO
    {
        public void AddLog(LogType logtype, string message = "", int? involveduser = null)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                SysLog log = new SysLog();
                log.CreateDate = DateTime.Now;
                log.LogType = logtype;

                log.InvolvedUser = involveduser;

                try
                {
                    log.IP = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr") == null ? "" : HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString();
                }
                catch
                {
                    log.IP = "";
                }
                log.Message =  message;
                db.SysLog.Add(log);
                db.SaveChanges();
            }
        }
    }
}
