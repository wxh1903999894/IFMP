using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;
using FeiLongLibrary.Utils;

namespace FeiLongLibrary.DAO
{
    public class SysLogDAO
    {
        FLDbContext db = new FLDbContext();
        public void AddLog(LogType logtype, string message = "", string involveduser = "")
        {
            SysLog log = new SysLog();
            log.CreateDate = DateTime.Now;
            log.Type = logtype;
            try
            {
                log.InvolvedUser = string.IsNullOrEmpty(involveduser) ? LoginHelper.CurrentUser.RealName : involveduser;
            }
            catch
            {
                log.InvolvedUser = involveduser;
            }
            try
            {
                log.IP = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr") == null ? "" : HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString();
            }
            catch
            {
                log.IP = "";
            }
            log.Message = LoginHelper.CurrentUser == null ? "" : LoginHelper.CurrentUser.RealName + message;
            db.SysLog.Add(log);
            db.SaveChanges();
        }
    }
}
