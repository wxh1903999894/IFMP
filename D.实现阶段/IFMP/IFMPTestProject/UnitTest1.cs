using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using IFMPLibrary.DAO;
using IFMPLibrary.DBContext;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.Utils;
using System.Collections.Generic;

namespace IFMPTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int BaseClass = 5;
                    int adddays = 1;
                    ClassTypeEnums ClassType = ClassTypeEnums.早班;
                    List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();
                    List<Flow> FlowList = db.Flow.ToList();
                    FlowList = FlowList.Where(t => TableTypeList.Select(m => m.ID).Contains(t.TableTypeID)).ToList();

                    List<BaseClassUser> BaseClassUserList = db.BaseClassUser.ToList();
                    List<BaseDateFlow> BaseDateFlowList = db.BaseDateFlow.ToList();
                    //TASK 添加
                    foreach (TableType TableType in TableTypeList)
                    {
                        Task Task = new Task();
                        Task.ClassType = ClassType;
                        Task.CreateDate = DateTime.Now;
                        Task.CreateUserID = 290;
                        Task.IsDel = false;
                        Task.TableTypeID = TableType.ID;
                        Task.TaskName = DateTime.Now.AddDays(adddays).ToString("MM-dd") + Enum.GetName(typeof(ClassTypeEnums), Task.ClassType) + TableType.Name;
                        db.Task.Add(Task);
                        db.SaveChanges();
                        List<Flow> SelFlowList = FlowList.Where(t => t.TableTypeID == TableType.ID).ToList();
                        foreach (Flow SelFlow in SelFlowList)
                        {
                            List<BaseClassUser> SelBaseClassUserList = BaseClassUserList.Where(t => t.FlowID == SelFlow.ID && t.BaseClassID == BaseClass).ToList();
                            BaseDateFlow BaseDateFlow = BaseDateFlowList.FirstOrDefault(t => t.ClassType == ClassType && t.FlowID == SelFlow.ID);
                            foreach (BaseClassUser SelBaseClassUser in SelBaseClassUserList)
                            {
                                TaskFlow TaskFlow = new TaskFlow();
                                //TaskFlow.
                                TaskFlow.ApplyType = ApplyTypeEnums.未交;
                                TaskFlow.BaseClassID = SelBaseClassUser.BaseClassID;
                                TaskFlow.BeginDate = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(adddays), BaseDateFlow.BeginDate);
                                TaskFlow.EndDate = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(adddays), BaseDateFlow.EndDate);
                                if (TaskFlow.EndDate < TaskFlow.BeginDate)
                                    TaskFlow.EndDate = TaskFlow.EndDate.AddDays(1);
                                TaskFlow.FlowID = SelFlow.ID;
                                TaskFlow.IsReminded = false;
                                TaskFlow.TaskID = Task.ID;
                                TaskFlow.RemindDate = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(adddays), BaseDateFlow.RemindDate);
                                if (TaskFlow.RemindDate < TaskFlow.BeginDate)
                                    TaskFlow.RemindDate = TaskFlow.RemindDate.AddDays(1);
                                TaskFlow.UserID = SelBaseClassUser.UserID;
                                db.TaskFlow.Add(TaskFlow);
                            }

                        }
                        db.SaveChanges();

                    }



                    //List<User> UserList = db.User.Where(t=>);



                    //BaseUtils test = new BaseUtils();
                    //bool result = test.GetRegex("23.7°", RegexType.度数);
                    //result = test.GetRegex("23.7", RegexType.度数);
                    //result = test.GetRegex("我°", RegexType.度数);
                    //result = test.GetRegex("°", RegexType.度数);
                    //result = test.GetRegex("23.7°°", RegexType.度数);

                    //List<BaseDateFlow> BaseDateFlowList = db.BaseDateFlow.Where(t => t.FlowID > 107 && t.FlowID < 118).ToList();
                    //foreach (BaseDateFlow BaseDateFlow in BaseDateFlowList)
                    //{
                    //    BaseDateFlow newdata = new BaseDateFlow();
                    //    BaseUtils.DeepCopy(newdata, BaseDateFlow);
                    //    if (BaseDateFlow.FlowID != 117)
                    //    {
                    //        newdata.FlowID = (newdata.FlowID + 24) > 137 ? (newdata.FlowID + 25) : (newdata.FlowID + 24);
                    //    }
                    //    else
                    //    {
                    //        newdata.FlowID = 140;
                    //    }

                    //    newdata.TableTypeID = 20;
                    //    db.BaseDateFlow.Add(newdata);
                    //}
                    //db.SaveChanges();


                    //List<TableColumn> TableColumnList = db.TableColumn.Where(t => t.TableTypeID == 4).ToList();

                    //foreach (TableColumn TableColumn in TableColumnList)
                    //{
                    //    TableColumn NewTableColumn = new TableColumn();
                    //    BaseUtils.DeepCopy(NewTableColumn, TableColumn);
                    //    NewTableColumn.TableTypeID = 23;
                    //    db.TableColumn.Add(NewTableColumn);
                    //}

                    //db.SaveChanges();


                    //List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == 17).ToList();

                    //foreach (Flow Flow in FlowList)
                    //{
                    //    Flow NewFlow = new Flow();
                    //    BaseUtils.DeepCopy(NewFlow, Flow);
                    //    NewFlow.TableTypeID = 20;
                    //    db.Flow.Add(NewFlow);
                    //}

                    //db.SaveChanges();

                    //flow 43-47删除

                    //DDUtils test = new DDUtils();
                    //test.NewPostFile();
                    //TaskNotice();
                }


            }
            catch
            {

            }

        }

        private void TaskNotice()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                try
                {
                    List<TaskFlow> TaskFlowList = db.TaskFlow.Where(t => !t.IsReminded && t.ApplyType == ApplyTypeEnums.未交 && t.RemindDate < DateTime.Now).ToList();
                    List<Flow> FlowList = db.Flow.ToList();
                    List<TableType> TableTypeList = db.TableType.ToList();
                    List<User> UserList = db.User.ToList();
                    //发出消息
                    int senduser = UserList.FirstOrDefault(t => t.UserName == "admin").ID;
                    foreach (TaskFlow TaskFlow in TaskFlowList)
                    {
                        User User = UserList.FirstOrDefault(t => t.ID == TaskFlow.UserID);

                        if (User.DDID == "085146610134609723")
                        {
                            TaskFlow.IsReminded = true;
                            Flow Flow = FlowList.FirstOrDefault(t => t.ID == TaskFlow.FlowID);
                            TableType TableType = db.TableType.FirstOrDefault(t => t.ID == Flow.TableTypeID);
                            //AddNotice("您需要在【" + (Flow.IsAudit ? "审核" : "填写") + "】的【" + TableTypeList.FirstOrDefault(t => t.ID == Flow.TableTypeID).Name
                            //    + "】表单还未【" + (Flow.IsAudit ? "审核" : "填写") + "】，请在【" + TaskFlow.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "】前完成【" + (Flow.IsAudit ? "审核" : "填写") + "】", TaskFlow.UserID, senduser, NoticeType.工作通知);

                            new NoticeDAO().SendDDRemindNotice(
                                User.DDID,
                                "",
                                "",
                                "提醒",
                                TableType.Name + "需要" + (Flow.IsAudit ? "审核" : "填写"),
                                "请及时" + (Flow.IsAudit ? "审核" : "填写") + "相关信息",
                                new NoticeDAO().BuildRemindFormList(TableType.Name, Flow.Name, TaskFlow.EndDate));

                        }

                    }


                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //AddNotice("系统服务的工作提醒存在问题:" + ex.Message, db.User.FirstOrDefault(t => t.UserName == "admin").ID, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
                }
            }

        }

    }
}
