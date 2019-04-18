using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using IFMPLibrary.DAO;

namespace JFZAPP.ashx
{
    /// <summary>
    /// Integral 的摘要说明
    /// </summary>
    public class Integral : IHttpHandler
    {
        private StringBuilder sb = new StringBuilder("");
        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Params["method"];
            switch (method)
            {
                case "MyIntegral":
                    MyIntegral(context);
                    break;
                case "AList":
                    AuditList(context);
                    break;
                case "IntegralDetail":
                    IntegralDetail(context);
                    break;
                case "IntegralDetai":
                    IntegralDetai(context);
                    break;
                case "GetUser":
                    GetUser(context);
                    break;

            }
        }

        public void GetUser(HttpContext context)
        {
            try
            {
                int sid = Convert.ToInt32(context.Request.Params["sid"]);

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.ScoreID == sid && t.IsDel != true).ToList();
                    if (ScoreUserList.Count > 0)
                    {
                        string name = "";
                        foreach (ScoreUser ScoreUser in ScoreUserList)
                        {
                            name += "{\"RewardUserName\":\"" + db.User.FirstOrDefault(t => t.ID == ScoreUser.UserID).RealName
                                  + "\",\"BScore\":\"" + ScoreUser.BScore
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
            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        public void MyIntegral(HttpContext context)
        {
            try
            {
                int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                string month = context.Request.Params["date"];
                int sely = DateTime.Now.Year, selm = DateTime.Now.Month;
                if (!string.IsNullOrEmpty(month))
                {
                    try
                    {
                        sely = int.Parse(month.Split('-')[0]);
                        selm = int.Parse(month.Split('-')[1]);
                    }
                    catch (Exception)
                    {

                    }
                }

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //统计所有人的积分
                    List<User> MonthUserList = db.User.Where(t => t.IsDel != true && !db.NoScoreUser.Select(m => m.UserID).Contains(t.ID)).ToList();
                    List<User> YearUserList = db.User.Where(t => t.IsDel != true && !db.NoScoreUser.Select(m => m.UserID).Contains(t.ID)).ToList();
                    List<User> TotalUserList = db.User.Where(t => t.IsDel != true && !db.NoScoreUser.Select(m => m.UserID).Contains(t.ID)).ToList();
                    List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.IsDel != true).ToList();
                    List<Score> ScoreList = db.Score.Where(t => t.IsDel != true && t.AuditState == AuditState.通过).ToList();
                    foreach (User User in MonthUserList)
                    {
                        User.PlusTotal = ScoreUserList.Where(t => t.BScore > 0 && t.UserID == User.ID && ScoreList.Where(m => m.IsDel != true && m.CreateDate.Value.Month == selm).Select(m => m.ID).Contains(t.ScoreID)).Sum(t => t.BScore);
                        User.NegativeTotal = ScoreUserList.Where(t => t.BScore < 0 && t.UserID == User.ID && ScoreList.Where(m => m.IsDel != true && m.CreateDate.Value.Month == selm).Select(m => m.ID).Contains(t.ScoreID)).Sum(t => t.BScore);
                        User.Total = User.PlusTotal + User.NegativeTotal;
                    }

                    foreach (User User in YearUserList)
                    {
                        User.PlusTotal = ScoreUserList.Where(t => t.BScore > 0 && t.UserID == User.ID && ScoreList.Where(m => m.IsDel != true && m.CreateDate.Value.Year == sely).Select(m => m.ID).Contains(t.ScoreID)).Sum(t => t.BScore);
                        User.NegativeTotal = ScoreUserList.Where(t => t.BScore < 0 && t.UserID == User.ID && ScoreList.Where(m => m.IsDel != true && m.CreateDate.Value.Year == sely).Select(m => m.ID).Contains(t.ScoreID)).Sum(t => t.BScore);
                        User.Total = User.PlusTotal + User.NegativeTotal;
                    }

                    foreach (User User in TotalUserList)
                    {
                        User.PlusTotal = ScoreUserList.Where(t => t.BScore > 0 && t.UserID == User.ID && ScoreList.Where(m => m.IsDel != true).Select(m => m.ID).Contains(t.ScoreID)).Sum(t => t.BScore);
                        User.NegativeTotal = ScoreUserList.Where(t => t.BScore < 0 && t.UserID == User.ID && ScoreList.Where(m => m.IsDel != true).Select(m => m.ID).Contains(t.ScoreID)).Sum(t => t.BScore);
                        User.Total = User.PlusTotal + User.NegativeTotal;
                    }

                    string name = "";
                    foreach (User User in MonthUserList.OrderByDescending(t => t.Total).ToList())
                    {
                        name += "{\"zf\":\"" + User.Total
                            + "\",\"jf\":\"" + User.PlusTotal
                            + "\",\"kf\":\"" + User.NegativeTotal
                            + "\",\"mrank\":\"" + MonthUserList.Count(t => t.Total > User.Total) + 1
                            + "\",\"yrank\":\"" + YearUserList.Count(t => t.Total > YearUserList.FirstOrDefault(m => m.ID == User.ID).Total) + 1
                            + "\",\"zrank\":\"" + TotalUserList.Count(t => t.Total > TotalUserList.FirstOrDefault(m => m.ID == User.ID).Total) + 1
                            + "\",\"mscore\":\"" + User.Total
                            + "\",\"yscore\":\"" + YearUserList.FirstOrDefault(t => t.ID == User.ID).Total
                            + "\",\"zscore\":\"" + TotalUserList.FirstOrDefault(t => t.ID == User.ID).Total
                            + "\",\"stype\":\"" + 1
                            + "\",\"typescore\":\"" + User.Total
                            + "\"},";
                    }
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        public void AuditList(HttpContext context)
        {
            try
            {
                int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                int flag = int.Parse(context.Request.Params["flag"]);
                int pageindex = Convert.ToInt32(context.Request.Params["pageindex"]);
                int pagesize = Convert.ToInt32(context.Request.Params["pagesize"]);

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //flag=1 未审核
                    //flag=2 已审核
                    //flag=-1 审核中
                    //flag=-2 已完成
                    //其他 全部
                    List<Score> ScoreList = new List<Score>();
                    switch (flag)
                    {
                        case 1:
                            ScoreList = db.Score.Where(t => t.IsDel != true && ((t.AuditState == AuditState.待初审 && t.FirstAuditUserID == uid) || (t.AuditState == AuditState.待终审 && t.LastAuditUserID == uid))).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                            break;
                        case 2:
                            ScoreList = db.Score.Where(t => t.IsDel != true && ((t.AuditState == AuditState.待终审 && t.FirstAuditUserID == uid) || ((t.AuditState == AuditState.通过 || t.AuditState == AuditState.驳回) && t.LastAuditUserID == uid))).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                            break;
                        case -1:
                            ScoreList = db.Score.Where(t => t.CreateUserID == uid && (t.AuditState == AuditState.待初审 || t.AuditState == AuditState.待终审)).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                            break;
                        case -2:
                            ScoreList = db.Score.Where(t => t.CreateUserID == uid && (t.AuditState == AuditState.通过 || t.AuditState == AuditState.驳回)).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                            break;
                        default:
                            ScoreList = db.Score.Where(t => t.IsDel != true && t.AuditState != AuditState.待初审 && (t.FirstAuditUserID == uid || t.LastAuditUserID == uid)).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                            break;
                    }

                    if (ScoreList.Count > 0)
                    {
                        string name = "";
                        List<User> UserList = db.User.ToList();
                        List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true).ToList();
                        List<DepartmentUser> DepartmentUserList = db.DepartmentUser.ToList();
                        List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.IsDel != true).ToList();
                        //User User = db.User.FirstOrDefault(t=>t.ID==uid);
                        //Department Department = db.Department.FirstOrDefault(t=>t.ID==db.DepartmentUser.FirstOrDefault(m=>m.UserID==uid).DepartmentID);
                        foreach (Score Score in ScoreList)
                        {
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

                            name += "{\"STitle\":\"" + Score.Title
                             + "\",\"SID\":\"" + Score.ID
                             + "\",\"EventName\":\"" + Score.ScoreEventID
                             + "\",\"EventMark\":\"" + Score.Content
                             + "\",\"SImage\":\"" + img
                             + "\",\"SType\":\"" + 1
                             + "\",\"VDate\":\"" + Convert.ToDateTime(Score.CreateDate).ToString("yyyy-MM-dd")
                             + "\",\"AduitState\":\"" + Score.AuditState
                             + "\",\"AduitStateName\":\"" + Enum.GetName(typeof(AuditState), Score.AuditState)
                             + "\",\"RealName\":\"" + UserList.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                             + "\",\"UserList\":\"" + new ScoreUserDAO().GetScoreUser(Score.ID)
                             + "\",\"DepName\":\"" + DepartmentList.FirstOrDefault(t => DepartmentUserList.Where(m => m.UserID == Score.CreateUserID).Select(m => m.DepartmentID).Contains(t.ID) && t.IsAdmin).Name
                              + "\",\"FirstAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == Score.FirstAuditUserID).RealName
                             + "\",\"LastAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == Score.LastAuditUserID).RealName
                               + "\",\"BScore\":\"" + ScoreUserList.Where(t => t.ScoreID == Score.ID).Sum(t => t.BScore)
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
            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        public void IntegralDetail(HttpContext context)
        {
            try
            {
                int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                string month = context.Request.Params["m"];
                string aa = context.Request.Params["RealName"];
                string username = HttpUtility.UrlDecode(aa);
                int sely = DateTime.Now.Year, selm = DateTime.Now.Month;
                if (!string.IsNullOrEmpty(month))
                {
                    try
                    {
                        sely = int.Parse(month.Split('-')[0]);
                        selm = int.Parse(month.Split('-')[1]);
                    }
                    catch (Exception)
                    {

                    }
                }

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Score> ScoreList = db.Score.Where(t => t.IsDel != true
                        && t.CreateDate.Value.Date.Year == sely
                        && t.CreateDate.Value.Date.Month == selm
                        && t.AuditState == AuditState.通过
                        && db.ScoreUser.Where(m => m.IsDel != true && m.UserID == uid).Select(m => m.ScoreID).Contains(t.ID)).ToList();

                    if (ScoreList.Count > 0)
                    {
                        string name = "";
                        List<ScoreEvent> ScoreEventList = db.ScoreEvent.ToList();
                        List<User> UserList = db.User.ToList();
                        List<ScoreUser> ScoreUser = db.ScoreUser.ToList();
                        foreach (Score Score in ScoreList)
                        {
                            name += "{\"STitle\":\"" + Score.Title
                            + "\",\"SID\":\"" + Score.ID
                            + "\",\"EventNames\":\"" + ScoreEventList.FirstOrDefault(t => t.ID == Score.ScoreEventID).Name
                            + "\",\"FirstAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == Score.FirstAuditUserID).RealName
                            + "\",\"LastAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == Score.LastAuditUserID).RealName
                             + "\",\"AduitState\":\"" + Score.AuditState
                             + "\",\"AduitStateName\":\"" + Enum.GetName(typeof(AuditState), Score.AuditState)
                             + "\",\"AduitDate\":\"" + (Score.LastAuditDate == null ? "" : Score.LastAuditDate.Value.ToString("yyyy-MM-dd"))
                            + "\",\"BSCore\":\"" + ScoreUser.Where(t => t.ScoreID == Score.ID).Sum(t => t.BScore)
                            + "\"},";
                        }
                        sb.Append("{\"result\":\"true\",\"name\":\"" + username + "\",\"data\":[");
                        sb.Append(name.TrimEnd(','));
                        sb.Append("]}");
                    }
                    else
                    {
                        sb.Append("{\"result\":\"true\",\"name\":\"" + username + "\",\"data\":[]}");
                    }
                }
            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
        }
        public void IntegralDetai(HttpContext context)
        {
            try
            {
                int sid = Convert.ToInt32(context.Request.Params["id"]);
                int uid = Convert.ToInt32(context.Request.Params["UserID"]);

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    Score Score = db.Score.FirstOrDefault(t => t.ID == sid);
                    List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList();
                    if (ScoreUserList.Count > 0)
                    {
                        string name = "";
                        List<ScoreEvent> ScoreEventList = db.ScoreEvent.ToList();
                        List<User> UserList = db.User.ToList();
                        foreach (ScoreUser ScoreUser in ScoreUserList)
                        {
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

                            name += "{\"STitle\":\"" + Score.Title
                            + "\",\"SID\":\"" + Score.ID
                            + "\",\"VDate\":\"" + Convert.ToDateTime(Score.CreateDate.Value).ToString("yyyy-MM-dd")
                            + "\",\"EventNames\":\"" + ScoreEventList.FirstOrDefault(t => t.ID == Score.ScoreEventID).Name
                            + "\",\"EventMark\":\"" + Score.Content
                            + "\",\"BSCore\":\"" + ScoreUser.BScore
                            + "\",\"SImage\":\"" + img
                            + "\",\"VUserName\":\"" + UserList.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName
                            + "\",\"FirstAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == Score.FirstAuditUserID).RealName
                            + "\",\"LastAduitUserName\":\"" + UserList.FirstOrDefault(t => t.ID == Score.LastAuditUserID).RealName
                            + "\",\"AduitState\":\"" + Score.AuditState
                            + "\",\"AduitStateName\":\"" + Enum.GetName(typeof(AuditState), Score.AuditState)
                            + "\",\"FirstAduitDate\":\"" + (Score.FirstAuditDate == null ? "" : Score.FirstAuditDate.Value.ToString("yyyy-MM-dd"))
                            + "\",\"LastAduitDate\":\"" + (Score.LastAuditDate == null ? "" : Score.LastAuditDate.Value.ToString("yyyy-MM-dd"))
                                   + "\",\"FirstAduitMark\":\"" + Score.FirstAuditMark
                                          + "\",\"LastAduitMark\":\"" + Score.LastAuditMark
                            + "\"},";


                        }
                        sb.Append("{\"result\":\"true\",\"data\":[");
                        sb.Append(name.TrimEnd(','));
                        sb.Append("]}");
                    }
                    else
                    {
                        sb.Append("{\"result\":\"true\",\"data\":[]}");
                    }
                }

            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString().TrimEnd(','));
            context.Response.End();
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