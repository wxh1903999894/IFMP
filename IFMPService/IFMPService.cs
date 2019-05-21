using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using System.Xml;
using System.IO;
using System.Web;
using System.IO.Ports;

namespace IFMPService
{
    public partial class IFMPService : ServiceBase
    {
        public IFMPService()
        {
            InitializeComponent();
        }
        SerialPort mySerialPort = new SerialPort("COM3");
        protected override void OnStart(string[] args)
        {
            //在这里有以下需要做的功能
            //人员档案的提示
            //日常行为的提示
            //发送以上消息

            //人员档案的提示
            System.Timers.Timer UserDetailsHandle = new System.Timers.Timer();
            UserDetailsHandle.Interval = 1000 * 60 * 60 * 24;
            //UserDetailsHandle.Interval = 1000 * 60;

            UserDetailsHandle.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => UserProbation());
            UserDetailsHandle.AutoReset = true;
            UserDetailsHandle.Start();


            //生日的提示
            System.Timers.Timer BirthDateHandle = new System.Timers.Timer();
            BirthDateHandle.Interval = 1000 * 60 * 60 * 24;
            //UserDetailsHandle.Interval = 1000 * 60;

            BirthDateHandle.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => BirthDate());
            BirthDateHandle.AutoReset = true;
            BirthDateHandle.Start();

