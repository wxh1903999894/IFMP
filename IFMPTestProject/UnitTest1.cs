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
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace IFMPTestProject
{
    [TestClass]
    public class UnitTest1
    {
        public class TempSpotProblem
        {
            public int SpId { get; set; }

            /// <summary>
            /// 点检ID
            /// </summary>
            public int SpotId { get; set; }

            /// <summary>
            /// 宿舍问题描述
            /// </summary>
            public string ProDesc { get; set; }

            /// <summary>
            /// 问题责任人
            /// </summary>
            public string DutyUser { get; set; }

            /// <summary>
            /// 点检人
            /// </summary>
            public string CreateUser { get; set; }

            /// <summary>
            /// 点检日期
            /// </summary>
            public Nullable<DateTime> CreateDate { get; set; }

            /// <summary>
            /// 复查人员
            /// </summary>
            public string ReviewUser { get; set; }

            /// <summary>
            /// 复查意见
            /// </summary>
            public string ReviewMemo { get; set; }

            /// <summary>
            /// 复查日期
            /// </summary>
            public Nullable<DateTime> ReviewDate { get; set; }

            /// <summary>
            /// 是否复查
            /// </summary>
            public Nullable<bool> IsreView { get; set; }

            /// <summary>
            /// 图片
            /// </summary>
            public string SImage { get; set; }
        }



        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    
                    var UserList = db.User.Where(t => t.ID > 0);
                    //var sql = ((System.Data.Entity.Infrastructure.DbQuery<object>)db.User.Where(t => t.ID > 0)).ToString();
                    //string FullUser = string.Join(",", db.Scheduling.Select(t => t.CheckName));

                    //int WeekDay = (int)DateTime.Now.DayOfWeek;
                    //List<Scheduling> SchedulingList = db.Scheduling.Where(t => t.Date == (WeekDate)WeekDay).ToList();

                    //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    //stopwatch.Start(); 

                    ////List<User> UserList = db.User.ToList();
                    //List<SpotCheck> SpotCheckList = db.SpotCheck.ToList();
                    //foreach (SpotCheck SpotCheck in SpotCheckList)
                    //{
                    //    int UserID = Convert.ToInt32(SpotCheck.CreateUser);
                    //    string name = db.User.FirstOrDefault(t => t.ID == UserID).RealName;
                    //}
                    //stopwatch.Stop(); //  停止监视
                    //TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
                    //double Milliseconds = timespan.TotalMilliseconds;


                    int k = 0;

                    //new DDUtils().GetUserByCode("23321a3ed0033c4c9b00f463afff5d88");
                    //int day = (int)DateTime.Now.DayOfWeek;
                    //DateTime BeginDate=Convert.ToDateTime("2019-04-01 00:00:00");
                    //List<TableData> TableData = db.TableData.Where(t => t.CreateDate > BeginDate).ToList();
                    //TableData.ForEach(t => t.CreateDate = db.Table.FirstOrDefault(m => m.ID == t.TableID).CreateDate);
                    //db.SaveChanges();

                    //修改权限的部分,这个还没用
                    //List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID > 23 && t.Name.Contains("班长")).ToList();
                    //List<BaseFlowRole> BaseFlowRoleList = db.BaseFlowRole.Where(t => db.Flow.Where(m => m.TableTypeID > 23 && m.Name.Contains("班长")).Select(m => m.ID).Contains(t.FlowID)).ToList();

                    //foreach (BaseFlowRole BaseFlowRole in BaseFlowRoleList)
                    //{
                    //    BaseFlowRole.RoleID = 16;
                    //    BaseFlowRole NewBaseFlowRole = new BaseFlowRole();
                    //    NewBaseFlowRole.RoleID = 17;
                    //    NewBaseFlowRole.FlowID = BaseFlowRole.FlowID;
                    //    db.BaseFlowRole.Add(NewBaseFlowRole);
                    //    db.SaveChanges();
                    //}



                    //剩下部分的流程






                    //DirectoryInfo Dir = new DirectoryInfo(@"C:\Users\rjb-01\Desktop\电子水泵C版点检指导书\NewC");


                    //DirectoryInfo[] DirSub = Dir.GetDirectories();

                    //if (DirSub.Length <= 0)
                    //{
                    //    foreach (FileInfo f in Dir.GetFiles("*.*", SearchOption.TopDirectoryOnly)) //查找文件
                    //    {

                    //        //listBox1.Items.Add(Dir+f.ToString()); //listBox1中填加文件名
                    //        string name = f.Name;
                    //        ResourceData ResourceData = new ResourceData();
                    //        ResourceData.CreateDate = DateTime.Now;
                    //        ResourceData.IsDel = false;
                    //        ResourceData.IsCarousel = false;
                    //        ResourceData.ResourcePathID = 20;
                    //        ResourceData.Name = name.Replace("(1)", "");
                    //        db.ResourceData.Add(ResourceData);
                    //    }
                    //    db.SaveChanges();
                    //}


                    //List<BaseDateFlow> BaseDateFlowList = db.BaseDateFlow.Where(t => t.TableTypeID == 12 && t.ClassType == ClassTypeEnums.早班).ToList();
                    //foreach (BaseDateFlow BaseDateFlow in BaseDateFlowList)
                    //{
                    //    BaseDateFlow New = new BaseDateFlow();
                    //    New.BeginDate = BaseDateFlow.BeginDate;
                    //    New.ClassType = ClassTypeEnums.早班;
                    //    New.EndDate = BaseDateFlow.EndDate;
                    //    New.FlowID = 0;
                    //    New.TableTypeID = 27;
                    //    New.RemindDate = BaseDateFlow.RemindDate;
                    //    New.Name = BaseDateFlow.Name;
                    //    db.BaseDateFlow.Add(New);
                    //}
                    //db.SaveChanges();

                    //List<Flow> FlowList = db.Flow.Where(t => t.ID >= 262).ToList();
                    //string[] BeginList = new string[] { "2019-04-10 07:10:00", "2019-04-10 07:10:00", "2019-04-10 08:00:00", "2019-04-10 08:00:00" };
                    //string[] EndList = new string[] { "2019-04-10 08:20:00", "2019-04-10 08:20:00", "2019-04-10 09:00:00", "2019-04-10 17:00:00" };
                    //string[] RemindList = new string[] { "2019-04-10 08:10:00", "2019-04-10 08:10:00", "2019-04-10 08:50:00", "2019-04-10 16:50:00" };
                    //for (int i = 0; i < FlowList.Count; i++)
                    //{
                    //    BaseDateFlow BaseDateFlow = new BaseDateFlow();
                    //    BaseDateFlow.BeginDate = Convert.ToDateTime(BeginList[i % 4]);
                    //    BaseDateFlow.EndDate = Convert.ToDateTime(EndList[i % 4]);
                    //    BaseDateFlow.RemindDate = Convert.ToDateTime(RemindList[i % 4]);
                    //    BaseDateFlow.ClassType = ClassTypeEnums.早班;
                    //    BaseDateFlow.FlowID = FlowList[i].ID;
                    //    BaseDateFlow.Name = FlowList[i].Name + ":早班";
                    //    BaseDateFlow.TableTypeID = FlowList[i].TableTypeID;
                    //    db.BaseDateFlow.Add(BaseDateFlow);
                    //}
                    //db.SaveChanges();


                    //List<Flow> FlowList = db.Flow.Where(t => t.ID >= 262).ToList();
                    //int[] RoleList = new int[] { 15, 9, 5, 10 };
                    //for (int i = 0; i < FlowList.Count; i++)
                    //{
                    //    BaseFlowRole BaseFlowRole = new IFMPLibrary.Entities.BaseFlowRole();
                    //    BaseFlowRole.FlowID = FlowList[i].ID;
                    //    BaseFlowRole.RoleID = RoleList[i % 4];
                    //    db.BaseFlowRole.Add(BaseFlowRole);
                    //}
                    //db.SaveChanges();

                    //List<int> Type1 = new List<int> { 24, 25, 27, 55 };

                    //List<TableType> TableTypeList = db.TableType.Where(t => t.ID > 55).ToList();
                    //foreach (TableType TableType in TableTypeList)
                    //{
                    //    if (TableType.ID != 27 && TableType.ID != 55)
                    //    {
                    //        Flow Flow1 = new Flow();
                    //        Flow1.ParentID = 0;
                    //        Flow1.IsAudit = false;
                    //        Flow1.TableTypeID = TableType.ID;
                    //        Flow1.Name = "填写" + TableType.Name;
                    //        db.Flow.Add(Flow1);
                    //        db.SaveChanges();

                    //        Flow Flow2 = new Flow();
                    //        Flow2.ParentID = Flow1.ID;
                    //        Flow2.IsAudit = true;
                    //        Flow2.TableTypeID = TableType.ID;
                    //        Flow2.Name = "班长审核";
                    //        db.Flow.Add(Flow2);
                    //        db.SaveChanges();

                    //        Flow Flow3 = new Flow();
                    //        Flow3.ParentID = Flow2.ID;
                    //        Flow3.IsAudit = true;
                    //        Flow3.TableTypeID = TableType.ID;
                    //        Flow3.Name = "车间主任审核";
                    //        db.Flow.Add(Flow3);
                    //        db.SaveChanges();

                    //        Flow Flow4 = new Flow();
                    //        Flow4.ParentID = Flow3.ID;
                    //        Flow4.IsAudit = true;
                    //        Flow4.TableTypeID = TableType.ID;
                    //        Flow4.Name = "生产设备部部长审核";
                    //        db.Flow.Add(Flow4);
                    //        db.SaveChanges();
                    //    }
                    //}


                    //foreach (TableColumn TableColumn in TableColumnList)
                    //{
                    //    TableColumn New = new TableColumn();
                    //    New.ColumnName = TableColumn.ColumnName;
                    //    New.DefaultData = TableColumn.DefaultData;
                    //    New.DictionaryID = TableColumn.DictionaryID;
                    //    New.HintDictionaryID = TableColumn.HintDictionaryID;
                    //    New.IsFill = true;
                    //    New.IsStats = false;
                    //    New.Order = TableColumn.Order;
                    //    New.TableTypeID = 25;
                    //    db.TableColumn.Add(New);
                    //}
                    //db.SaveChanges();

                    //InsertTableColumnByExcel();
                    //int BaseClass = 5;
                    //int adddays = 1;
                    //ClassTypeEnums ClassType = ClassTypeEnums.早班;
                    //ProductionLine ProductionLine = db.ProductionLine.FirstOrDefault(t => t.ID == 1);
                    //List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true && t.ProductionLineID == ProductionLine.ID).ToList();
                    //List<Flow> FlowList = db.Flow.ToList();
                    //FlowList = FlowList.Where(t => TableTypeList.Select(m => m.ID).Contains(t.TableTypeID)).ToList();

                    //List<BaseClassUser> BaseClassUserList = db.BaseClassUser.ToList();
                    //List<BaseDateFlow> BaseDateFlowList = db.BaseDateFlow.ToList();
                    ////TASK 添加
                    //foreach (TableType TableType in TableTypeList)
                    //{
                    //    Task Task = new Task();
                    //    Task.ClassType = ClassType;
                    //    Task.CreateDate = DateTime.Now;
                    //    Task.CreateUserID = 290;
                    //    Task.IsDel = false;
                    //    Task.TableTypeID = TableType.ID;
                    //    Task.TaskName = DateTime.Now.AddDays(adddays).ToString("MM-dd") + Enum.GetName(typeof(ClassTypeEnums), Task.ClassType) + TableType.Name;
                    //    db.Task.Add(Task);
                    //    db.SaveChanges();
                    //    List<Flow> SelFlowList = FlowList.Where(t => t.TableTypeID == TableType.ID).ToList();
                    //    foreach (Flow SelFlow in SelFlowList)
                    //    {
                    //        List<BaseClassUser> SelBaseClassUserList = BaseClassUserList.Where(t => t.FlowID == SelFlow.ID && t.BaseClassID == BaseClass).ToList();
                    //        BaseDateFlow BaseDateFlow = BaseDateFlowList.FirstOrDefault(t => t.ClassType == ClassType && t.FlowID == SelFlow.ID);
                    //        foreach (BaseClassUser SelBaseClassUser in SelBaseClassUserList)
                    //        {
                    //            TaskFlow TaskFlow = new TaskFlow();
                    //            //TaskFlow.
                    //            TaskFlow.ApplyType = ApplyTypeEnums.未交;
                    //            TaskFlow.BaseClassID = SelBaseClassUser.BaseClassID;
                    //            TaskFlow.BeginDate = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(adddays), BaseDateFlow.BeginDate);
                    //            TaskFlow.EndDate = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(adddays), BaseDateFlow.EndDate);
                    //            TaskFlow.FlowID = SelFlow.ID;
                    //            TaskFlow.IsReminded = false;
                    //            TaskFlow.TaskID = Task.ID;

                    //            TaskFlow.RemindDate = new BaseUtils().GetTodayDate(DateTime.Now.AddDays(adddays), BaseDateFlow.RemindDate);
                    //            TaskFlow.UserID = SelBaseClassUser.UserID;
                    //            db.TaskFlow.Add(TaskFlow);
                    //        }

                    //    }
                    //    db.SaveChanges();

                    //}


                    //string pattern = @"^[A-DF]$";
                    //System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex(pattern);
                    //bool result1 = Regex.Match("A").Success;
                    //bool result2 = Regex.Match("0").Success;
                    //bool result3 = Regex.Match("1").Success;
                    //bool result4 = Regex.Match("AA").Success;
                    //bool result5 = Regex.Match("G").Success;
                    //bool result6 = Regex.Match("好想吃五花肉").Success;

                    //int i = 0;




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



        public void InsertTableColumnByExcel()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                DataTable DT = ReadExcel("d:/AddExcel.xls");
                int i = 3;
                int TableType = 75;

                TableColumn TableColumn = new TableColumn();
                TableColumn.ColumnName = "设备名称";
                TableColumn.DefaultData = "标签打印机";
                TableColumn.Order = 1;
                TableColumn.IsStats = false;
                TableColumn.TableTypeID = TableType;
                TableColumn.IsFill = true;
                db.TableColumn.Add(TableColumn);

                TableColumn = new TableColumn();
                TableColumn.ColumnName = "设备编号";
                TableColumn.DefaultData = "WHFL123-17";
                TableColumn.Order = 2;
                TableColumn.IsStats = false;
                TableColumn.TableTypeID = TableType;
                TableColumn.IsFill = true;
                db.TableColumn.Add(TableColumn);


                foreach (DataRow DR in DT.Rows)
                {
                    if (!string.IsNullOrEmpty(DR[0].ToString()))
                    {
                        TableColumn = new TableColumn();
                        TableColumn.ColumnName = DR[0].ToString() + DR[1].ToString();
                        TableColumn.DictionaryID = 8;
                        TableColumn.DefaultData = "是";
                        TableColumn.Order = i;
                        TableColumn.IsStats = false;
                        TableColumn.TableTypeID = TableType;
                        TableColumn.IsFill = true;
                        db.TableColumn.Add(TableColumn);
                        i++;
                    }

                }
                db.SaveChanges();
            }


        }

        public DataTable ReadExcel(string path)
        {
            DataTable dt = new DataTable();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source='" + path + "';" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            try
            {
                conn.Open();
                //获取表名
                DataTable dtname = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = dtname.Rows[0][2].ToString().Trim();
                //读取excel文件数据
                string strExcel = string.Format("select * from [{0}]", sheetName);
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn);
                myCommand.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            conn.Close();
            //CommonFunction.delfile(path);
            return dt;
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
