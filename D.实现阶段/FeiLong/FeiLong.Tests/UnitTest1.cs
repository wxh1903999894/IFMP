using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeiLongLibrary.BLL;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;
using FeiLongLibrary.DBContext;

namespace FeiLong.Tests
{
    [TestClass]
    public class UnitTest1
    {
        FLDbContext db = new FLDbContext();

        [TestMethod]
        public void TestMethod1()
        {
            DateTime testdate1 = Convert.ToDateTime("2018-01-30");
            testdate1 = testdate1.AddMonths(1);


            DateTime beginDate = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            DateTime endDate = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.AddMonths(1).Month + "-01");

            //db.BaseClass.Where(t => t.ID == 1);

            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();
            //var auth = db.Authorization.Where(t => t.ID > 0);
            //stopwatch.Stop(); //  停止监视
            //TimeSpan timeSpan = stopwatch.Elapsed; //  获取总时间
            //double milliseconds = timeSpan.TotalMilliseconds;


            //System.Diagnostics.Stopwatch stopwatch2 = new System.Diagnostics.Stopwatch();
            //stopwatch2.Start();
            //List<Authorization> testlist = db.Authorization.Where(t => t.ID > 0).ToList();
            //stopwatch.Stop(); //  停止监视
            //TimeSpan timeSpan2 = stopwatch2.Elapsed; //  获取总时间
            //double milliseconds2 = timeSpan2.TotalMilliseconds;


            //try
            //{
            //    string str1 = "";
            //    string str2 = "";
            //    System.Diagnostics.Stopwatch stopwatch3 = new System.Diagnostics.Stopwatch();
            //    stopwatch3.Start();
            //    foreach (SysUser user in db.SysUser.Where(t => t.IsAccountDisabled != true))
            //    {
            //        str1 = str1 + user.RealName + ",";

            //        foreach (Role role in db.Role.Where(t => t.IsDel != true))
            //        {

            //        }
            //    }
            //    stopwatch3.Stop(); //  停止监视
            //    TimeSpan timeSpan3 = stopwatch3.Elapsed; //  获取总时间
            //    double milliseconds3 = timeSpan3.TotalMilliseconds;


            //    System.Diagnostics.Stopwatch stopwatch4 = new System.Diagnostics.Stopwatch();
            //    stopwatch4.Start();
            //    foreach (SysUser user in db.SysUser.Where(t => t.IsAccountDisabled != true).ToList())
            //    {
            //        str2 = str2 + user.RealName + ",";
            //    }
            //    stopwatch4.Stop(); //  停止监视
            //    TimeSpan timeSpan4 = stopwatch4.Elapsed; //  获取总时间
            //    double milliseconds4 = timeSpan4.TotalMilliseconds;


            //    foreach (Role role in db.Role.Where(t => t.IsDel != true))
            //    {

            //    }
            //}
            //catch
            //{

            //}

            //AuthorizationRole ar = new AuthorizationRole();
            //ar.Role = db.Role.FirstOrDefault(t => t.Name == "系统管理员");
            //ar.Authorization = db.Authorization.FirstOrDefault(t => t.ShowName == "用户管理");
            //ar.User = null;
            //db.AuthorizationRole.Add(ar);
            //db.SaveChanges();
            //AuthorizationRole ar = db.AuthorizationRole.FirstOrDefault();

            int i = 0;
            //DateTime dt1 = DateTime.Now;
            //DateTime dt2 = Convert.ToDateTime("2018-05-10 12:00:00");
            //DateTime dt3 = dt1.Date + dt2.TimeOfDay;



            //BaseDateFlow bdf = new BaseDateFlow();
            //bdf.ClassType = ClassTypeEnums.早班;

            //bdf.BeginDate = Convert.ToDateTime("09:30:00");
            //bdf.EndDate = Convert.ToDateTime("10:00:00");
            //bdf.RemindDate = Convert.ToDateTime("09:50:00");

            //bdf.FlowID = 5;
            //bdf.Name = "生产设备部部长审核";
            //db.BaseDateFlow.Add(bdf);
            //db.SaveChanges();
        }
    }
}
