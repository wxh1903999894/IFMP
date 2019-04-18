using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeiLongLibrary.DAO;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Utils;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Enums;
using System.Transactions;

namespace FeiLongLibrary.BLL
{
    public class TaskBLL
    {
        public class TaskAdd
        {
            public List<FLTask> TaskList { get; set; }
            public List<BaseClass> BaseClassList { get; set; }
        }
        public ApiResult Add(TaskAdd TaskAdd)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            Action action = () =>
                {
                    using (FLDbContext db = new FLDbContext())
                    {
                        foreach (FLTask Task in TaskAdd.TaskList)
                        {
                            if (string.IsNullOrEmpty(Task.TaskName))
                            {
                                message = "请填写任务名称";
                                goto Response;
                            }

                            Task.CreateDate = DateTime.Now;
                            Task.IsDel = false;

                            db.FLTask.Add(Task);
                            db.SaveChanges();

                            if (Task.IsBaseClass == true)
                            {
                                //使用默认的班次
                                List<TaskFlow> tfList = new List<TaskFlow>();
                                List<Flow> FlowList = new FlowDAO().GetFlow(Task.TableType);

                                foreach (BaseClass bs in TaskAdd.BaseClassList)
                                {
                                    foreach (Flow Flow in FlowList)
                                    {
                                        List<BaseClassUser> bculist = db.BaseClassUser.Where(t => t.BaseClassID == bs.ID && t.FlowID == Flow.ID).ToList();
                                        foreach (BaseClassUser bcu in bculist)
                                        {
                                            foreach (TaskFlow tf in Task.TaskFlowList)
                                            {
                                                //这里可以缓存一个bdf来提升效率
                                                if (tf.IsBase)
                                                {
                                                    BaseDateFlow bdf = db.BaseDateFlow.FirstOrDefault(t => t.FlowID == tf.FlowID);
                                                    tf.BeginDate = bdf.BeginDate;
                                                    tf.EndDate = bdf.EndDate;
                                                    tf.RemindDate = bdf.RemindDate;
                                                }
                                                //tf.FlowID = bcu.FlowID;
                                                tf.MaintainUserID = bcu.UserID;
                                                tf.IsReminded = false;
                                                tf.MaintainUserID = bcu.UserID;
                                                tf.TaskID = Task.ID;
                                                tfList.Add(tf);
                                            }
                                        }
                                    }
                                }

                                db.TaskFlow.AddRange(tfList);
                            }
                            else
                            {
                                //先验证是否完整
                                List<Flow> flowList = new FlowDAO().GetFlow(Task.TableType);
                                foreach (Flow flow in flowList)
                                {
                                    if (Task.TaskFlowList.FirstOrDefault(t => t.FlowID == flow.ID) == null)
                                    {
                                        message = "请填写完整的流程";
                                        goto Response;
                                    }
                                }

                                foreach (TaskFlow tf in Task.TaskFlowList)
                                {
                                    if (tf.IsBase)
                                    {
                                        BaseDateFlow bdf = db.BaseDateFlow.FirstOrDefault(t => t.FlowID == tf.FlowID);
                                        tf.BeginDate = bdf.BeginDate;
                                        tf.EndDate = bdf.EndDate;
                                        tf.RemindDate = bdf.RemindDate;
                                    }
                                    tf.IsReminded = false;
                                    tf.TaskID = Task.ID;
                                }

                                db.TaskFlow.AddRange(Task.TaskFlowList);
                            }
                        }
                    }
                    new SysLogDAO().AddLog(LogType.Success, message: "成功添加任务");
                    result = ApiResult.NewSuccessJson("成功添加任务");

                Response:
                    if (!string.IsNullOrEmpty(message))
                    {
                        //回滚
                        foreach (FLTask Task in TaskAdd.TaskList)
                        {
                            if (Task.ID != 0)
                            {
                                new FLDbContext().FLTask.Remove(new FLDbContext().FLTask.FirstOrDefault(t => t.ID == Task.ID));
                            }
                            else
                            {
                                break;
                            }
                        }
                        result = ApiResult.NewErrorJson(message);

                    }
                };
            TransactioExtension.Excute(action);

            return result;
        }

        public ApiResult GetAll(DateTime? BeginDate = null, DateTime? EndDate = null, int pageindex = 1, int pagesize = 10, ClassTypeEnums? ClassType = null)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                BeginDate = BeginDate ?? Convert.ToDateTime("1900-01-01");
                EndDate = EndDate ?? Convert.ToDateTime("2099-12-31");
                using (FLDbContext db = new FLDbContext())
                {
                    var tasklist = from task in db.FLTask
                                   join taskflow in db.TaskFlow on task.ID equals taskflow.TaskID
                                   where taskflow.BeginDate >= BeginDate
                                   && taskflow.BeginDate <= EndDate
                                   && (ClassType == null || task.ClassType == ClassType)
                                   && taskflow.MaintainUserID == LoginHelper.CurrentUser.ID
                                   select new
                                   {
                                       TaskName = task.TaskName,
                                       TaskID = task.ID,
                                       BeginDate = taskflow.BeginDate,
                                       EndDate = taskflow.EndDate,
                                       RemindDate = taskflow.RemindDate,
                                       IsAudit = taskflow.IsAudit,
                                       ApplyType = taskflow.ApplyType
                                   };
                    int total = tasklist.Count();
                    List<object> returnlist = new List<object>();
                    TaskDAO taskdao = new TaskDAO();
                    foreach (var data in tasklist.OrderByDescending(t => t.EndDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList())
                    {
                        returnlist.Add(new
                        {
                            TaskName = data.TaskName,
                            TaskID = data.TaskID,
                            BeginDate = data.BeginDate.ToString("yyyy-MM-dd HH:mm"),
                            EndDate = data.EndDate.ToString("yyyy-MM-dd HH:mm"),
                            RemindDate = data.RemindDate.ToString("yyyy-MM-dd HH:mm"),
                            Status = taskdao.GetApplyTypeName(data.IsAudit, data.ApplyType)
                        });
                    }

                    result = ApiResult.NewSuccessJson(new
                    {
                        Total = total,
                        List = returnlist
                    });
                }
            }
            catch
            {
                result = ApiResult.NewErrorJson("请检查网络状态或联系系统管理员");
            }
            if (!string.IsNullOrEmpty(message))
            {
                //回滚

                result = ApiResult.NewErrorJson(message);
            }
            return result;
        }
    }
}
