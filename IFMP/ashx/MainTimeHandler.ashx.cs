using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using IFMPLibrary.DAO;
using IFMPLibrary.DBContext;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.Utils;

namespace IFMP.ashx
{
    /// <summary>
    /// MainTimeHandler 的摘要说明
    /// </summary>
    public class MainTimeHandler : IHttpHandler
    {
        IFMPDBContext db = new IFMPDBContext();


        public void ProcessRequest(HttpContext context)
        {
            GetTimeLine(context);
        }

        private void GetTimeLine(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            int id = Convert.ToInt32(context.Request["UserID"]);
            DateTime begindate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime enddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

            List<TaskFlow> taskflowlist = db.TaskFlow.Where(t => t.UserID == id && (t.BeginDate >= begindate && t.BeginDate <= enddate)).OrderBy(t => t.BeginDate).ToList();
            List<object> list = new List<object>();
            if (taskflowlist.Count > 0)
            {
                string begintime = "";
                foreach (TaskFlow taskflow in taskflowlist)
                {
                    begintime = taskflow.BeginDate.ToString("HH:mm");
                    list.Add(new
                    {
                        BeginDate = begintime,
                        TableType = db.Task.FirstOrDefault(t => t.ID == taskflow.TaskID && t.IsDel != true),
                        FlowID = taskflow.FlowID
                    });
                }
            }

            sb.Append("[");
            string name = "";
            if (list.Count > 0)
            {
                var testlist = taskflowlist.Where(t => t.BeginDate != null).Select(t => t.BeginDate).Distinct().ToList();
                Task task = new Task();
                List<TableType> TableTypeList = db.TableType.ToList();
                List<Flow> FlowList = db.Flow.ToList();
                for (int i = 0; i < testlist.Count; i++)
                {
                    string TableType = "";
                    string date = "";
                    string testdate = testlist[i].ToString("HH:mm");
                    for (int j = 0; j < list.Count; j++)
                    {
                        dynamic temp = list[j];
                        task = temp.TableType;
                        int flowid = Convert.ToInt32(temp.FlowID);
                        if (temp.BeginDate == testdate)
                        {
                            date = temp.BeginDate;
                            TableType = TableType + TableTypeList.FirstOrDefault(t => t.ID == task.TableTypeID).Name + ":" + FlowList.FirstOrDefault(t => t.ID == flowid).Name + ",";
                        }
                    }

                    name += "{\"BeginDate\":" + "\"" + date + "\",";
                    name += "\"TableType\":" + "\"" + TableType.TrimStart(',').TrimEnd(',') + "\"},";
                }
            }
            sb.Append(name.TrimEnd(','));
            sb.Append("]");
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            //context.Response.End();
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