using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace IFMP.ashx
{
    /// <summary>
    /// BaseData 的摘要说明
    /// </summary>
    public class BaseData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Params["method"];
            switch (method)
            {
                case "GetTableType":
                    GetTableType(context);
                    break;
                case "GetFlow":
                    GetFlow(context);
                    break;
                case "BuildBaseClassUser":
                    BuildBaseClassUser(context);
                    break;
                case "GetBaseClass":
                    GetBaseClass(context);
                    break;
                case "GetFlowByTableType":
                    GetFlowByTableType(context);
                    break;
                case "GetTableColumn":
                    GetTableColumn(context);
                    break;
                case "GetShiftTask":
                    GetShiftTask(context);
                    break;
                case "ShiftTaskFull":
                    ShiftTaskFull(context);
                    break;
                case "ShiftTaskFlow":
                    ShiftTaskFlow(context);
                    break;
                case "GetAlertMessage":
                    GetAlertMessage(context);
                    break;
                case "GetPic":
                    GetPic(context);
                    break;
                default:
                    break;
            }
        }

        //返回所有的表单类型
        public void GetTableType(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();
                    foreach (TableType TableType in TableTypeList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("ID", TableType.ID);
                        jobject.Add("Name", TableType.Name);
                        jarray.Add(jobject);
                    }

                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
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


        //返回流程，包括初始化的部分
        public void GetFlow(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            int TableTypeID = Convert.ToInt32(context.Request["tabletype"]);
            int BaseClassID = Convert.ToInt32(context.Request["baseclassid"]);
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == TableTypeID).ToList();
                    List<BaseClassUser> BaseClassUserList = db.BaseClassUser.ToList();
                    foreach (Flow Flow in FlowList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("ID", Flow.ID);
                        jobject.Add("Name", Flow.Name);
                        //获取满足的用户
                        List<User> UserList = db.User.Where(t => t.IsDel != true
                   && db.UserRole.Where(l => db.Role.Where(m => m.IsDel != true && (db.BaseFlowRole.Where(k => k.FlowID == Flow.ID).Count() == 0 || db.BaseFlowRole.Where(k => k.FlowID == Flow.ID).Select(k => k.RoleID).Contains(m.ID))).Select(m => m.ID).Contains(l.RoleID)
                   ).Select(l => l.UserID).Contains(t.ID)
                   ).ToList();

                        if (UserList.Count == 0)
                        {
                            UserList = db.User.Where(t => t.IsDel != true).ToList();
                        }

                        JArray userarray = new JArray();
                        foreach (User User in UserList)
                        {
                            //在这里初始化,
                            JObject userobject = new JObject();
                            userobject.Add("ID", User.ID);
                            userobject.Add("Name", User.RealName);

                            if (BaseClassID > 0 && BaseClassUserList.FirstOrDefault(t => t.UserID == User.ID && t.FlowID == Flow.ID && t.BaseClassID == BaseClassID) != null)
                            {
                                userobject.Add("IsSet", true);
                            }
                            else
                            {
                                userobject.Add("IsSet", false);
                            }
                            userarray.Add(userobject);
                        }

                        jobject.Add("User", userarray);

                        jarray.Add(jobject);
                    }

                    returnobj.Add("result", "success");
                    returnobj.Add("Name", db.TableType.FirstOrDefault(t => t.ID == TableTypeID).Name);
                    returnobj.Add("List", jarray);
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


        public void GetFlowByTableType(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            int TableTypeID = Convert.ToInt32(context.Request["tabletype"]);
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == TableTypeID).ToList();
                    FlowList = new FlowDAO().GetFlowLevel(FlowList);
                    foreach (Flow Flow in FlowList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("Name", Flow.Name);
                        jobject.Add("ID", Flow.ID);
                        jobject.Add("ParentID", Flow.ParentID);
                        jobject.Add("Level", Flow.Level);
                        jobject.Add("IsAudit", Flow.IsAudit);
                        jarray.Add(jobject);
                    }
                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                    returnobj.Add("MaxLevel", FlowList.OrderByDescending(t => t.Level).FirstOrDefault().Level);
                    returnobj.Add("TableType", db.TableType.FirstOrDefault(t => t.ID == TableTypeID).Name);
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



        //获取所有基础班次
        public void GetBaseClass(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<BaseClass> BaseClassList = db.BaseClass.Where(t => t.IsDel != true).ToList();

                    foreach (BaseClass BaseClass in BaseClassList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("ID", BaseClass.ID);
                        jobject.Add("Name", BaseClass.Name);
                        jarray.Add(jobject);
                    }

                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
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

        public void BuildBaseClassUser(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                //int TableTypeID = Convert.ToInt32(context.Request["tabletype"]);
                int BaseClassID = Convert.ToInt32(context.Request["baseclassid"]);
                JArray FlowUserArray = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(context.Request["flowuser"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    BaseClass BaseClass = db.BaseClass.FirstOrDefault(t => t.ID == BaseClassID);
                    if (BaseClass != null)
                    {
                        foreach (JObject FlowUserObject in FlowUserArray)
                        {
                            int flowid = Convert.ToInt32(FlowUserObject["ID"]);
                            //删除原有的
                            db.BaseClassUser.RemoveRange(db.BaseClassUser.Where(t => t.BaseClassID == BaseClass.ID && t.FlowID == flowid));

                            foreach (object User in FlowUserObject["UserList"])
                            {
                                BaseClassUser BaseClassUser = new BaseClassUser();
                                BaseClassUser.BaseClassID = BaseClass.ID;
                                BaseClassUser.FlowID = flowid;
                                BaseClassUser.UserID = Convert.ToInt32(User);
                                db.BaseClassUser.Add(BaseClassUser);
                            }
                        }
                        db.SaveChanges();

                        returnobj.Add("result", "success");
                    }
                    else
                    {
                        returnobj.Add("result", "failed");
                    }
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

        public void GetTableColumn(HttpContext context)
        {
            string url = context.Request.UrlReferrer.ToString();
            url = url.Split('&')[0];
            int tabletypeid = Convert.ToInt32(url.Substring(url.IndexOf("type") + 5, url.Length - url.IndexOf("type") - 5));
            //int tabletypeid = Convert.ToInt32(context.Request["type"]);
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<TableColumn> TableColumnList = db.TableColumn.Where(t => t.IsFill == true && t.TableTypeID == tabletypeid).OrderBy(t => t.Order).ToList();
                    foreach (TableColumn TableColumn in TableColumnList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("id", TableColumn.ID);
                        jobject.Add("text", TableColumn.ColumnName);
                        jarray.Add(jobject);
                    }
                }
            }
            catch (Exception error)
            {

            }
            context.Response.Clear();
            context.Response.Write(jarray);
            context.Response.End();
        }



        public void GetShiftTask(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                int TaskID = Convert.ToInt32(context.Request["taskid"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    Task Task = db.Task.FirstOrDefault(t => t.ID == TaskID);
                    //获取这个task的流程和参与的人员（填写）
                    List<TaskFlow> TaskFlowList = db.TaskFlow.Where(t => t.TaskID == Task.ID && t.ApplyDate != null && db.Flow.Where(m => m.IsAudit == false).Select(m => m.ID).Contains(t.FlowID)).ToList();
                    JArray UserArray = new JArray();
                    JArray FlowArray = new JArray();
                    //List<int> UserIDList = new List<int>();
                    List<int> FlowIDList = TaskFlowList.OrderBy(t => t.FlowID).Select(t => t.FlowID).Distinct().ToList();
                    List<User> UserList = db.User.ToList();
                    List<Flow> FlowList = db.Flow.ToList();
                    foreach (int FlowID in FlowIDList)
                    {
                        JObject JObject = new JObject();
                        JObject.Add("FlowID", FlowID);
                        JObject.Add("Name", FlowList.FirstOrDefault(t => t.ID == FlowID).Name);
                        List<TaskFlow> SelTaskFlowList = TaskFlowList.Where(t => t.FlowID == FlowID).ToList();
                        JArray SelUserArray = new JArray();
                        foreach (TaskFlow SelTaskFlow in SelTaskFlowList)
                        {
                            JObject SelUserObject = new JObject();
                            SelUserObject.Add("UserID", SelTaskFlow.UserID);
                            SelUserObject.Add("Name", UserList.FirstOrDefault(t => t.ID == SelTaskFlow.UserID).RealName);
                            if (UserArray.FirstOrDefault(t => Convert.ToInt32(t["UserID"]) == SelTaskFlow.UserID) == null)
                            {
                                UserArray.Add(SelUserObject);
                            }
                            SelUserArray.Add(SelUserObject);
                        }
                        JObject.Add("UserList", SelUserArray);
                        FlowArray.Add(JObject);
                    }

                    returnobj.Add("result", "success");
                    returnobj.Add("Flow", FlowArray);
                    returnobj.Add("User", UserArray);
                    //returnobj.Add("UserFull", UserList);
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

        public void ShiftTaskFull(HttpContext context)
        {
            JObject returnobj = new JObject();
            try
            {
                int TaskID = Convert.ToInt32(context.Request["taskid"]);
                string str = context.Request["olduserlist"];
                JArray OldUserList = (JArray)JsonConvert.DeserializeObject(context.Request["olduserlist"]);
                JArray UserList = (JArray)JsonConvert.DeserializeObject(context.Request["userlist"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        if (UserList[i]["id"].ToString() != OldUserList[i]["id"].ToString())
                        {
                            int OldUserID = Convert.ToInt32(OldUserList[i]["id"]);
                            int UserID = Convert.ToInt32(UserList[i]["id"]);
                            List<Table> TableList = db.Table.Where(t => t.TaskID == TaskID && t.CreateUserID == OldUserID).ToList();
                            List<TableData> TableDataList = db.TableData.Where(t => t.CreateUserID == OldUserID).ToList();
                            TableDataList = TableDataList.Where(t => TableList.Select(m => m.ID).Contains(t.TableID)).ToList();
                            List<TaskFlow> TaskFlowList = db.TaskFlow.Where(t => t.TaskID == TaskID && t.UserID == OldUserID).ToList();

                            TableList = TableList.Select(t => { t.CreateUserID = UserID; return t; }).ToList();
                            TableDataList = TableDataList.Select(t => { t.CreateUserID = UserID; return t; }).ToList();
                            TaskFlowList = TaskFlowList.Select(t => { t.UserID = UserID; return t; }).ToList();
                            db.SaveChanges();
                        }
                    }

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

        public void ShiftTaskFlow(HttpContext context)
        {
            JObject returnobj = new JObject();
            try
            {
                int TaskID = Convert.ToInt32(context.Request["taskid"]);
                string str = context.Request["olduserlist"];
                JArray OldUserList = (JArray)JsonConvert.DeserializeObject(context.Request["olduserlist"]);
                JArray UserList = (JArray)JsonConvert.DeserializeObject(context.Request["userlist"]);
                JArray FlowList = (JArray)JsonConvert.DeserializeObject(context.Request["flowlist"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        if (UserList[i]["id"].ToString() != OldUserList[i]["id"].ToString())
                        {
                            int OldUserID = Convert.ToInt32(OldUserList[i]["id"]);
                            int UserID = Convert.ToInt32(UserList[i]["id"]);
                            int FlowID = Convert.ToInt32(FlowList[i]);
                            List<Table> TableList = db.Table.Where(t => t.TaskID == TaskID && t.FlowID == FlowID && t.CreateUserID == OldUserID).ToList();
                            List<TaskFlow> TaskFlowList = db.TaskFlow.Where(t => t.TaskID == TaskID && t.FlowID == FlowID && t.UserID == OldUserID).ToList();
                            List<TableData> TableDataList = db.TableData.Where(t => t.CreateUserID == OldUserID).ToList();
                            TableDataList = TableDataList.Where(t => TableList.Select(m => m.ID).Contains(t.TableID)).ToList();
                            TableList = TableList.Select(t => { t.CreateUserID = UserID; return t; }).ToList();
                            TableDataList = TableDataList.Select(t => { t.CreateUserID = UserID; return t; }).ToList();
                            TaskFlowList = TaskFlowList.Select(t => { t.UserID = UserID; return t; }).ToList();
                            db.SaveChanges();
                        }
                    }

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

        //获取大屏的报警信息
        public void GetAlertMessage(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    DateTime Date = DateTime.Now.AddMinutes(-1);
                    SysLog log = db.SysLog.FirstOrDefault(t => t.CreateDate > Date && t.LogType == LogType.报警日志);

                    if (log == null)
                    {
                        returnobj.Add("data", null);
                    }
                    else
                    {
                        returnobj.Add("data", log.Message);
                    }
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

        public void GetPic(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    DirectoryInfo Dir = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "resource");
                    DirectoryInfo[] DirSub = Dir.GetDirectories();

                    foreach (FileInfo f in Dir.GetFiles("*.*", SearchOption.TopDirectoryOnly))
                    {
                        JObject jobject = new JObject();
                        jobject.Add("Name", f.Name);
                        jarray.Add(jobject);
                    }
                }
                returnobj.Add("result", "success");
                returnobj.Add("PicList", jarray);
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