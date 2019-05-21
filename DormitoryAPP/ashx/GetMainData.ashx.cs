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
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data.Entity;

namespace DormitoryAPP.ashx
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
                case "GetDormitory":
                    GetDormitory(context);
                    break;
                case "SpotProblemAdd":
                    SpotProblemAdd(context);
                    break;
                case "GetPM":
                    GetPM(context);
                    break;
                case "GetProblemDetail":
                    GetProblemDetail(context);
                    break;
                case "GetGrade":
                    GetGrade(context);
                    break;
                case "GetBestorBad":
                    GetBestorBad(context);
                    break;
                case "GetDorUser":
                    GetDorUser(context);
                    break;
                case "GetReviewDetail":
                    GetReviewDetail(context);
                    break;
                case "SaveReviewDetail":
                    SaveReviewDetail(context);
                    break;
                case "ImageDetail":
                    ImageDetail(context);
                    break;
                case "GetDepUser":
                    GetDepUser(context);
                    break;
                case "ModifyCheck":
                    ModifyCheck(context);
                    break;
                case "GetWeek":
                    GetWeek(context);
                    break;
                case "GetWeekUser":
                    GetWeekUser(context);
                    break;
                case "GetProblemDetailPageList":
                    GetProblemDetailPageList(context);
                    break;
                case "GetProblemDetailPageListByDromitoryId":
                    GetProblemDetailPageListByDromitoryId(context);
                    break;
                case "GetBestorBadMore":
                    GetBestorBadMore(context);
                    break;
                case "GetDorMarks":
                    GetDorMarks(context);
                    break;
                case "GetScoreByDor":
                    GetScoreByDor(context);
                    break;
                case "GetDormitoryMarks":
                    GetDormitoryMarks(context);
                    break;
                case "GetSelectProblem":
                    GetSelectProblem(context);
                    break;
                case "GetDormitoryMarksnoToday":
                    GetDormitoryMarksnoToday(context);
                    break;
            }
        }

        #region 获取选择问题列表
        public void GetSelectProblem(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var name = "";
                List<SpotSelectProblem> problemlist = db.SpotSelectProblem.OrderBy(t=>t.Order).Where(x => x.IsDel == false).ToList();
                foreach (SpotSelectProblem problem in problemlist)
                {
                    name += "{\"value\":\"" + problem.ID + "\",\"text\":\"" + problem.Problem + "\"},";
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

        #region 获取今天评分过的宿舍列表
        public void GetDormitoryMarks(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var date = DateTime.Now.Date;
                List<SpotCheck> SpotCheckList = db.SpotCheck.OrderBy(t => t.DormitoryId).Where(x => DbFunctions.TruncateTime(x.CreateDate) == date && x.SpotScore > 0).ToList();
                List<Dormitory> DormitoryList = db.Dormitory.Where(x => x.IsCheck == true).ToList();
                var name = "";
                List<DormitoryUser> dormitoryuserlist = db.DormitoryUser.ToList();
                foreach (SpotCheck SpotCheck in SpotCheckList)
                {
                    if (dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == SpotCheck.DormitoryId) != null)
                    {
                        var DormitoryUser = dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == SpotCheck.DormitoryId).UserID;
                        if (DormitoryUser != null && DormitoryUser != "")
                        {
                            var dormitory = DormitoryList.FirstOrDefault(x => x.ID == SpotCheck.DormitoryId);
                            name += "{\"value\":\"" + dormitory.ID + "\",\"text\":\"" + dormitory.DormiName + "\"},";
                        }
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
        #endregion

        #region 获取今天没有评分过的宿舍列表
        public void GetDormitoryMarksnoToday(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var name = "";
                var date = DateTime.Now.Date;
                var model = db.SpotCheck.Where(x => DbFunctions.TruncateTime(x.CreateDate) == date).ToList();
                if (model.Count == 0)
                {
                    List<Dormitory> DormitoryList = db.Dormitory.Where(x => x.IsCheck == true).ToList();
                    List<DormitoryUser> dormitoryuserlist = db.DormitoryUser.ToList();
                    foreach (Dormitory dormitory in DormitoryList)
                    {
                        if (dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == dormitory.ID) != null)
                        {
                            var DormitoryUser = dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == dormitory.ID).UserID;
                            if (DormitoryUser != null && DormitoryUser != "")
                            {
                                name += "{\"value\":\"" + dormitory.ID + "\",\"text\":\"" + dormitory.DormiName + "\"},";
                            }
                        }
                    }
                }
                else
                {
                    List<SpotCheck> SpotCheckList = db.SpotCheck.OrderBy(t => t.DormitoryId).Where(x => DbFunctions.TruncateTime(x.CreateDate) == date && x.SpotScore > 0).ToList();
                    List<Dormitory> DormitoryList = db.Dormitory.Where(x => x.IsCheck == true).ToList();
                    string spotcheck = "";
                    string dormitory = "";
                    string exp = "";
                    foreach (SpotCheck sp in SpotCheckList)
                    {
                        spotcheck += sp.DormitoryId + ",";
                    }
                    foreach (Dormitory dor in DormitoryList)
                    {
                        dormitory += dor.ID + ",";
                    }
                    var res = dormitory.Split(',').Except(spotcheck.Split(','));
                    foreach (var v in res)
                    {
                        exp += v + ",";
                    }
                    if (exp != "")
                    {
                        List<DormitoryUser> dormitoryuserlist = db.DormitoryUser.ToList();
                        foreach (var dorid in exp.TrimEnd(',').Split(','))
                        {
                            var id = int.Parse(dorid);
                            if (dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == id) != null)
                            {
                                var DormitoryUser = dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == id).UserID;
                                if (DormitoryUser != null && DormitoryUser != "")
                                {
                                    var dormitorymodel = DormitoryList.FirstOrDefault(x => x.ID == id);
                                    name += "{\"value\":\"" + dormitorymodel.ID + "\",\"text\":\"" + dormitorymodel.DormiName + "\"},";
                                }
                            }
                        }
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
        #endregion

        #region 根据宿舍获取今天的评分
        public void GetScoreByDor(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                int dormitoryid = Convert.ToInt32(context.Request.Params["dormitoryid"]);
                SpotCheck model = new SpotCheck();
                var date = DateTime.Now.Date;
                model = db.SpotCheck.FirstOrDefault(x => x.DormitoryId == dormitoryid && DbFunctions.TruncateTime(x.CreateDate) == date);
                var name = "";
                name = "{\"score\":\"" + model.SpotScore + "\",\"spotid\":\"" + model.SpotId + "\"}";
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(name.TrimEnd(','));
                sb.Append("]}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion

        #region 批量提交宿舍分数
        public void GetDorMarks(HttpContext context)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //int uid = 6;
                    string userid = context.Request.Params["UserID"];
                    int uid = Convert.ToInt32(userid.Split(',')[0]);
                    var value = context.Request.Params["SpotScore"].Split(',');
                    var pid = context.Request.Params["DormitorySel"].TrimEnd(',').Split(',');
                    var enumvalue = (WeekDate)Enum.Parse(typeof(WeekDate), DateTime.Now.DayOfWeek.ToString("d"));
                    enumvalue = enumvalue == 0 ? WeekDate.星期日 : enumvalue;
                    var checkname = db.Scheduling.FirstOrDefault(x => x.Date == enumvalue).CheckName;
                    if (checkname.Split(',').Any(x => x == uid.ToString()))
                    {
                        for (int i = 0; i < pid.Length; i++)
                        {
                            if (value[i] != "")
                            {
                                SpotCheck SpotCheck = new SpotCheck();
                                SpotCheck.SpotScore = value[i] == "" ? 0 : Convert.ToInt32(value[i]);
                                SpotCheck.DormitoryId = Convert.ToInt32(pid[i]);
                                SpotCheck.CreateUser = uid;
                                SpotCheck.CreateDate = DateTime.Now;
                                db.SpotCheck.Add(SpotCheck);
                                db.SaveChanges();
                            }
                        }
                        sb.Append("{\"result\":\"true\"}");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\"}");
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.ApplicationInstance.CompleteRequest();
        }
        #endregion

        #region 获取点检排班时间
        public void GetWeek(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("");
            string name = "";
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Scheduling> scheduinglist = db.Scheduling.OrderBy(x => x.Date).ToList();
                    foreach (var scheduing in scheduinglist)
                    {
                        name += "{\"value\":\"" + (int)scheduing.Date + "\",\"text\":\"" + (WeekDate)scheduing.Date + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
            }
            catch
            {
                sb.Append("{\"result\":\"false\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.End();
        }
        #endregion

        #region 获取点检排班人列表
        public void GetWeekUser(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("");
            string name = "";
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int datavalue = Convert.ToInt32(context.Request.Params["datavalue"]);
                    List<Scheduling> scheduinglist = db.Scheduling.Where(x => x.Date != (WeekDate)datavalue).ToList();
                    var thischeckname = db.Scheduling.FirstOrDefault(x => x.Date == (WeekDate)datavalue).CheckName.TrimEnd(',').Split(',');
                    string checknameid = "";
                    foreach (var scheduing in scheduinglist)
                    {
                        checknameid += scheduing.CheckName + ",";
                    }
                    var checkname = checknameid.TrimEnd(',').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    var newuser = "";
                    var res = checkname.Except(thischeckname);
                    foreach (var v in res)
                    {
                        newuser += v + ",";
                    }

                    var checkid = newuser.TrimEnd(',').Split(',');
                    List<User> userlist = db.User.ToList();
                    for (int i = 0; i < checkid.Length; i++)
                    {
                        int userid = 0;
                        if (int.TryParse(checkid[i], out userid))
                        {
                            string realname = userlist.FirstOrDefault(x => x.ID == userid).RealName;
                            name += "{\"value\":\"" + userid + "\",\"text\":\"" + realname + "\"},";
                        }
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\",\"data\":\"" + ex.Message + "\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.End();
        }
        #endregion

        #region 获取部门人员列表
        public void GetDepUser(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                int depid = Convert.ToInt32(context.Request.Params["depid"]);
                List<DepartmentUser> model = db.DepartmentUser.Where(x => x.DepartmentID == depid).ToList();
                var name = "";
                List<User> userlist = db.User.ToList();
                foreach (var userid in model)
                {
                    User user = userlist.FirstOrDefault(x => x.ID == userid.UserID);
                    name += "{\"value\":\"" + user.ID + "\",\"text\":\"" + user.RealName + "\"},";
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

        #region 添加新点检人
        public void ModifyCheck(HttpContext context)
        {
            try
            {
                int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                //int uid = 315;
                int UserSel = Convert.ToInt32(context.Request.Params["UserSel"]);
                int DataSel = Convert.ToInt32(context.Request.Params["DataSel"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Scheduling> scheduinglist = db.Scheduling.ToList();
                    string checknameid = "";
                    foreach (var scheduing in scheduinglist)
                    {
                        checknameid += scheduing.CheckName + ",";
                    }
                    var checkname = checknameid.TrimEnd(',').Split(',').Distinct().ToArray();
                    if (checkname.Any(x => x == uid.ToString()))
                    {
                        Scheduling model = db.Scheduling.FirstOrDefault(t => t.Date == (WeekDate)DataSel);
                        var check = model.CheckName;
                        if (check.Split(',').Any(x => x == UserSel.ToString())) { throw new Exception(); }
                        var name = check + "," + UserSel;
                        model.CheckName = name.TrimStart(',');
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception();
                    }
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
        #endregion

        #region 获取宿舍列表
        public void GetDormitory(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<Dormitory> DormitoryList = db.Dormitory.Where(x => x.IsCheck == true).ToList();
                var name = "";
                List<DormitoryUser> dormitoryuserlist = db.DormitoryUser.ToList();
                foreach (Dormitory dormitory in DormitoryList)
                {
                    if (dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == dormitory.ID) != null)
                    {
                        var DormitoryUser = dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == dormitory.ID).UserID;
                        if (DormitoryUser != null && DormitoryUser != "")
                        {
                            name += "{\"value\":\"" + dormitory.ID + "\",\"text\":\"" + dormitory.DormiName + "\"},";
                        }
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
        #endregion


        #region 获取宿舍人员列表
        public void GetDorUser(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                int dormitoryid = Convert.ToInt32(context.Request.Params["dormitoryid"]);
                DormitoryUser model = db.DormitoryUser.FirstOrDefault(x => x.DormitoryID == dormitoryid);
                var name = "";
                List<User> userlist = db.User.ToList();
                foreach (var dor in model.UserID.TrimEnd(',').Split(','))
                {
                    int id = int.Parse(dor);
                    User user = userlist.FirstOrDefault(x => x.ID == id);
                    name += "{\"value\":\"" + id + "\",\"text\":\"" + user.RealName + "\"},";
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

        #region 宿舍点检保存
        //public void SpotProblemAdd(HttpContext context)
        //{
        //    try
        //    {
        //        //int uid = 6;
        //        string userid = context.Request.Params["UserID"];
        //        int uid = Convert.ToInt32(userid.Split(',')[0]);
        //        int SpotScore = Convert.ToInt32(context.Request.Params["SpotScore"]);
        //        int index = Convert.ToInt32(context.Request.Params["index"]);
        //        int DormitorySel = Convert.ToInt32(context.Request.Params["DormitorySel"]);
        //        var imageFile = context.Request.Params["ImgUrl"];
        //        var ProDesc = context.Request.Params["ProDesc"];
        //        var DutyUser = Convert.ToInt32(context.Request.Params["DutyUser"]);

        //        string ImagePath = "";
        //        string ResourceImagePath = "";
        //        ImagePath = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
        //        string Image = imageFile;
        //        if (!string.IsNullOrEmpty(Image))
        //        {
        //            ResourceImagePath = HttpContext.Current.Server.MapPath("..\\Templete\\");
        //            Image = Image.Replace(Image.Split(',')[0] + ",", "");

        //            if (!Directory.Exists(ResourceImagePath))
        //            {
        //                Directory.CreateDirectory(ResourceImagePath);
        //            }
        //            byte[] bytes = Convert.FromBase64String(Image);
        //            MemoryStream memStream = new MemoryStream(bytes);
        //            Bitmap bitmap = new Bitmap(memStream);

        //            bitmap.Save(ResourceImagePath + ImagePath, ImageFormat.Jpeg);
        //            ImagePath = "Templete/" + ImagePath;
        //        }
        //        else
        //        {
        //            ImagePath = "";
        //        }

        //        using (IFMPDBContext db = new IFMPDBContext())
        //        {
        //            var enumvalue = (WeekDate)Enum.Parse(typeof(WeekDate), DateTime.Now.DayOfWeek.ToString("d"));
        //            enumvalue = enumvalue == 0 ? WeekDate.星期日 : enumvalue;
        //            var checkname = db.Scheduling.FirstOrDefault(x => x.Date == enumvalue).CheckName;
        //            if (checkname.Split(',').Any(x => x == uid.ToString()))
        //            {
        //                SpotCheck SpotCheck = new SpotCheck();
        //                var user = db.User.FirstOrDefault(x => x.ID == uid);
        //                if (index == 0)
        //                {
        //                    SpotCheck.SpotScore = SpotScore;
        //                }
        //                SpotCheck.DormitoryId = DormitorySel;
        //                SpotCheck.CreateUser = user.ID;
        //                SpotCheck.CreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        //                db.SpotCheck.Add(SpotCheck);
        //                db.SaveChanges();
        //                if (DutyUser != -2)
        //                {
        //                    SpotProblem SpotProblem = new SpotProblem();
        //                    SpotProblem.SpotId = SpotCheck.SpotId;
        //                    SpotProblem.ProDesc = ProDesc;
        //                    SpotProblem.SImage = ImagePath;
        //                    SpotProblem.DutyUser = db.User.FirstOrDefault(x => x.ID == DutyUser).ID.ToString();
        //                    SpotProblem.CreateUser = db.User.FirstOrDefault(x => x.ID == uid).ID.ToString();
        //                    SpotProblem.CreateDate = DateTime.Now;
        //                    SpotProblem.IsreView = false;
        //                    db.SpotProblem.Add(SpotProblem);
        //                    db.SaveChanges();
        //                }
        //                sb.Append("{\"result\":\"true\"}");
        //            }
        //            else
        //            {
        //                throw new Exception();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        sb.Append("{\"result\":\"false\"}");
        //        //sb.Append("{\"result\":\"false\",\"data\":\"" + "-" + context.Request.Params["UserID"].ToString() + "-" + ex.Message + "\"}");
        //    }

        //    context.Response.Clear();
        //    context.Response.Write(sb.ToString().TrimEnd(','));
        //    context.ApplicationInstance.CompleteRequest();
        //    //context.Response.End();
        //}
        #endregion

        #region 宿舍点检保存,没有分数的
        public void SpotProblemAdd(HttpContext context)
        {
            try
            {
                //int uid = 6;
                string userid = context.Request.Params["UserID"];
                int uid = Convert.ToInt32(userid.Split(',')[0]);
                int spotid = Convert.ToInt32(context.Request.Params["spotid"]);
                var imageFile = context.Request.Params["ImgUrl"];
                var ProDesc = context.Request.Params["ProDesc"];
                var DutyUser = Convert.ToInt32(context.Request.Params["DutyUser"]);
                var problem = context.Request.Params["problemIDList"];
                string ImagePath = "";
                string ResourceImagePath = "";
                ImagePath = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                string Image = imageFile;
                if (!string.IsNullOrEmpty(Image))
                {
                    ResourceImagePath = HttpContext.Current.Server.MapPath("..\\Templete\\");
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
                }
                else
                {
                    ImagePath = "";
                }

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    var enumvalue = (WeekDate)Enum.Parse(typeof(WeekDate), DateTime.Now.DayOfWeek.ToString("d"));
                    enumvalue = enumvalue == 0 ? WeekDate.星期日 : enumvalue;
                    var checkname = db.Scheduling.FirstOrDefault(x => x.Date == enumvalue).CheckName;
                    if (checkname.Split(',').Any(x => x == uid.ToString()))
                    {
                        SpotProblem SpotProblem = new SpotProblem();
                        SpotProblem.SelectDesc = problem;
                        SpotProblem.SpotId = spotid;
                        SpotProblem.ProDesc = ProDesc;
                        SpotProblem.SImage = ImagePath;
                        SpotProblem.DutyUser = db.User.FirstOrDefault(x => x.ID == DutyUser).ID.ToString();
                        SpotProblem.CreateUser = db.User.FirstOrDefault(x => x.ID == uid).ID.ToString();
                        SpotProblem.CreateDate = DateTime.Now;
                        SpotProblem.IsreView = false;
                        db.SpotProblem.Add(SpotProblem);
                        db.SaveChanges();
                        sb.Append("{\"result\":\"true\"}");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\"}");
                //sb.Append("{\"result\":\"false\",\"data\":\"" + "-" + context.Request.Params["UserID"].ToString() + "-" + ex.Message + "\"}");
            }

            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.ApplicationInstance.CompleteRequest();
            //context.Response.End();
        }
        #endregion

        #region 宿舍排名
        public void GetPM(HttpContext context)
        {
            StringBuilder sbscore = new StringBuilder("");
            //1 年 2 月
            int flag = Convert.ToInt32(context.Request.Params["flag"]);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                var BeginDate = new DateTime();
                var EndDate = new DateTime();
                if (flag == 1)
                {
                    BeginDate = Convert.ToDateTime(DateTime.Now.Year + "-01" + "-01");
                    EndDate = Convert.ToDateTime((DateTime.Now.Year + 1) + "-01" + "-01");
                }
                if (flag == 2)
                {
                    int begin = Convert.ToInt32(context.Request.Params["begin"]);
                    int end = Convert.ToInt32(context.Request.Params["end"]);
                    BeginDate = Convert.ToDateTime(DateTime.Now.Year + "-" + begin + "-01");
                    EndDate = Convert.ToDateTime(DateTime.Now.Year + "-" + (end + 1) + "-01");
                }
                //List<Dormitory> DormitoryList = db.Dormitory.Where(x => x.CreateDate > BeginDate && x.CreateDate < EndDate).ToList();
                List<Dormitory> DormitoryList = db.Dormitory.ToList();
                string name = "";
                foreach (Dormitory dormitory in DormitoryList)
                {
                    List<SpotCheck> model = db.SpotCheck.Where(x => x.DormitoryId == dormitory.ID && x.CreateDate >= BeginDate && x.CreateDate < EndDate).ToList();
                    if (model.Count > 1)
                    {
                        int score = 0;
                        score = model.Sum(x => x.SpotScore);
                        name += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + score + "\"},";
                    }
                    else if (model.Count == 1)
                    {
                        name += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + model[0].SpotScore + "\"},";
                    }
                    else if (model.Count == 0)
                    {
                        name += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + 0 + "\"},";
                    }
                }
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(name.TrimEnd(','));
                sb.Append("]}");
                var dt = JsonToDataTable(sb.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    string text = "";
                    foreach (DataRow row in dt.Rows.Cast<DataRow>().OrderByDescending(r => int.Parse(r["score"].ToString())))
                    {
                        text += "{\"name\":\"" + row["name"] + "\",\"score\":\"" + row["score"] + "\"},";
                    }
                    sbscore.Append("{\"result\":\"true\",\"data\":[");
                    sbscore.Append(text.TrimEnd(','));
                    sbscore.Append("]}");
                }
            }
            context.Response.Clear();
            context.Response.Write(sbscore.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion

        #region 问题统计详情
        public void GetProblemDetail(HttpContext context)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //1 问题统计  2 问题复查
                    int flag = Convert.ToInt32(context.Request.Params["flag"]);
                    List<SpotProblem> SpotProblemList = db.SpotProblem.OrderByDescending(t => t.CreateDate).ToList();
                    if (flag == 2)
                    {
                        SpotProblemList = db.SpotProblem.OrderByDescending(t => t.CreateDate).Where(x => x.IsreView == false).ToList();
                    }
                    string name = "";
                    List<User> userlist = db.User.ToList();
                    List<SpotCheck> spotchecklist = db.SpotCheck.ToList();
                    List<Dormitory> dormitorylist = db.Dormitory.ToList();
                    foreach (SpotProblem spotproblem in SpotProblemList)
                    {
                        var isreview = spotproblem.IsreView.ToString() == "False" ? "否" : "是";
                        SpotCheck model = spotchecklist.FirstOrDefault(x => x.SpotId == spotproblem.SpotId);
                        Dormitory dormodel = dormitorylist.FirstOrDefault(x => x.ID == model.DormitoryId);
                        int duryuserid = int.Parse(spotproblem.DutyUser);
                        var dutyuser = userlist.FirstOrDefault(x => x.ID == duryuserid).RealName;
                        int createuserid = int.Parse(spotproblem.CreateUser);
                        var createuser = userlist.FirstOrDefault(x => x.ID == createuserid).RealName;
                        var reviewuser = "";
                        if (spotproblem.ReviewUser != null)
                        {
                            int reviewuserid = int.Parse(spotproblem.ReviewUser);
                            reviewuser = userlist.FirstOrDefault(x => x.ID == reviewuserid).RealName;
                        }
                        if (dormodel != null)
                        {
                            name += "{\"ProDesc\":\"" + spotproblem.ProDesc + "\",\"DutyUser\":\"" + dutyuser + "\",\"CreateUser\":\"" + createuser + "\",\"CreateDate\":\"" + Convert.ToDateTime(spotproblem.CreateDate).ToShortDateString() + "\",\"SpotScore\":\"" + model.SpotScore + "\",\"DormiName\":\"" + dormodel.DormiName + "\",\"IsreView\":\"" + isreview + "\",\"SpId\":\"" + spotproblem.SpId + "\",\"ReviewUser\":\"" + reviewuser + "\",\"ReviewMemo\":\"" + spotproblem.ReviewMemo + "\",\"SpotId\":\"" + model.SpotId + "\"},";
                        }
                    }
                    if (name == "")
                    {
                        sb.Append("{\"result\":\"false\"}");
                    }
                    else
                    {
                        sb.Append("{\"result\":\"true\",\"data\":[");
                        sb.Append(name.TrimEnd(','));
                        sb.Append("]}");
                    }
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
        #endregion


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
            /// 宿舍选择的问题
            /// </summary>
            public string SelectDesc { get; set; }

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
        }
        #region 问题统计详情分页
        public void GetProblemDetailPageList(HttpContext context)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //1 问题统计  2 问题复查
                    int flag = Convert.ToInt32(context.Request.Params["flag"]);
                    int pageindex = Convert.ToInt32(context.Request.Params["pageindex"]);
                    int pagesize = Convert.ToInt32(context.Request.Params["pagesize"]);
                    List<TempSpotProblem> SpotProblemList = new List<TempSpotProblem>();
                    if (flag == 2)
                    {
                        SpotProblemList = db.SpotProblem.OrderByDescending(t => t.CreateDate).Where(x => x.IsreView == false).Select(t =>
                            new TempSpotProblem
                            {
                                SelectDesc=t.SelectDesc,
                                CreateDate = t.CreateDate,
                                CreateUser = t.CreateUser,
                                DutyUser = t.DutyUser,
                                IsreView = t.IsreView,
                                ProDesc = t.ProDesc,
                                ReviewDate = t.ReviewDate,
                                ReviewMemo = t.ReviewMemo,
                                ReviewUser = t.ReviewUser,
                                SpId = t.SpId,
                                SpotId = t.SpotId
                            }).OrderByDescending(t => t.CreateDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    }
                    else
                    {
                        SpotProblemList = db.SpotProblem.Select(t =>
                            new TempSpotProblem
                            {
                                SelectDesc = t.SelectDesc,
                                CreateDate = t.CreateDate,
                                CreateUser = t.CreateUser,
                                DutyUser = t.DutyUser,
                                IsreView = t.IsreView,
                                ProDesc = t.ProDesc,
                                ReviewDate = t.ReviewDate,
                                ReviewMemo = t.ReviewMemo,
                                ReviewUser = t.ReviewUser,
                                SpId = t.SpId,
                                SpotId = t.SpotId
                            }).OrderByDescending(t => t.CreateDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    }
                    string name = "";
                    List<User> userlist = db.User.ToList();
                    List<SpotCheck> spotchecklist = db.SpotCheck.ToList();
                    List<Dormitory> dormitorylist = db.Dormitory.ToList();
                    List<SpotSelectProblem> problemlist = db.SpotSelectProblem.ToList();
                    foreach (TempSpotProblem spotproblem in SpotProblemList)
                    {
                        var isreview = spotproblem.IsreView.ToString() == "False" ? "否" : "是";
                        SpotCheck model = spotchecklist.FirstOrDefault(x => x.SpotId == spotproblem.SpotId);
                        Dormitory dormodel = dormitorylist.FirstOrDefault(x => x.ID == model.DormitoryId);
                        int duryuserid = int.Parse(spotproblem.DutyUser);
                        var dutyuser = userlist.FirstOrDefault(x => x.ID == duryuserid).RealName;
                        int createuserid = int.Parse(spotproblem.CreateUser);
                        var createuser = userlist.FirstOrDefault(x => x.ID == createuserid).RealName;
                        var reviewuser = "";
                        if (spotproblem.ReviewUser != null)
                        {
                            int reviewuserid = int.Parse(spotproblem.ReviewUser);
                            reviewuser = userlist.FirstOrDefault(x => x.ID == reviewuserid).RealName;
                        }
                        if (dormodel != null)
                        {
                            name += "{\"ProDesc\":\"" + spotproblem.ProDesc + "\",\"DutyUser\":\"" + dutyuser + "\",\"CreateUser\":\"" + createuser + "\",\"CreateDate\":\"" + Convert.ToDateTime(spotproblem.CreateDate).ToShortDateString() + "\",\"SpotScore\":\"" + model.SpotScore + "\",\"DormiName\":\"" + dormodel.DormiName + "\",\"IsreView\":\"" + isreview + "\",\"SpId\":\"" + spotproblem.SpId + "\",\"ReviewUser\":\"" + reviewuser + "\",\"ReviewMemo\":\"" + spotproblem.ReviewMemo + "\",\"SpotId\":\"" + model.SpotId + "\"},";
                        }
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
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
        #endregion

        #region 根据宿舍ID问题统计详情分页
        public void GetProblemDetailPageListByDromitoryId(HttpContext context)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //1 问题统计  2 问题复查
                    int flag = Convert.ToInt32(context.Request.Params["flag"]);
                    int pageindex = Convert.ToInt32(context.Request.Params["pageindex"]);
                    int pagesize = Convert.ToInt32(context.Request.Params["pagesize"]);
                    int DormitorySel = Convert.ToInt32(context.Request.Params["dormitoryid"]);
                    if (DormitorySel == -2)
                    {
                        throw new Exception();
                    }
                    List<TempSpotProblem> SpotProblemList = new List<TempSpotProblem>();
                    if (flag == 2)
                    {
                        SpotProblemList = db.SpotProblem.OrderByDescending(t => t.CreateDate).Where(x => x.IsreView == false).Select(t =>
                            new TempSpotProblem
                            {
                                CreateDate = t.CreateDate,
                                CreateUser = t.CreateUser,
                                DutyUser = t.DutyUser,
                                IsreView = t.IsreView,
                                ProDesc = t.ProDesc,
                                ReviewDate = t.ReviewDate,
                                ReviewMemo = t.ReviewMemo,
                                ReviewUser = t.ReviewUser,
                                SpId = t.SpId,
                                SpotId = t.SpotId
                            }).OrderByDescending(t => t.CreateDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    }
                    else
                    {
                        SpotProblemList = db.SpotProblem.Select(t =>
                            new TempSpotProblem
                            {
                                CreateDate = t.CreateDate,
                                CreateUser = t.CreateUser,
                                DutyUser = t.DutyUser,
                                IsreView = t.IsreView,
                                ProDesc = t.ProDesc,
                                ReviewDate = t.ReviewDate,
                                ReviewMemo = t.ReviewMemo,
                                ReviewUser = t.ReviewUser,
                                SpId = t.SpId,
                                SpotId = t.SpotId
                            }).OrderByDescending(t => t.CreateDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    }
                    string name = "";
                    List<User> userlist = db.User.ToList();
                    List<SpotCheck> spotchecklist = db.SpotCheck.ToList();
                    List<Dormitory> dormitorylist = db.Dormitory.ToList();
                    foreach (TempSpotProblem spotproblem in SpotProblemList)
                    {
                        var isreview = spotproblem.IsreView.ToString() == "False" ? "否" : "是";
                        SpotCheck model = spotchecklist.FirstOrDefault(x => x.SpotId == spotproblem.SpotId);
                        Dormitory dormodel = dormitorylist.FirstOrDefault(x => x.ID == DormitorySel);
                        int duryuserid = int.Parse(spotproblem.DutyUser);
                        var dutyuser = userlist.FirstOrDefault(x => x.ID == duryuserid).RealName;
                        int createuserid = int.Parse(spotproblem.CreateUser);
                        var createuser = userlist.FirstOrDefault(x => x.ID == createuserid).RealName;
                        var reviewuser = "";
                        if (spotproblem.ReviewUser != null)
                        {
                            int reviewuserid = int.Parse(spotproblem.ReviewUser);
                            reviewuser = userlist.FirstOrDefault(x => x.ID == reviewuserid).RealName;
                        }
                        if (dormodel != null && model.DormitoryId == DormitorySel)
                        {
                            name += "{\"ProDesc\":\"" + spotproblem.ProDesc + "\",\"DutyUser\":\"" + dutyuser + "\",\"CreateUser\":\"" + createuser + "\",\"CreateDate\":\"" + Convert.ToDateTime(spotproblem.CreateDate).ToShortDateString() + "\",\"SpotScore\":\"" + model.SpotScore + "\",\"DormiName\":\"" + dormodel.DormiName + "\",\"IsreView\":\"" + isreview + "\",\"SpId\":\"" + spotproblem.SpId + "\",\"ReviewUser\":\"" + reviewuser + "\",\"ReviewMemo\":\"" + spotproblem.ReviewMemo + "\",\"SpotId\":\"" + model.SpotId + "\"},";
                        }
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
            }
            catch
            {
                sb.Append("{\"result\":\"true\",\"data\":[]}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion

        #region 宿舍评分
        public void GetGrade(HttpContext context)
        {
            try
            {
                int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                //int uid = 1;
                int SpotScore = Convert.ToInt32(context.Request.Params["SpotScore"]);
                int DormitorySel = Convert.ToInt32(context.Request.Params["DormitorySel"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    var user = db.User.FirstOrDefault(x => x.ID == uid);
                    SpotCheck SpotCheck = new SpotCheck();
                    SpotCheck.SpotScore = SpotScore;
                    SpotCheck.DormitoryId = DormitorySel;
                    SpotCheck.CreateUser = user.ID;
                    SpotCheck.CreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    db.SpotCheck.Add(SpotCheck);
                    db.SaveChanges();
                    var name = "{\"id\":\"" + SpotCheck.SpotId + "\",\"dormitoryid\":\"" + DormitorySel + "\"}";

                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
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
        #endregion

        #region 最优最差宿舍
        public void GetBestorBad(HttpContext context)
        {

            int begin = Convert.ToInt32(context.Request.Params["begin"]);
            int end = Convert.ToInt32(context.Request.Params["end"]);
            var year = DateTime.Now.Year;
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var month = 12;
                if (begin == year)
                {
                    month = DateTime.Now.Month;
                }
                var result = "";
                List<Dormitory> DormitoryList = db.Dormitory.ToList();
                for (int j = 0; j <= end - year; j++)
                {
                    for (int i = 1; i <= month; i++)
                    {
                        var name = "";
                        StringBuilder sbscore = new StringBuilder("");
                        var valdate = begin + j;
                        var BeginDate = Convert.ToDateTime(valdate + "-" + i + "-01");
                        var EndDate = Convert.ToDateTime(valdate + "-" + (i + 1) + "-01");
                        foreach (Dormitory dormitory in DormitoryList)
                        {
                            List<SpotCheck> spotchecklist = db.SpotCheck.Where(x => x.CreateDate >= BeginDate && x.CreateDate < EndDate && x.DormitoryId == dormitory.ID).ToList();
                            if (spotchecklist.Count > 0)
                            {
                                if (spotchecklist.Count > 1)
                                {
                                    int score = 0;
                                    score = spotchecklist.Sum(x => x.SpotScore);
                                    name += "{\"date\":\"" + i + "\",\"score\":\"" + score + "\",\"dormitory\":\"" + dormitory.DormiName + "\"},";
                                }
                                else
                                {
                                    name += "{\"date\":\"" + i + "\",\"score\":\"" + spotchecklist[0].SpotScore + "\",\"dormitory\":\"" + dormitory.DormiName + "\"},";
                                }
                            }
                        }
                        sbscore.Remove(0, sbscore.Length);
                        sbscore.Append("{\"result\":\"true\",\"data\":[");
                        sbscore.Append(name.TrimEnd(','));
                        sbscore.Append("]}");
                        var dtYear = JsonToDataTable(sbscore.ToString());
                        string text = "";
                        StringBuilder sbscorenew = new StringBuilder("");
                        DataTable dtYearNew = new DataTable();
                        if (dtYear != null && dtYear.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtYear.Rows.Cast<DataRow>().OrderByDescending(r => int.Parse(r["score"].ToString())))
                            {
                                text += "{\"dormitory\":\"" + row["dormitory"] + "\",\"score\":\"" + row["score"] + "\",\"date\":\"" + row["date"] + "\"},";
                            }
                            sbscorenew.Append("{\"result\":\"true\",\"data\":[");
                            sbscorenew.Append(text.TrimEnd(','));
                            sbscorenew.Append("]}");
                            dtYearNew = JsonToDataTable(sbscorenew.ToString());
                        }
                        if (dtYearNew.Rows.Count > 0)
                        {
                            var max = dtYearNew.AsEnumerable().First<DataRow>()["score"];
                            var dr_first = dtYearNew.AsEnumerable().First<DataRow>()["dormitory"];
                            var min = dtYearNew.AsEnumerable().Last<DataRow>()["score"];
                            var dr_last = dtYearNew.AsEnumerable().Last<DataRow>()["dormitory"];
                            string date = dtYearNew.Rows[0]["date"].ToString();
                            result += "{\"max\":\"" + max + "\",\"min\":\"" + min + "\",\"date\":\"" + (begin + j) + "-" + date + "\",\"dr_first\":\"" + dr_first + "\",\"dr_last\":\"" + dr_last + "\"},";

                        }
                    }
                }
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(result.TrimEnd(','));
                sb.Append("]}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion

        #region 最优最差宿舍并列的情况
        public void GetBestorBadMore(HttpContext context)
        {
            int begin = Convert.ToInt32(context.Request.Params["begin"]);
            int end = Convert.ToInt32(context.Request.Params["end"]);
            var year = DateTime.Now.Year;
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var month = 12;
                if (begin == year)
                {
                    month = DateTime.Now.Month;
                }
                var result = "";
                List<Dormitory> DormitoryList = db.Dormitory.ToList();
                List<SpotCheck> spotcheck = db.SpotCheck.ToList();
                for (int j = 0; j <= end - year; j++)
                {
                    for (int i = 1; i <= month; i++)
                    {
                        var name = "";
                        StringBuilder sbscore = new StringBuilder("");
                        var valdate = begin + j;
                        var BeginDate = Convert.ToDateTime(valdate + "-" + i + "-01");
                        var EndDate = new DateTime();
                        if (i == 12)
                        {
                            EndDate = Convert.ToDateTime((valdate + 1) + "-" + "1" + "-01");
                        }
                        else
                        {
                            EndDate = Convert.ToDateTime(valdate + "-" + (i + 1) + "-01");
                        }
                        foreach (Dormitory dormitory in DormitoryList)
                        {
                            var isthere = spotcheck.FirstOrDefault(x => x.CreateDate >= BeginDate && x.CreateDate < EndDate && x.DormitoryId == dormitory.ID);
                            var spotchecklist = spotcheck.Where(x => x.CreateDate >= BeginDate && x.CreateDate < EndDate && x.DormitoryId == dormitory.ID).ToList();
                            if (isthere != null)
                            {
                                if (spotchecklist.Count > 1)
                                {
                                    int score = 0;
                                    score = spotchecklist.Sum(x => x.SpotScore);
                                    name += "{\"date\":\"" + i + "\",\"score\":\"" + score + "\",\"dormitory\":\"" + dormitory.DormiName + "\"},";
                                }
                                else
                                {
                                    name += "{\"date\":\"" + i + "\",\"score\":\"" + spotchecklist[0].SpotScore + "\",\"dormitory\":\"" + dormitory.DormiName + "\"},";
                                }
                            }
                        }
                        sbscore.Remove(0, sbscore.Length);
                        sbscore.Append("{\"result\":\"true\",\"data\":[");
                        sbscore.Append(name.TrimEnd(','));
                        sbscore.Append("]}");
                        var dtYear = JsonToDataTable(sbscore.ToString());
                        string text = "";
                        StringBuilder sbscorenew = new StringBuilder("");
                        DataTable dtYearNew = new DataTable();
                        if (dtYear != null && dtYear.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtYear.Rows.Cast<DataRow>().OrderByDescending(r => int.Parse(r["score"].ToString())))
                            {
                                text += "{\"dormitory\":\"" + row["dormitory"] + "\",\"score\":\"" + row["score"] + "\",\"date\":\"" + row["date"] + "\"},";
                            }
                            sbscorenew.Append("{\"result\":\"true\",\"data\":[");
                            sbscorenew.Append(text.TrimEnd(','));
                            sbscorenew.Append("]}");
                            dtYearNew = JsonToDataTable(sbscorenew.ToString());
                        }
                        if (dtYearNew.Rows.Count > 0)
                        {
                            var maxDor = "";
                            var minDor = "";
                            var max = Convert.ToInt32(dtYearNew.AsEnumerable().First<DataRow>()["score"]);
                            var min = Convert.ToInt32(dtYearNew.AsEnumerable().Last<DataRow>()["score"]);
                            string date = dtYearNew.Rows[0]["date"].ToString();

                            for (int x = 0; x < dtYearNew.Rows.Count; x++)
                            {
                                int strScore = int.Parse(dtYearNew.Rows[x]["score"].ToString());
                                string strName = dtYearNew.Rows[x]["dormitory"].ToString();
                                if (strScore == max)
                                {
                                    maxDor += strName + ",";
                                }
                                if (strScore == min)
                                {
                                    minDor += strName + ",";
                                }
                            }

                            result += "{\"max\":\"" + max + "\",\"min\":\"" + min + "\",\"date\":\"" + (begin + j) + "-" + date + "\",\"maxDor\":\"" + maxDor.TrimEnd(',') + "\",\"minDor\":\"" + minDor.TrimEnd(',') + "\"},";

                        }
                    }
                }
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(result.TrimEnd(','));
                sb.Append("]}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion

        #region 获取复查详情页数据
        public void GetReviewDetail(HttpContext context)
        {
            int SpId = Convert.ToInt32(context.Request.Params["SpId"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var name = "";
                SpotProblem model = db.SpotProblem.FirstOrDefault(x => x.SpId == SpId);
                SpotCheck spotcheck = db.SpotCheck.FirstOrDefault(x => x.SpotId == model.SpotId);
                Dormitory dormitory = db.Dormitory.FirstOrDefault(x => x.ID == spotcheck.DormitoryId);

                string problem = "";
                if (model.SelectDesc != "" && model.SelectDesc != null)
                {
                    var problemarray = model.SelectDesc.Split(',');
                    for (int i = 0; i < problemarray.Length; i++)
                    {
                        int id = int.Parse(problemarray[i]);
                        problem += db.SpotSelectProblem.FirstOrDefault(x => x.ID == id).Problem + "&";
                    }
                }

                int duryuserid = int.Parse(model.DutyUser);
                var dutyuser = db.User.FirstOrDefault(x => x.ID == duryuserid).RealName;
                int createuserid = int.Parse(model.CreateUser);
                var createuser = db.User.FirstOrDefault(x => x.ID == createuserid).RealName;
                string img = "";
                if (model.SImage.ToString() == "")
                {
                    img = "";
                }
                else
                {
                    if (model.SImage.ToString().Substring(0, 8) == "Templete")
                    {
                        img = "../" + model.SImage.ToString().Replace("\\", "/");
                    }
                    else
                    {
                        img = model.SImage.ToString().Replace("\\", "/");
                    }
                }

                name += "{\"ProDesc\":\"" + model.ProDesc + "\",\"DutyUser\":\"" + dutyuser + "\",\"SImage\":\"" + img + "\",\"CreateUser\":\"" + createuser + "\",\"dormitory\":\"" + dormitory.DormiName + "\",\"Problem\":\"" + problem.TrimEnd('&') + "\"},";
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(name.TrimEnd(','));
                sb.Append("]}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion

        #region  保存复查意见
        public void SaveReviewDetail(HttpContext context)
        {
            try
            {
                int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                //int uid = 6;
                var Review = context.Request.Params["Review"];
                int SpId = Convert.ToInt32(context.Request.Params["SpId"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //var enumvalue = (WeekDate)Enum.Parse(typeof(WeekDate), DateTime.Now.DayOfWeek.ToString("d"));
                    //var checkname = db.Scheduling.FirstOrDefault(x => x.Date == enumvalue).CheckName;
                    if (string.Join(",", db.Scheduling.Select(t => t.CheckName)).Split(',').Any(x => x == uid.ToString()))
                    {
                        SpotProblem model = db.SpotProblem.FirstOrDefault(x => x.SpId == SpId);
                        model.ReviewMemo = Review;
                        model.ReviewUser = db.User.FirstOrDefault(x => x.ID == uid).ID.ToString();
                        model.ReviewDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        model.IsreView = true;
                        db.SaveChanges();
                        sb.Append("{\"result\":\"true\"}");
                    }
                    else
                    {
                        throw new Exception();
                    }
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
        #endregion

        #region  获取问题图片详情
        public void ImageDetail(HttpContext context)
        {
            int SpId = Convert.ToInt32(context.Request.Params["SpId"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var name = "";
                SpotProblem SpotProblem = db.SpotProblem.FirstOrDefault(x => x.SpId == SpId);
                string problem = "";
                if (SpotProblem.SelectDesc != "" && SpotProblem.SelectDesc != null)
                {
                    var problemarray = SpotProblem.SelectDesc.Split(',');
                    for (int i = 0; i < problemarray.Length; i++)
                    {
                        int id = int.Parse(problemarray[i]);
                        problem += db.SpotSelectProblem.FirstOrDefault(x => x.ID == id).Problem + "&";
                    }
                }
                string img = "";
                if (SpotProblem.SImage.ToString() == "")
                {
                    img = "";
                }
                else
                {
                    if (SpotProblem.SImage.ToString().Substring(0, 8) == "Templete")
                    {
                        img = "../" + SpotProblem.SImage.ToString().Replace("\\", "/");
                    }
                    else
                    {
                        img = SpotProblem.SImage.ToString().Replace("\\", "/");
                    }
                }

                name += "{\"CreateDate\":\"" + Convert.ToDateTime(SpotProblem.CreateDate).ToShortDateString() + "\",\"SImage\":\"" + img + "\",\"ProDesc\":\"" + SpotProblem.ProDesc + "\",\"Problem\":\"" + problem.TrimEnd('&') + "\"},";
                sb.Append("{\"result\":\"true\",\"data\":[");
                sb.Append(name.TrimEnd(','));
                sb.Append("]}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        #endregion

        #region json
        public DataTable JsonToDataTable(string strJson)
        {
            DataTable dt = null;
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);
                bool success = (bool)jo["result"];
                if (!success)
                {
                    return null;
                }
                JArray ja = (JArray)jo["data"];
                dt = ToDataTable(ja.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
            return dt;
        }

        public DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();
            DataTable result;
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
            if (arrayList.Count > 0)
            {
                foreach (Dictionary<string, object> dictionary in arrayList)
                {
                    if (dictionary.Keys.Count == 0)
                    {
                        result = dataTable;
                        return result;
                    }
                    if (dataTable.Columns.Count == 0)
                    {
                        foreach (string current in dictionary.Keys)
                        {
                            dataTable.Columns.Add(current, dictionary[current].GetType());
                        }
                    }
                    DataRow dataRow = dataTable.NewRow();
                    foreach (string current in dictionary.Keys)
                    {
                        dataRow[current] = dictionary[current];
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
            result = dataTable;
            return result;
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