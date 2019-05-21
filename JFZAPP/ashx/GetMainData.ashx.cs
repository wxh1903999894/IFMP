using System.Web;
using System.Data;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using IFMPLibrary.DAO;

namespace JFZAPP.ashx
{
    /// <summary>
    /// GetMainData 的摘要说明
    /// </summary>
    public class GetMainData : IHttpHandler
    {
        private StringBuilder sb = new StringBuilder("");
        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Params["method"];
            switch (method)
            {
                case "GetAuditUser":
                    GetAuditUser(context);
                    break;
                case "GetLastUser":
                    GetLastUser(context);
                    break;
                case "GetDep":
                    GetDep(context);
                    break;
                case "GetUser":
                    GetUser(context);
                    break;
                case "GetAuditLastUser":
                    GetAuditLastUser(context);
                    break;
                case "GetAuditState":
                    GetAuditState(context);
                    break;
                case "GetBuckleRegistrationBySID":
                    GetBuckleRegistrationBySID(context);
                    break;
                case "GetBuckleRegistrationAudit":
                    GetBuckleRegistrationAudit(context);
                    break;
                case "GetFirstorLastEvent":
                    GetFirstorLastEvent(context);
                    break;
                case "GetEventName":
                    GetEventName(context);
                    break;
                case "RegistraAdd":
                    RegistraAdd(context);
                    break;
                case "GetTicket":
                    GetTicket(context);
                    break;
                case "TaskAdd":
                    TaskAdd(context);
                    break;
                case "GetTask":
                    GetTask(context);
                    break;
                case "GetTaskByid":
                    GetTaskByid(context);
                    break;
                case "TaskAudit":
                    TaskAudit(context);
                    break;
                case "GetALLTask":
                    GetALLTask(context);
                    break;
                case "GetTableByFlag":
                    GetTableByFlag(context);
                    break;
                case "TaskQD":
                    TaskQD(context);
                    break;
                case "TaskWC":
                    TaskWC(context);
                    break;
                case "GetPM":
                    GetPM(context);
                    break;
                case "GetGroup":
                    GetGroup(context);
                    break;
            }
        }

