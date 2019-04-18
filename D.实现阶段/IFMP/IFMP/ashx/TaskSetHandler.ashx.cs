using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;


namespace IFMP.ashx
{
    /// <summary>
    /// TaskSetHandler 的摘要说明
    /// </summary>
    public class TaskSetHandler : IHttpHandler
    {
        IFMPDBContext db = new IFMPDBContext();


        public void ProcessRequest(HttpContext context)
        {
            TaskList(context);
        }


        private void TaskList(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            List<TaskSet> tasksetlist = db.TaskSet.Where(t => t.IsDel != true).ToList();
            List<object> list = new List<object>();
            if (tasksetlist.Count > 0)
            {
                foreach (TaskSet taskset in tasksetlist)
                {
                    list.Add(new
                    {
                        taskset.ID,
                        taskset.TaskName,//任务名称
                        taskset.BaseClassID,//基础班次
                        taskset.ClassType,//班次类型
                        TableType = taskset.TableTypeID,//表单类型
                        taskset.Weeks,

                    });
                }
            }

            sb.Append("[");
            string name = "";
            //if (tasksetlist.Count > 0)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        name += "{\"id\":" + "\"" + dr["ID"].ToString() + "\",";
            //        name += "\"title\":" + "\"" + dr["RestName"].ToString() + "\",";
            //        name += "\"start\":" + "\"" + GetDate(int.Parse(dr["weeks"].ToString())) + " " + dr["BeginTime"].ToString() + "\",";
            //        name += "\"end\":" + "\"" + GetDate(int.Parse(dr["weeks"].ToString())) + " " + dr["EndTime"].ToString() + "\",";
            //        name += "\"bmname\":" + "\"" + dr["BMName"].ToString() + "\",";
            //        name += "\"ename\":" + "\"" + dr["EMName"].ToString() + "\",";
            //        name += "\"rname\":" + "\"" + dr["RMName"].ToString() + "\",";
            //        name += "\"members\":null,\"username\":null,\"notice\":false,\"allDay\":false,\"hasnoticed\":false,\"className\":null,\"editable\":false,\"backgroundColor\":\"#c90000\",\"textColor\":null,\"status\":\"正常\"},";
            //    }
            //}
            sb.Append(name.TrimEnd(','));
            sb.Append("]");
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        public string GetDate(int week)
        {
            DateTime startweek = DateTime.Now.AddDays(1 - Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d")));
            return startweek.AddDays(week - 1).ToString("yyyy-MM-dd");
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