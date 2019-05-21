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
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace DormitoryAPP.ashx
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
                case "IsCheckUser":
                    IsCheckUser(context);
                    break;
                case "IsCheckUserToday":
                    IsCheckUserToday(context);
                    break;
            }
        }

        #region 判断是否是当天的点检人
        public void IsCheckUserToday(HttpContext context)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                    //int uid = 6;
                    string name = "";
                    var enumvalue = (WeekDate)Enum.Parse(typeof(WeekDate), DateTime.Now.DayOfWeek.ToString("d"));
                    enumvalue = enumvalue == 0 ? WeekDate.星期日 : enumvalue;
                    var checkname = db.Scheduling.FirstOrDefault(x => x.Date == enumvalue).CheckName;
                    if (checkname.Split(',').Any(x => x == uid.ToString()))
                    {
                        name += "{\"value\":\"" + "1"  + "\"}";
                    }
                    else
                    {
                        List<Scheduling> scheduinglist = db.Scheduling.ToList();
                        string checknameid = "";
                        foreach (var scheduing in scheduinglist)
                        {
                            checknameid += scheduing.CheckName + ",";
                        }
                        var check = checknameid.TrimEnd(',').Split(',').Distinct().ToArray();
                        if (check.Any(x => x == uid.ToString()))
                        {
                            name += "{\"value\":\"" + "2" + "\"}";
                        }
                        else
                        {
                            name += "{\"value\":\"" + "3" + "\"}";
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

        #region 判断是否是点检人
        public void IsCheckUser(HttpContext context)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                    //int uid = 6;
                    List<Scheduling> scheduinglist = db.Scheduling.ToList();
                    string checknameid = "";
                    foreach (var scheduing in scheduinglist)
                    {
                        checknameid += scheduing.CheckName + ",";
                    }
                    var checkname = checknameid.TrimEnd(',').Split(',').Distinct().ToArray();
                    if (checkname.Any(x => x == uid.ToString()))
                    {
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

        #region 首页我的宿舍
        public void MyIntegral(HttpContext context)
        {
            try
            {
                StringBuilder sbscore = new StringBuilder("");
                StringBuilder sbscoreMonth = new StringBuilder("");
                //int UserID = 6;
                int UserID = Convert.ToInt32(context.Request.Params["UserID"]);
                string uid = UserID.ToString();
                //string uid = context.Request.Params["UserID"];
                //string uid = "200";
                int sely = DateTime.Now.Year, selm = DateTime.Now.Month;
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    var user = db.User.FirstOrDefault(x => x.ID == UserID);
                    if (user == null)
                    {
                        throw new Exception();
                    }
                    List<DormitoryUser> dormituser = db.DormitoryUser.ToList();
                    var isin = dormituser.Count;
                    int i = 0;
                    foreach (var dor in dormituser)
                    {   
                        if (!dor.UserID.Split(',').Any(x => x == uid))
                        {
                            i = i + 1;
                        }
                        if (isin == i)
                        {
                            throw new Exception();
                        }
                    }
                    var whatdormitory = "";
                    var beginYear = Convert.ToDateTime(sely + "-01" + "-01");
                    var EndYear = Convert.ToDateTime((sely + 1) + "-01" + "-01");
                    var BeginMonth = Convert.ToDateTime(sely + "-" + selm + "-01");
                    var EndMonth = Convert.ToDateTime(sely + "-" + (selm + 1) + "-01");
                    var name = "";
                    name += "{";
                    //年度
                    int yearscore = 0;
                    List<Dormitory> DormitoryListYear = db.Dormitory.ToList();
                    List<DormitoryUser> dormitoryuserlist = db.DormitoryUser.ToList();
                    List<SpotCheck> spotchecklist = db.SpotCheck.ToList();
                    foreach (Dormitory dormitory in DormitoryListYear)
                    {
                        if (dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == dormitory.ID)!=null)
                        {
                            var dormitoryuser = dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == dormitory.ID).UserID;
                            if (dormitoryuser.Split(',').Any(x => x == uid))
                            {
                                whatdormitory = dormitory.DormiName;
                                List<SpotCheck> model = spotchecklist.Where(x => x.DormitoryId == dormitory.ID && x.CreateDate >= beginYear && x.CreateDate < EndYear).ToList();

                                if (model.Count > 1)
                                {
                                    int score = 0;
                                    score = model.Sum(x => x.SpotScore);
                                    name += "\"YearScore\":\"" + score + "\",";
                                    yearscore = score;
                                }
                                else if (model.Count == 1)
                                {
                                    name += "\"YearScore\":\"" + model[0].SpotScore + "\",";
                                    yearscore = model[0].SpotScore;
                                }
                                else if (model.Count == 0)
                                {
                                    name += "\"YearScore\":\"" + 0 + "\",";
                                }
                            }
                        }
                    }
                    var YearRanking = "";
                    var MonthRanking = "";
                    //年度排名
                    foreach (Dormitory dormitory in DormitoryListYear)
                    {
                        List<SpotCheck> model = spotchecklist.Where(x => x.DormitoryId == dormitory.ID && x.CreateDate >= beginYear && x.CreateDate < EndYear).ToList();
                        
                        if (model.Count > 1)
                        {
                            int score = 0;
                            score = model.Sum(x => x.SpotScore);
                            YearRanking += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + score + "\"},";
                        }
                        else if (model.Count == 1)
                        {
                            YearRanking += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + model[0].SpotScore + "\"},";
                        }
                        else if (model.Count == 0)
                        {
                            YearRanking += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + 0 + "\"},";
                        }
                    }
                    sbscore.Append("{\"result\":\"true\",\"data\":[");
                    sbscore.Append(YearRanking.TrimEnd(','));
                    sbscore.Append("]}");
                    var dtYear = JsonToDataTable(sbscore.ToString());
                    StringBuilder scoreyear = new StringBuilder("");
                    DataTable dtYearNew = new DataTable();
                    if (dtYear != null && dtYear.Rows.Count > 0)
                    {
                        string textyear = "";
                        foreach (DataRow row in dtYear.Rows.Cast<DataRow>().OrderByDescending(r => int.Parse(r["score"].ToString())))
                        {
                            textyear += "{\"name\":\"" + row["name"] + "\",\"score\":\"" + row["score"] + "\"},";
                        }
                        scoreyear.Append("{\"result\":\"true\",\"data\":[");
                        scoreyear.Append(textyear.TrimEnd(','));
                        scoreyear.Append("]}");
                        dtYearNew = JsonToDataTable(scoreyear.ToString());
                    }
                    var selfyeardt = GetDistinctSelf(dtYearNew, "score");
                    var rowscoreyear = string.Format("score='{0}'", yearscore);
                    DataRow[] rows = selfyeardt.Select(rowscoreyear);
                    int iIndex = selfyeardt.Rows.IndexOf(rows[0]) + 1;
                    name += "\"iIndex\":\"" + iIndex + "\",";
                    //月度
                    int monthscore = 0;
                    List<Dormitory> DormitoryListMonth = db.Dormitory.ToList();
                    foreach (Dormitory dormitory in DormitoryListMonth)
                    {
                        if (dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == dormitory.ID) != null)
                        {
                            var dormitoryuser = dormitoryuserlist.FirstOrDefault(x => x.DormitoryID == dormitory.ID).UserID;
                            if (dormitoryuser.Split(',').Any(x => x == uid))
                            {
                                List<SpotCheck> model = spotchecklist.Where(x => x.DormitoryId == dormitory.ID && x.CreateDate >= BeginMonth && x.CreateDate < EndMonth).ToList();
                                if (model.Count > 1)
                                {
                                    int score = 0;
                                    score = model.Sum(x => x.SpotScore);
                                    name += "\"MonthScore\":\"" + score + "\",";
                                    monthscore = score;
                                }
                                else if (model.Count == 1)
                                {
                                    name += "\"MonthScore\":\"" + model[0].SpotScore + "\",";
                                    monthscore = model[0].SpotScore;
                                }
                                else if (model.Count == 0)
                                {
                                    name += "\"MonthScore\":\"" + 0 + "\",";
                                }
                            }
                        }                       
                    }
                    //月度排名
                    foreach (Dormitory dormitory in DormitoryListMonth)
                    {
                        List<SpotCheck> model = spotchecklist.Where(x => x.DormitoryId == dormitory.ID && x.CreateDate >= BeginMonth && x.CreateDate < EndMonth).ToList();

                        if (model.Count > 1)
                        {
                            int score = 0;
                            score = model.Sum(x => x.SpotScore);
                            MonthRanking += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + score + "\"},";
                        }
                        else if (model.Count == 1)
                        {
                            MonthRanking += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + model[0].SpotScore + "\"},";
                        }
                        else if (model.Count == 0)
                        {
                            MonthRanking += "{\"name\":\"" + dormitory.DormiName + "\",\"score\":\"" + 0 + "\"},";
                        }
                    }
                    sbscoreMonth.Append("{\"result\":\"true\",\"data\":[");
                    sbscoreMonth.Append(MonthRanking.TrimEnd(','));
                    sbscoreMonth.Append("]}");
                    var dtMonth = JsonToDataTable(sbscoreMonth.ToString());
                    StringBuilder scoremonth = new StringBuilder("");
                    DataTable dtMonthNew = new DataTable();
                    if (dtMonth != null && dtMonth.Rows.Count > 0)
                    {
                        string textmonth = "";
                        foreach (DataRow row in dtMonth.Rows.Cast<DataRow>().OrderByDescending(r => int.Parse(r["score"].ToString())))
                        {
                            textmonth += "{\"name\":\"" + row["name"] + "\",\"score\":\"" + row["score"] + "\"},";
                        }
                        scoremonth.Append("{\"result\":\"true\",\"data\":[");
                        scoremonth.Append(textmonth.TrimEnd(','));
                        scoremonth.Append("]}");
                        dtMonthNew = JsonToDataTable(scoremonth.ToString());
                    }
                    var selfmonthdt = GetDistinctSelf(dtMonthNew, "score");
                    var rowscoremonth = string.Format("score='{0}'", monthscore);
                    DataRow[] rowsmonth = selfmonthdt.Select(rowscoremonth);
                    int iIndexmontht = selfmonthdt.Rows.IndexOf(rowsmonth[0]) + 1;
                    name += "\"iIndexmontht\":\"" + iIndexmontht + "\"";
                    name += "}";


                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
            }
            catch (Exception ex)
            {
                sb.Append("{\"result\":\"false\",\"data\":[");
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

        /// <summary>
        /// 过滤掉排名的重复行
        /// </summary>
        /// <param name="SourceDt"></param>
        /// <param name="filedName"></param>
        /// <returns></returns>
        public DataTable GetDistinctSelf(DataTable SourceDt, string filedName)
        {
            for (int i = SourceDt.Rows.Count - 2; i > 0; i--)
            {
                DataRow[] rows = SourceDt.Select(string.Format("{0}='{1}'", filedName, SourceDt.Rows[i][filedName]));
                if (rows.Length > 1)
                {
                    SourceDt.Rows.RemoveAt(i);
                }
            }
            return SourceDt;


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