        //获取当前人员所能查看的所有组别
        public void GetGroup(HttpContext context)
        {
            int userid = Convert.ToInt32(context.Request.Params["UserID"]);
            //这个用部门来分组
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<Department> departmentlist = new List<Department>();
                //判断是否为不参与人员
                if (db.NoScoreUser.FirstOrDefault(t => t.UserID == userid) != null)
                {
                    departmentlist = db.Department.Where(t => t.IsDel != true && db.NoScoreUserDepartment.Where(m => m.NoScoreUserID == db.NoScoreUser.FirstOrDefault(k => k.UserID == userid).UserID).Select(m => m.DepartmentID).Contains(t.ID)).ToList();
                }
                else
                {
                    //获取当前人所在的部门
                    departmentlist = db.Department.Where(t => t.IsDel != true && db.DepartmentUser.Where(m => m.UserID == userid).Select(m => m.DepartmentID).Contains(t.ID)).ToList();
                }

                //DataTable dt = SysDataBLL.GetListByUID((int)CommonEnum.Deleted.未删除, (int)CommonEnum.BaseDataType.积分制分组, uid);
                if (departmentlist.Count > 0)
                {
                    string name = "";
                    foreach (Department department in departmentlist)
                    {
                        name += "{\"SDID\":\"" + department.ID + "\",\"DataName\":\"" + department.Name
                       + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetPM(HttpContext context)
        {
            int uid = Convert.ToInt32(context.Request.Params["UserID"]);
            //1 年 2 月 3 累计
            int flag = Convert.ToInt32(context.Request.Params["flag"]);
            int group = Convert.ToInt32(context.Request.Params["group"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> UserList = db.User.Where(t => t.IsDel != true && !db.NoScoreUser.Select(m => m.UserID).Contains(t.ID)).ToList();

                DateTime BeginDate = DateTime.MinValue;
                DateTime EndDate = DateTime.MaxValue;
                if (flag == 1)
                {
                    BeginDate = Convert.ToDateTime(DateTime.Now.Year + "-01" + "-01");
                    EndDate = Convert.ToDateTime((DateTime.Now.Year + 1) + "-01" + "-01");
                }
                if (flag == 2)
                {
                    BeginDate = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
                    EndDate = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.AddMonths(1).Month + "-01");

                }
                List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.IsDel != true && db.Score.Where(m => m.CreateDate > BeginDate && m.CreateDate < EndDate && m.AuditState == AuditState.通过).Select(m => m.ID).Contains(t.ScoreID)).ToList();

                string name = "";
                foreach (User User in UserList)
                {
                    name += "{\"uid\":\"" + User.ID + "\",\"name\":\"" + User.RealName
                     + "\",\"score\":\"" + ScoreUserList.Where(t => t.UserID == User.ID).Sum(t => t.BScore)
                        //+ "\",\"px\":\"" + row["px"]
                        //判断本人
                        + "\",\"isbr\":\"" + (User.ID == uid ? "1" : "0")
                     + "\"},";
                }
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(name.TrimEnd(','));
                sb.Append("]}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void TaskWC(HttpContext context)
        {
            int SUID = Convert.ToInt32(context.Request.Params["SUID"]);
            int UID = Convert.ToInt32(context.Request.Params["UserID"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                ScoreTaskUser ScoreTaskUser = db.ScoreTaskUser.FirstOrDefault(t => t.UserID == UID && t.ScoreTaskID == SUID);
                if (ScoreTaskUser != null)
                {
                    ScoreTaskUser.CompleteDate = DateTime.Now;
                    db.SaveChanges();
                    sb.Append("{\"result\":\"true\"}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }

        public void TaskQD(HttpContext context)
        {
            int SUID = Convert.ToInt32(context.Request.Params["SUID"]);
            int UID = Convert.ToInt32(context.Request.Params["UserID"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                ScoreTaskUser ScoreTaskUser = new ScoreTaskUser();
                ScoreTaskUser.UserID = UID;
                ScoreTaskUser.ScoreTaskID = SUID;
                db.ScoreTaskUser.Add(ScoreTaskUser);
                db.SaveChanges();
                sb.Append("{\"result\":\"true\"}");
            }


            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetALLTask(HttpContext context)
        {
            int recordCount = 0;
            string UID = context.Request.Params["UserID"];
            int pagesize = Convert.ToInt32(context.Request.Params["pagesize"]);
            int pageindex = Convert.ToInt32(context.Request.Params["pageindex"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<ScoreTaskUser> ScoreTaskUserList = db.ScoreTaskUser.ToList();
                List<User> UserList = db.User.ToList();
                List<ScoreTask> ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && t.AuditState == AuditState.通过).OrderByDescending(t => t.EndDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                if (ScoreTaskList.Count > 0)
                {
                    string name = "";
                    foreach (ScoreTask ScoreTask in ScoreTaskList)
                    {
                        name += "{\"TaskName\":\"" + ScoreTask.Name + "\",\"TScore\":\"" + ScoreTask.CompleteBScore
                   + "\",\"TaskContent\":\"" + ScoreTask.Content
                   + "\",\"SignScore\":\"" + ScoreTask.SignBScore
                   + "\",\"TaskUser\":\"" + ScoreTask.CreateUserID
                   + "\",\"TaskUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.CreateUserID).RealName
                   + "\",\"SUID\":\"" + ScoreTask.ID
                   + "\",\"IsComp\":\"" + ScoreTaskUserList.FirstOrDefault(t => t.ScoreTaskID == ScoreTask.ID && t.CompleteDate != null) == null ? "0" : "1"
                   + "\",\"TState\":\"" + ScoreTask.AuditState
                   + "\",\"TStateName\":\"" + Enum.GetName(typeof(AuditState), ScoreTask.AuditState)
                   + "\",\"FirstAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID) == null ? "" : UserList.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                   + "\",\"LastAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID) == null ? "" : UserList.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                   + "\",\"FirstAduitDate\":\"" + ScoreTask.FirstAuditDate == null ? "" : ScoreTask.FirstAuditDate.Value.ToString("yyyy-MM-dd")
                   + "\",\"LastAduitDate\":\"" + ScoreTask.LastAuditDate == null ? "" : ScoreTask.LastAuditDate.Value.ToString("yyyy-MM-dd")
                   + "\",\"FirstAduitMess\":\"" + ScoreTask.FirstAuditMark
                   + "\",\"LastAduitMess\":\"" + ScoreTask.LastAuditMark
                   + "\",\"EndDate\":\"" + ScoreTask.EndDate.ToString("yyyy-MM-dd") + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void TaskAudit(HttpContext context)
        {
            int SUID = Convert.ToInt32(context.Request.Params["SUID"]);
            int UID = Convert.ToInt32(context.Request.Params["UserID"]);
            int AduitState = Convert.ToInt32(context.Request.Params["AduitState"]);
            string AduitMark = context.Request.Params["AduitMark"];

            using (IFMPDBContext db = new IFMPDBContext())
            {
                ScoreTask ScoreTask = db.ScoreTask.FirstOrDefault(t => t.ID == SUID && t.IsDel != true && (t.FirstAuditUserID == UID || t.LastAuditUserID == UID));
                if (ScoreTask != null)
                {
                    if (ScoreTask.FirstAuditUserID == UID)
                    {
                        ScoreTask.FirstAuditMark = AduitMark;
                        ScoreTask.AuditState = AuditState.待终审;
                    }
                    if (ScoreTask.LastAuditUserID == UID)
                    {
                        ScoreTask.LastAuditMark = AduitMark;
                        ScoreTask.AuditState = AuditState.通过;
                    }

                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetTaskByid(HttpContext context)
        {
            int SUID = Convert.ToInt32(context.Request.Params["SUID"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                ScoreTask ScoreTask = db.ScoreTask.FirstOrDefault(t => t.ID == SUID && t.IsDel != true);
                if (ScoreTask != null)
                {
                    string name = "";
                    name += "{\"TaskName\":\"" + ScoreTask.Name + "\",\"TScore\":\"" + ScoreTask.CompleteBScore
                  + "\",\"TaskContent\":\"" + ScoreTask.Content
                  + "\",\"SignScore\":\"" + ScoreTask.SignBScore
                  + "\",\"TaskUser\":\"" + ScoreTask.CreateUserID
                  + "\",\"TaskUserName\":\"" + db.User.FirstOrDefault(t => t.ID == ScoreTask.CreateUserID).RealName
                  + "\",\"SUID\":\"" + ScoreTask.ID
                  + "\",\"IsComp\":\"" + db.ScoreTaskUser.FirstOrDefault(t => t.ScoreTaskID == ScoreTask.ID && t.CompleteDate != null) == null ? "0" : "1"
                  + "\",\"TState\":\"" + ScoreTask.AuditState
                  + "\",\"TStateName\":\"" + Enum.GetName(typeof(AuditState), ScoreTask.AuditState)
                  + "\",\"FirstAduitUserName\":\"" + db.User.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID) == null ? "" : db.User.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                  + "\",\"LastAduitUserName\":\"" + db.User.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID) == null ? "" : db.User.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                  + "\",\"FirstAduitDate\":\"" + ScoreTask.FirstAuditDate == null ? "" : ScoreTask.FirstAuditDate.Value.ToString("yyyy-MM-dd")
                  + "\",\"LastAduitDate\":\"" + ScoreTask.LastAuditDate == null ? "" : ScoreTask.LastAuditDate.Value.ToString("yyyy-MM-dd")
                  + "\",\"FirstAduitMess\":\"" + ScoreTask.FirstAuditMark
                  + "\",\"LastAduitMess\":\"" + ScoreTask.LastAuditMark
                  + "\",\"EndDate\":\"" + ScoreTask.EndDate.ToString("yyyy-MM-dd") + "\"},";

                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }

        //这个是获取审核的
        public void GetTask(HttpContext context)
        {
            int UID = Convert.ToInt32(context.Request.Params["UserID"]);
            int pagesize = Convert.ToInt32(context.Request.Params["pagesize"]);
            int pageindex = Convert.ToInt32(context.Request.Params["pageindex"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<ScoreTaskUser> ScoreTaskUser = db.ScoreTaskUser.ToList();
                List<User> UserList = db.User.ToList();
                List<ScoreTask> ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && ((t.AuditState == AuditState.待初审 && t.FirstAuditUserID == UID) || (t.AuditState == AuditState.待终审 && t.FirstAuditDate != null && t.LastAuditUserID == UID))).OrderByDescending(t => t.EndDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();

                if (ScoreTaskList.Count == 0)
                {
                    string name = "";
                    foreach (ScoreTask ScoreTask in ScoreTaskList)
                    {
                        name += "{\"TaskName\":\"" + ScoreTask.Name + "\",\"TScore\":\"" + ScoreTask.CompleteBScore
                   + "\",\"TaskContent\":\"" + ScoreTask.Content
                   + "\",\"SignScore\":\"" + ScoreTask.SignBScore
                   + "\",\"TaskUser\":\"" + ScoreTask.CreateUserID
                   + "\",\"TaskUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.CreateUserID).RealName
                   + "\",\"SUID\":\"" + ScoreTask.ID
                   + "\",\"IsComp\":\"" + ScoreTaskUser.FirstOrDefault(t => t.ScoreTaskID == ScoreTask.ID && t.CompleteDate != null) == null ? "0" : "1"
                   + "\",\"TState\":\"" + ScoreTask.AuditState
                   + "\",\"TStateName\":\"" + Enum.GetName(typeof(AuditState), ScoreTask.AuditState)
                   + "\",\"FirstAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID) == null ? "" : UserList.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                   + "\",\"LastAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID) == null ? "" : UserList.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                   + "\",\"FirstAduitDate\":\"" + ScoreTask.FirstAuditDate == null ? "" : ScoreTask.FirstAuditDate.Value.ToString("yyyy-MM-dd")
                   + "\",\"LastAduitDate\":\"" + ScoreTask.LastAuditDate == null ? "" : ScoreTask.LastAuditDate.Value.ToString("yyyy-MM-dd")
                   + "\",\"FirstAduitMess\":\"" + ScoreTask.FirstAuditMark
                   + "\",\"LastAduitMess\":\"" + ScoreTask.LastAuditMark
                   + "\",\"EndDate\":\"" + ScoreTask.EndDate.ToString("yyyy-MM-dd") + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }

            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetTableByFlag(HttpContext context)
        {
            int UID = Convert.ToInt32(context.Request.Params["UserID"]);
            int pagesize = Convert.ToInt32(context.Request.Params["pagesize"]);
            int pageindex = Convert.ToInt32(context.Request.Params["pageindex"]);
            int flag = Convert.ToInt32(context.Request.Params["flag"]);
            //flag=1 auditstate=1,2 
            //flag=2 auditstate=3,4 
            //flag=3 未审核
            //flag=4 已审核
            //flag=5 全部审核

            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<ScoreTaskUser> ScoreTaskUser = db.ScoreTaskUser.ToList();
                List<User> UserList = db.User.ToList();
                List<ScoreTask> ScoreTaskList = new List<ScoreTask>(); ;
                switch (flag)
                {
                    case 1:
                        ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && t.CreateUserID == UID && (t.AuditState == AuditState.待初审 || t.AuditState == AuditState.待终审)).OrderByDescending(t => t.EndDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                        break;
                    case 2:
                        ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && t.CreateUserID == UID && (t.AuditState == AuditState.通过 || t.AuditState == AuditState.驳回)).OrderByDescending(t => t.EndDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                        break;
                    case 3:
                        ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && t.CreateUserID == UID && ((t.AuditState == AuditState.待初审 && t.FirstAuditUserID == UID) || (t.AuditState == AuditState.待终审 && t.LastAuditUserID == UID))).OrderByDescending(t => t.EndDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                        break;
                    case 4:
                        ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && t.CreateUserID == UID && (t.AuditState == AuditState.通过 || t.AuditState == AuditState.驳回) && (t.FirstAuditUserID == UID || t.LastAuditUserID == UID)).OrderByDescending(t => t.EndDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                        break;
                    case 5:
                        ScoreTaskList = db.ScoreTask.Where(t => t.IsDel != true && t.CreateUserID == UID && (t.AuditState != AuditState.待初审) && (t.FirstAuditUserID == UID || t.LastAuditUserID == UID)).OrderByDescending(t => t.EndDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                        break;

                    default:
                        break;
                }

                if (ScoreTaskList.Count == 0)
                {
                    string name = "";
                    foreach (ScoreTask ScoreTask in ScoreTaskList)
                    {
                        name += "{\"TaskName\":\"" + ScoreTask.Name + "\",\"TScore\":\"" + ScoreTask.CompleteBScore
                   + "\",\"TaskContent\":\"" + ScoreTask.Content
                   + "\",\"SignScore\":\"" + ScoreTask.SignBScore
                   + "\",\"TaskUser\":\"" + ScoreTask.CreateUserID
                   + "\",\"TaskUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.CreateUserID).RealName
                   + "\",\"SUID\":\"" + ScoreTask.ID
                   + "\",\"IsComp\":\"" + ScoreTaskUser.FirstOrDefault(t => t.ScoreTaskID == ScoreTask.ID && t.CompleteDate != null) == null ? "0" : "1"
                   + "\",\"TState\":\"" + ScoreTask.AuditState
                   + "\",\"TStateName\":\"" + Enum.GetName(typeof(AuditState), ScoreTask.AuditState)
                   + "\",\"FirstAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID) == null ? "" : UserList.FirstOrDefault(t => t.ID == ScoreTask.FirstAuditUserID).RealName
                   + "\",\"LastAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID) == null ? "" : UserList.FirstOrDefault(t => t.ID == ScoreTask.LastAuditUserID).RealName
                   + "\",\"FirstAduitDate\":\"" + ScoreTask.FirstAuditDate == null ? "" : ScoreTask.FirstAuditDate.Value.ToString("yyyy-MM-dd")
                   + "\",\"LastAduitDate\":\"" + ScoreTask.LastAuditDate == null ? "" : ScoreTask.LastAuditDate.Value.ToString("yyyy-MM-dd")
                   + "\",\"FirstAduitMess\":\"" + ScoreTask.FirstAuditMark
                   + "\",\"LastAduitMess\":\"" + ScoreTask.LastAuditMark
                   + "\",\"EndDate\":\"" + ScoreTask.EndDate.ToString("yyyy-MM-dd") + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void TaskAdd(HttpContext context)
        {
            try
            {
                string TaskName = context.Request.Params["TaskName"];
                DateTime EndDate = Convert.ToDateTime(context.Request.Params["EndDate"]);
                int TScore = Convert.ToInt32(context.Request.Params["TScore"]);
                int SignScore = Convert.ToInt32(context.Request.Params["SignScore"]);
                int FirstAduitUser = Convert.ToInt32(context.Request.Params["FirstAduitUser"]);
                int LastAduitUser = Convert.ToInt32(context.Request.Params["LastAduitUser"]);
                string TaskContent = context.Request.Params["TaskContent"];
                int UserID = Convert.ToInt32(context.Request.Params["UserID"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {

                    ScoreTask ScoreTask = new ScoreTask();
                    ScoreTask.AuditState = AuditState.待初审;
                    ScoreTask.CompleteBScore = TScore;
                    ScoreTask.Content = TaskContent;
                    ScoreTask.CreateDate = DateTime.Now;
                    ScoreTask.CreateUserID = UserID;
                    ScoreTask.EndDate = EndDate;
                    ScoreTask.FirstAuditUserID = FirstAduitUser;
                    ScoreTask.LastAuditUserID = LastAduitUser;
                    ScoreTask.IsDel = false;
                    ScoreTask.Name = TaskName;
                    ScoreTask.SignBScore = SignScore;
                    db.ScoreTask.Add(ScoreTask);
                    db.SaveChanges();
                    sb.Append("{\"result\":\"true\"}");
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

        public void GetTicket(HttpContext context)
        {
            int UID = Convert.ToInt32(context.Request.Params["UserID"]);
            int PageSize = Convert.ToInt32(context.Request.Params["pagesize"]);
            int PageIndex = Convert.ToInt32(context.Request.Params["pageindex"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.UserID == UID && db.Score.FirstOrDefault(m => m.ID == t.ScoreID && m.IsReward == true && m.AuditState == AuditState.通过 && m.IsDel != true) != null && t.IsPrint == true && t.IsDel != true).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                if (ScoreUserList.Count > 0)
                {
                    List<Score> ScoreList = db.Score.Where(t => t.AuditState == AuditState.通过 && t.IsDel != true).ToList();
                    List<ScoreEvent> ScoreEventList = db.ScoreEvent.Where(t => t.IsDel != true).ToList();
                    List<User> UserList = db.User.ToList();
                    string name = "";
                    foreach (ScoreUser ScoreUser in ScoreUserList)
                    {
                        name += "{\"EventName\":\"" + ScoreEventList.FirstOrDefault(t => t.ID == ScoreList.FirstOrDefault(m => m.ID == ScoreUser.ScoreID).ScoreEventID).Name + "\",\"BSCore\":\"" + ScoreUser.BScore
                       + "\",\"FirstAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreList.FirstOrDefault(m => m.ID == ScoreUser.ScoreID).FirstAuditUserID).RealName
                       + "\",\"LastAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreList.FirstOrDefault(m => m.ID == ScoreUser.ScoreID).LastAuditUserID).RealName
                       + "\",\"VUserName\":\"" + UserList.FirstOrDefault(t => t.ID == ScoreList.FirstOrDefault(m => m.ID == ScoreUser.ScoreID).CreateUserID).RealName
                       + "\",\"VDate\":\"" + ScoreList.FirstOrDefault(m => m.ID == ScoreUser.ScoreID).CreateDate.Value.ToString("yyyy-MM-dd")
                       + "\",\"PrintState\":\"" + (ScoreUser.IsPrint ? "1" : "0") + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }

        public void GetFirstorLastEvent(HttpContext context)
        {
            int type = Convert.ToInt32(context.Request.Params["type"]);
            int pid = Convert.ToInt32(context.Request.Params["pid"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<ScoreEventType> ScoreEventTypeList = db.ScoreEventType.Where(t => t.IsDel != true && t.ParentID == pid).ToList();
                if (ScoreEventTypeList.Count > 0)
                {
                    string name = "";
                    foreach (ScoreEventType ScoreEventType in ScoreEventTypeList)
                    {
                        name += "{\"SDID\":\"" + ScoreEventType.ID + "\",\"DataName\":\"" + ScoreEventType.Name + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }

            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetEventName(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<ScoreEventType> ScoreEventTypeList = db.ScoreEventType.Where(t => t.IsDel != true).ToList();
                if (ScoreEventTypeList.Count > 0)
                {
                    string name = "";
                    foreach (ScoreEventType ScoreEventType in ScoreEventTypeList)
                    {
                        name += "{\"SDID\":\"" + ScoreEventType.ID + "\",\"DataName\":\"" + ScoreEventType.Name + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void RegistraAdd(HttpContext context)
        {
            try
            {
                string ImgUrl = context.Request.Params["ImgUrl"];
                int UID = Convert.ToInt32(context.Request.Params["UserID"]);
                string STitle = context.Request.Params["STitle"];
                string EventName = context.Request.Params["EventName"];
                int EventMark = Convert.ToInt32(context.Request.Params["EventMark"]);
                int FirstAduitUser = Convert.ToInt32(context.Request.Params["FirstAduitUser"]);
                int LastAduitUser = Convert.ToInt32(context.Request.Params["LastAduitUser"]);
                string begin = context.Request.Params["begin"];
                string UIDS = context.Request.Params["UIDS"];
                string Scores = context.Request.Params["Scores"];

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    string Image = ImgUrl;
                    string ImagePath = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    string ResourceImagePath = HttpContext.Current.Server.MapPath("..\\Templete\\");
                    Image = Image.Replace(Image.Split(',')[0] + ",", "");

                    if (!Directory.Exists(ResourceImagePath))
                    {
                        Directory.CreateDirectory(ResourceImagePath);
                    }
                    byte[] bytes = Convert.FromBase64String(Image);
                    MemoryStream memStream = new MemoryStream(bytes);
                    Bitmap bitmap = new Bitmap(memStream);

                    bitmap.Save(ResourceImagePath + ImagePath, ImageFormat.Jpeg);
                    ImagePath = "Templete/" + ImagePath;

                    Score Score = new Score();
                    Score.AuditState = AuditState.待初审;
                    Score.Content = "";
                    Score.CreateDate = DateTime.Now;
                    Score.CreateUserID = UID;
                    Score.FirstAuditUserID = FirstAduitUser;
                    Score.LastAuditUserID = LastAduitUser;
                    Score.IsDel = false;
                    Score.Image = ImagePath;
                    Score.IsReward = true;
                    Score.ScoreEventID = EventMark;
                    Score.Title = STitle;
                    db.Score.Add(Score);
                    db.SaveChanges();


                    for (int i = 0; i < UIDS.Split(',').Length; i++)
                    {
                        int result = 0;
                        if (string.IsNullOrEmpty(UIDS.Split(',')[i]) || !int.TryParse(UIDS.Split(',')[i], out result))
                            continue;
                        ScoreUser ScoreUser = new ScoreUser();
                        ScoreUser.AScore = 0;
                        ScoreUser.BScore = Convert.ToInt32(Scores.Split(',')[i]);
                        ScoreUser.IsDel = false;
                        ScoreUser.IsPrint = false;
                        ScoreUser.ScoreID = Score.ID;
                        ScoreUser.UserID = result;
                        db.ScoreUser.Add(ScoreUser);
                    }

                    db.SaveChanges();

                    Notice Notice = new Notice();
                    Notice.Contenet = "当前有一条奖扣记录需要审核|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                + "分)"
                                + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                + "|状态：待初审";
                    Notice.IsSend = false;
                    Notice.NoticeType = NoticeType.积分制消息;
                    Notice.ReciveUserID = Score.FirstAuditUserID.Value;
                    Notice.SendUserID = UID;
                    Notice.SourceID = Score.ID;
                    Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=1&id=" + Score.ID;
                    db.Notice.Add(Notice);
                    db.SaveChanges();

                    foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                    {
                        Notice = new Notice();
                        Notice.Contenet = "您有一条奖扣记录需要审核|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                          + "分)"
                          + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                          + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                          + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                          + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                          + "|状态：待初审";
                        Notice.IsSend = false;
                        Notice.NoticeType = NoticeType.积分制消息;
                        Notice.ReciveUserID = ScoreUser.UserID;
                        Notice.SendUserID = UID;
                        Notice.SourceID = Score.ID;
                        Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                        db.Notice.Add(Notice);
                    }
                    db.SaveChanges();

                }
                sb.Append("{\"result\":\"true\"}");

            }
            catch
            {
                sb.Append("{\"result\":\"false\"}");
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetBuckleRegistrationAudit(HttpContext context)
        {
            int SID = Convert.ToInt32(context.Request.Params["SID"]);
            int UID = Convert.ToInt32(context.Request.Params["UserID"]);
            int AduitState = Convert.ToInt32(context.Request.Params["AduitState"]);
            string AduitMark = context.Request.Params["AduitMark"];

            using (IFMPDBContext db = new IFMPDBContext())
            {
                Score Score = db.Score.FirstOrDefault(t => t.ID == SID);
                if (Score == null)
                {
                    Notice Notice = new Notice();
                    if (Score.FirstAuditUserID == UID && Score.AuditState == AuditState.待初审)
                    {
                        if (AduitState == 3)
                        {
                            Score.AuditState = AuditState.待终审;                            
                            Notice.Contenet = "当前有一条奖扣记录需要审核|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                + "分)"
                                + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                + "|状态：待终审";
                            Notice.IsSend = false;
                            Notice.NoticeType = NoticeType.积分制消息;
                            Notice.ReciveUserID = Score.LastAuditUserID.Value;
                            Notice.SendUserID = UID;
                            Notice.SourceID = Score.ID;
                            Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=1&id=" + Score.ID;
                            db.Notice.Add(Notice);

                            //给每个人发一个
                            foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                            {
                                Notice = new Notice();
                                Notice.Contenet = "您有一条奖扣记录正在审核中|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                  + "分)"
                                  + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                  + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                  + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                  + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                  + "|状态：待终审";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = ScoreUser.UserID;
                                Notice.SendUserID = UID;
                                Notice.SourceID = Score.ID;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                                db.Notice.Add(Notice);
                            }

                        }
                        else
                        {
                            Score.AuditState = AuditState.驳回;
                            foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                            {
                                Notice = new Notice();
                                Notice.Contenet = "您有一条奖扣记录被驳回|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                  + "分)"
                                  + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                  + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                  + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                  + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                  + "|状态：驳回";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = ScoreUser.UserID;
                                Notice.SendUserID = UID;
                                Notice.SourceID = Score.ID;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                                db.Notice.Add(Notice);
                            }
                        }
                        Score.FirstAuditMark = AduitMark;
                        Score.FirstAuditDate = DateTime.Now;
                    }

                    if (Score.LastAuditUserID == UID && Score.AuditState == AuditState.待终审)
                    {
                        if (AduitState == 3)
                        {
                            Score.AuditState = AuditState.通过;                            
                            //给每个人发一个
                            foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                            {
                                Notice = new Notice();
                                Notice.Contenet = "您有一条奖扣记录已审核通过|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                  + "分)"
                                  + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                  + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                  + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                  + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                  + "|状态：通过";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = ScoreUser.UserID;
                                Notice.SendUserID = UID;
                                Notice.SourceID = Score.ID;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                                db.Notice.Add(Notice);
                            }
                        }
                        else
                        {
                            Score.AuditState = AuditState.驳回;
                            foreach (ScoreUser ScoreUser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList())
                            {
                                Notice = new Notice();
                                Notice.Contenet = "您有一条奖扣记录被驳回|主题：" + Score.Title + "(" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).BScore
                                  + "分)"
                                  + "|记录人：" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                                  + "|参与人：" + new ScoreUserDAO().GetScoreUser(Score.ID)
                                  + "|初审人：" + db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName
                                  + "|终审人：" + db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName
                                  + "|状态：驳回";
                                Notice.IsSend = false;
                                Notice.NoticeType = NoticeType.积分制消息;
                                Notice.ReciveUserID = ScoreUser.UserID;
                                Notice.SendUserID = UID;
                                Notice.SourceID = Score.ID;
                                Notice.URL = ParaUtils.SiteURL + "/jfz/app/RegistrationDetail.html?flag=2&id=" + Score.ID;
                                db.Notice.Add(Notice);
                            }
                        }
                        Score.LastAuditMark = AduitMark;
                        Score.LastAuditDate = DateTime.Now;
                    }
                    db.SaveChanges();
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetBuckleRegistrationBySID(HttpContext context)
        {
            int SID = Convert.ToInt32(context.Request.Params["SID"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                Score Score = db.Score.FirstOrDefault(t => t.ID == SID && t.IsDel != true);
                if (Score == null)
                {
                    string name = "";
                    string img = "";
                    if (Score.Image.ToString() == "")
                    {
                        img = "";
                    }
                    else
                    {
                        if (Score.Image.ToString().Substring(0, 8) == "Templete")
                        {
                            img = "../" + Score.Image.Replace("\\", "/");
                        }
                        else
                        {
                            img = Score.Image.Replace("\\", "/");
                        }
                    }
                    name += "{\"SID\":\"" + Score.ID
                        + "\",\"VUserName\":\"" + db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName + "\",\"VDate\":\"" + Score.CreateDate.Value.ToString("yyyy-MM-dd") + "\",\"STitle\":\"" + Score.Title + "\",\"EventName\":\"" + db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID).Name + "\",\"SImage\":\"" + img + "\"},";
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetAuditState(HttpContext context)
        {
            string name = "";
            foreach (int item in Enum.GetValues(typeof(AuditState)))
            {
                name += "{\"value\":\"" + item + "\",\"text\":\"" + Enum.GetName(typeof(AuditState), item) + "\"},";
            }

            sb.Append("{\"result\":\"true\",\"data\":[");
            sb.Append(name.TrimEnd(','));
            sb.Append("]}");

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetAuditLastUser(HttpContext context)
        {
            int events = Convert.ToInt32(context.Request.Params["events"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                ScoreEvent ScoreEvent = db.ScoreEvent.FirstOrDefault(t => t.ID == events);
                if (ScoreEvent != null)
                {
                    string name = "{\"lastaduituser\":\"" + db.User.FirstOrDefault(t => t.ID == ScoreEvent.LastAuditUserID.Value).RealName + "\"}";
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }


        public void GetUser(HttpContext context)
        {
            //string depid = context.Request.Params["dep"];
            //DataTable dt = SysUserBLL.GetTableByDepIds(depid);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> UserList = db.User.Where(t => t.IsDel != true).ToList();
                List<DepartmentUser> DepartmentUserList = db.DepartmentUser.ToList();
                List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();
                string name = "";
                foreach (User User in UserList)
                {
                    Department Department = DepartmentList.FirstOrDefault(t => t.IsDel != true && t.IsAdmin && DepartmentUserList.Where(m => m.UserID == User.ID).Select(m => m.DepartmentID).Contains(t.ID));
                    //DepartmentUser DepartmentUser = DepartmentUserList.FirstOrDefault(t => t.UserID == User.ID);
                    if (Department != null)
                    {
                        name += "{\"SysID\":\"" + User.ID
                            + "\",\"Realname\":\"" + User.RealName
                            + "\",\"DepID\":\"" + Department.ID
                            + "\",\"DepName\":\"" + Department.Name + "\"},";
                    }
                }
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(name.TrimEnd(','));
                sb.Append("]}");
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }

        public void GetDep(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();
                if (DepartmentList.Count > 0)
                {
                    string name = "";
                    foreach (Department Department in DepartmentList)
                    {
                        name += "{\"DepID\":\"" + Department.ID + "\",\"DepName\":\"" + Department.Name + "\"},";
                    }
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #region 获取初审人
        public void GetAuditUser(HttpContext context)
        {
            int DepIDs = Convert.ToInt32(context.Request.Params["DepIDs"]);
            int UserID = Convert.ToInt32(context.Request.Params["UserID"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> UserList = db.User.Where(t => db.ScoreAuditUser.Where(m => m.ScoreAuditUserType == ScoreAuditUserType.初审人).Select(m => m.UserID).Contains(t.ID) && t.IsDel != true).ToList();

                //DepartmentUser DepartmentUser = db.DepartmentUser.FirstOrDefault(t => t.UserID == UserID);
                Department Department = db.Department.FirstOrDefault(t => db.DepartmentUser.Where(m => m.UserID == UserID).Select(m => m.DepartmentID).Contains(t.ID) && t.IsDel != true && t.IsAdmin);

                string name = "";
                foreach (User User in UserList)
                {
                    name += "{\"value\":\"" + User.ID + "\",\"text\":\"" + User.RealName + "\",\"IsTrue\":\"" + (Department.MasterUserID == UserID ? "1" : "0") + "\"},";
                }
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(name.TrimEnd(','));
                sb.Append("]}");
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion


        #region 获取终审人
        public void GetLastUser(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> UserList = db.User.Where(t => db.ScoreAuditUser.Where(m => m.ScoreAuditUserType == ScoreAuditUserType.终审人).Select(m => m.UserID).Contains(t.ID) && t.IsDel != true).ToList();

                string name = "";
                foreach (User User in UserList)
                {
                    name += "{\"value\":\"" + User.ID + "\",\"text\":\"" + User.RealName + "\"},";
                }
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(name.TrimEnd(','));
                sb.Append("]}");
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}