            //日常行为的提示
            System.Timers.Timer TaskHandle = new System.Timers.Timer();
            TaskHandle.Interval = 1000 * 60;
            TaskHandle.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => TaskNotice());
            TaskHandle.AutoReset = true;
            TaskHandle.Start();

            //宿舍点检排班的提示


            //智能设备

            //ph和温度传感器用的是Com口上传
            //9600
            //mySerialPort.Parity = Parity.None;
            //mySerialPort.StopBits = StopBits.One;
            //mySerialPort.DataBits = 8;
            //mySerialPort.Handshake = Handshake.None;
            //mySerialPort.DataReceived += new SerialDataReceivedEventHandler(PHTEMPDataReceivedHandler);
            ////应该有一个时间的设置
            //mySerialPort.Open();


            //宿舍点检的提示
            System.Timers.Timer DormitoryUserHandle = new System.Timers.Timer();
            DormitoryUserHandle.Interval = 1000 * 60;
            DormitoryUserHandle.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => DoTimer());
            DormitoryUserHandle.AutoReset = true;
            DormitoryUserHandle.Start();

        }


        private void DoTimer()
        {
            DateTime Now = DateTime.Now;

            if (Now.Hour == 12 && Now.Minute == 30)
            {
                //提醒当天需要点检
                AlarmDormitoryUserNotice();
            }

            if (Now.Hour == 17 && Now.Minute == 00)
            {
                //提醒明天需要点检
                DormitoryUserNotice();
            }
        }


        //这个以天为单位,提醒当天有点检任务
        private void AlarmDormitoryUserNotice()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                try
                {
                    int WeekDay = (int)DateTime.Now.DayOfWeek;
                    List<Scheduling> SchedulingList = db.Scheduling.Where(t => t.Date == (WeekDate)WeekDay).ToList();

                    DateTime BeginDate = DateTime.Now.Date;
                    DateTime EndDate = DateTime.Now.AddDays(1).Date;


                    List<SpotCheck> SpotCheckList = db.SpotCheck.Where(t => t.CreateDate >= BeginDate
                        && t.CreateDate < EndDate).ToList();

                    List<User> UserList = db.User.Where(t => t.DDID != null && t.IsDel != true).ToList();
                    //UserList = UserList.Where(t => SpotCheckList.Select(m => m.CreateUser).Contains(t.ID)).ToList();



                    foreach (Scheduling Scheduling in SchedulingList)
                    {
                        foreach (string UserID in Scheduling.CheckName.Split(','))
                        {
                            int SelUserID = Convert.ToInt32(UserID);
                            if (SpotCheckList.FirstOrDefault(t => t.CreateUser == SelUserID) == null)
                            {
                                //发消息
                                User User = UserList.FirstOrDefault(t => t.ID == SelUserID);

                                new NoticeDAO().SendDDRemindNotice(
                                        User.DDID,
                                        "",
                                        "",
                                        DateTime.Now.ToString("yyyyMMdd") + "日工作提醒",
                                        "宿舍点检",
                                        "今日(" + DateTime.Now.ToString("yyyy-MM-dd") + ")需要进行宿舍点检，请去宿舍进行点检",
                                        null);

                            }


                        }
                        //int UserID = Convert.ToInt32(Scheduling.CheckName);



                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    AddNotice("系统服务的工作提醒存在问题:" + ex.Message, db.User.FirstOrDefault(t => t.UserName == "admin").ID, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
                }
            }

        }


        //这个以天为单位,提醒明天有点检任务
        private void DormitoryUserNotice()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                try
                {

                    int WeekDay = (int)DateTime.Now.DayOfWeek + 1;

                    if (WeekDay == 8)
                        WeekDay = 1;

                    List<Scheduling> SchedulingList = db.Scheduling.Where(t => t.Date == (WeekDate)WeekDay).ToList();
                    List<User> UserList = db.User.Where(t => t.DDID != null && t.IsDel != true).ToList();
                    foreach (Scheduling Scheduling in SchedulingList)
                    {
                        foreach (string UserID in Scheduling.CheckName.Split(','))
                        {
                            int SelUserID = Convert.ToInt32(UserID);
                            User User = UserList.FirstOrDefault(t => t.ID == SelUserID);

                            new NoticeDAO().SendDDRemindNotice(
                                User.DDID,
                                "",
                                "",
                                DateTime.Now.ToString("yyyyMMdd") + "日工作提醒",
                                "宿舍点检",
                                "明日(" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + ")需要进行宿舍点检，请提前做好准备",
                                null);

                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    AddNotice("系统服务的工作提醒存在问题:" + ex.Message, db.User.FirstOrDefault(t => t.UserName == "admin").ID, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
                }
            }

        }


        //这个以分钟为单位
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


                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    AddNotice("系统服务的工作提醒存在问题:" + ex.Message, db.User.FirstOrDefault(t => t.UserName == "admin").ID, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
                }
            }

        }

        //这个以天为单位
        private void UserProbation()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                try
                {
                    //DateTime AlertDate = DateTime.Now.AddDays(-1 * userdetails.ProbationDays);
                    var UserDetailsList = from userdetails in db.UserDetails.Where(t => t.QualifiedDate == null)
                                          join user in db.User.Where(t => t.IsDel != true) on userdetails.UserID equals user.ID
                                          where user.IsDel != true && user.UserState == UserState.试用期
                                          orderby user.ID
                                          select new
                                                 {
                                                     user.ID,
                                                     user.RealName,
                                                     userdetails.ProbationDays,
                                                     userdetails.HireDate
                                                 };

                    //List<UserDetails> UserDetailsList = db.UserDetails.Where(t => t.HireDate.AddDays(t.ProbationDays) < DateTime.Now && t.QualifiedDate == null && db.User.Where(m => m.IsDel != true && m.UserState != UserState.离职).Select(m => m.ID).Contains(t.UserID)).ToList();

                    int SystemConfigDay = 10;
                    //int SystemConfigDay = Convert.ToInt32(GetXML("Setting/SysConfig/ProbationDay"));

                    Role Role = db.Role.FirstOrDefault(t => t.Name == "员工转正消息处理人员");

                    List<User> ReciveUserList = db.User.Where(t => t.IsDel != true
                     && t.UserState != UserState.离职
                     && db.UserRole.Where(m => m.RoleID == Role.ID).Select(m => m.UserID).Contains(t.ID)).ToList();


                    //List<User> UserList = db.User.Where(t => t.IsDel != true && t.UserState != UserState.离职).ToList();

                    foreach (var UserDetails in UserDetailsList)
                    {
                        if (UserDetails.HireDate.AddDays(UserDetails.ProbationDays) > DateTime.Now && UserDetails.HireDate.AddDays(UserDetails.ProbationDays - SystemConfigDay) < DateTime.Now)
                        {
                            foreach (User ReciveUser in ReciveUserList)
                            {
                                AddNotice("员工" + UserDetails.RealName + "的试用期将于" + UserDetails.HireDate.AddDays(UserDetails.ProbationDays).ToString("yyyy-MM-dd") + "结束,请提前安排员工转正相关事宜", ReciveUser.ID, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    AddNotice("系统服务的员工转正提示存在问题:" + ex.Message, db.User.FirstOrDefault(t => t.UserName == "admin").ID, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
                }
            }

        }

        //这个以天为单位
        private void BirthDate()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                try
                {
                    List<User> UserList = db.User.Where(t => t.IsDel != true && t.UserState != UserState.离职).ToList();
                    List<UserDetails> UserDetails = db.UserDetails.ToList();

                    Role Role = db.Role.FirstOrDefault(t => t.Name == "员工生日接受处理人员");

                    List<User> ReciveUserList = db.User.Where(t => t.IsDel != true
                   && t.UserState != UserState.离职
                   && db.UserRole.Where(m => m.RoleID == Role.ID).Select(m => m.UserID).Contains(t.ID)).ToList();

                    foreach (User User in UserList)
                    {
                        DateTime? birthdate = UserDetails.FirstOrDefault(t => t.UserID == User.ID).BirthDate;
                        if (birthdate != null)
                        {
                            DateTime ThisDate = Convert.ToDateTime(DateTime.Now.Year + "-" + birthdate.Value.Month + "-" + birthdate.Value.Day);
                            if (Convert.ToInt32((ThisDate - DateTime.Now.Date).TotalDays) == 5)
                            {
                                foreach (User ReciveUser in ReciveUserList)
                                {
                                    AddNotice("距离员工" + User.RealName + "的生日" + birthdate.Value.ToString("MM-dd") + "还有5天，请提前准备生日礼物。", ReciveUser.ID, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddNotice("系统服务的员工生日接受存在问题:" + ex.Message, db.User.FirstOrDefault(t => t.UserName == "admin").ID, db.User.FirstOrDefault(t => t.UserName == "admin").ID);
                }
            }
        }


        //ph-温度传感器
        private static void PHTEMPDataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();

            if (!string.IsNullOrEmpty(indata.Trim()))
            {
                //ID:01 PH:15.2 TEMP:12.5  ALARM!
                string ID = indata.Substring(indata.IndexOf("ID:") + 3, indata.IndexOf("PH:") - indata.IndexOf("ID:") - 3).Trim();
                double PH = Convert.ToDouble(indata.Substring(indata.IndexOf("PH:") + 3, indata.IndexOf("TEMP:") - indata.IndexOf("PH:") - 3).Trim());
                double Temperature = Convert.ToDouble(indata.Substring(indata.IndexOf("TEMP:") + 5).Replace("ALARM!", "").Trim());

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    IntelligentDevice IntelligentDevice = db.IntelligentDevice.FirstOrDefault(t => t.Identity == ID);
                    if (IntelligentDevice != null
                     && (IntelligentDevice.BeginDate == null || new BaseUtils().GetTodayDate(DateTime.Now, IntelligentDevice.BeginDate.Value) < DateTime.Now)
                     && (IntelligentDevice.EndDate == null || new BaseUtils().GetTodayDate(DateTime.Now, IntelligentDevice.EndDate.Value) > DateTime.Now))
                    {
                        IntelligentDeviceData IntelligentDeviceData = new IntelligentDeviceData();
                        IntelligentDeviceData.IntelligentDeviceID = IntelligentDevice.ID;
                        IntelligentDeviceData.CreateDate = DateTime.Now;
                        IntelligentDeviceData.DeviceDataType = DeviceDataType.PH值;
                        IntelligentDeviceData.Data = PH.ToString();
                        IntelligentDeviceData.IsAlert = !(PH >= 5 && PH <= 9);
                        db.IntelligentDeviceData.Add(IntelligentDeviceData);

                        IntelligentDeviceData = new IntelligentDeviceData();
                        IntelligentDeviceData.IntelligentDeviceID = IntelligentDevice.ID;
                        IntelligentDeviceData.CreateDate = DateTime.Now;
                        IntelligentDeviceData.DeviceDataType = DeviceDataType.温度;
                        IntelligentDeviceData.Data = Temperature.ToString();
                        IntelligentDeviceData.IsAlert = false;
                        db.IntelligentDeviceData.Add(IntelligentDeviceData);

                        db.SaveChanges();
                    }
                }
            }
        }


        public void AddNotice(string message, int ReciveUserID, int SendUserID, NoticeType NoticeType = NoticeType.系统通知)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {

                Notice Notice = new Notice();
                Notice.Contenet = "【" + Enum.GetName(typeof(NoticeType), NoticeType) + "】" + message;
                Notice.IsSend = false;
                Notice.NoticeType = NoticeType;
                Notice.ReciveUserID = ReciveUserID;
                Notice.SendDate = DateTime.Now;
                Notice.SendUserID = SendUserID;
                db.Notice.Add(Notice);
                db.SaveChanges();
            }
        }

        public string GetXML(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释

            XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("") + ParaUtils.XmlPath, settings);
            XmlNode wx = doc.SelectSingleNode(path);

            if (wx != null)
            {
                return wx.InnerText;
            }


            return "";
        }

        protected override void OnStop()
        {
        }
    }
}
