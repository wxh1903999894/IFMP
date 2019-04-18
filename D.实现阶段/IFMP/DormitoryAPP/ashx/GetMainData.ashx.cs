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
            }
        }

        #region 获取宿舍列表
        public void GetDormitory(HttpContext context)
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<Dormitory> DormitoryList = db.Dormitory.ToList();
                var name = "";
                foreach (Dormitory dormitory in DormitoryList)
                {
                    var index = dormitory.DormiUser;
                    if (index != "" && index != null)
                    {
                        name += "{\"value\":\"" + dormitory.ID + "\",\"text\":\"" + dormitory.DormiName + "\"},";
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

                var model = db.Dormitory.FirstOrDefault(x => x.ID == dormitoryid);
                var name = "";

                foreach (var dor in model.DormiUser.TrimEnd(',').Split(','))
                {
                    int id = int.Parse(dor);
                    var user = db.User.FirstOrDefault(x => x.ID == id);
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
        public void SpotProblemAdd(HttpContext context)
        {
            try
            {
                int uid = Convert.ToInt32(context.Request.Params["UserID"]);
                //int uid = 1;
                int SpotScore = Convert.ToInt32(context.Request.Params["SpotScore"]);
                int DormitorySel = Convert.ToInt32(context.Request.Params["DormitorySel"]);
                var ProDesc = context.Request.Params["ProDesc"].TrimEnd(',').Split(',');
                var DutyUser = context.Request.Params["DutyUser"].TrimEnd(',').Split(',');
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    var user = db.User.FirstOrDefault(x => x.ID == uid);
                    SpotCheck SpotCheck = new SpotCheck();
                    SpotCheck.SpotScore = SpotScore;
                    SpotCheck.DormitoryId = DormitorySel;
                    SpotCheck.CreateUser = user.RealName;
                    SpotCheck.CreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    db.SpotCheck.Add(SpotCheck);
                    db.SaveChanges();
                    for (int i = 0; i < ProDesc.Length; i++)
                    {
                        int dutyuderid = int.Parse(DutyUser[i]);
                        SpotProblem SpotProblem = new SpotProblem();
                        SpotProblem.SpotId = SpotCheck.SpotId;
                        SpotProblem.ProDesc = ProDesc[i];
                        SpotProblem.DutyUser = db.User.FirstOrDefault(x => x.ID == dutyuderid).RealName;
                        SpotProblem.CreateUser = db.User.FirstOrDefault(x => x.ID == uid).RealName;
                        SpotProblem.CreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        SpotProblem.IsreView = false;
                        db.SpotProblem.Add(SpotProblem);
                        db.SaveChanges();    
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
                        foreach (SpotCheck spotcheckmodel in model)
                        {
                            score += spotcheckmodel.SpotScore;
                        }
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
                        text += "{\"name\":\"" + row["name"] + "宿舍" + "\",\"score\":\"" + row["score"] + "\"},";
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
                    var SpotProblemList = db.SpotProblem.ToList();
                    if (flag == 2)
                    {
                        SpotProblemList = db.SpotProblem.Where(x => x.IsreView == false).ToList();
                    }
                    string name = "";
                    foreach (SpotProblem spotproblem in SpotProblemList)
                    {
                        var isreview = spotproblem.IsreView.ToString()=="False" ? "否" : "是";
                        var model = db.SpotCheck.FirstOrDefault(x => x.SpotId == spotproblem.SpotId);
                        var dormodel = db.Dormitory.FirstOrDefault(x => x.ID == model.DormitoryId);
                        if (dormodel != null)
                        {
                            name += "{\"ProDesc\":\"" + spotproblem.ProDesc + "\",\"DutyUser\":\"" + spotproblem.DutyUser + "\",\"CreateUser\":\"" + spotproblem.CreateUser + "\",\"CreateDate\":\"" + Convert.ToDateTime(spotproblem.CreateDate).ToShortDateString() + "\",\"SpotScore\":\"" + model.SpotScore + "\",\"DormiName\":\"" + dormodel.DormiName + "\",\"IsreView\":\"" + isreview + "\",\"SpId\":\"" + spotproblem.SpId + "\",\"ReviewMemo\":\"" + spotproblem.ReviewMemo + "\"},";
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
                    SpotCheck.CreateUser = user.RealName;
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
                                    foreach (SpotCheck spotcheckmodel in spotchecklist)
                                    {
                                        score += spotcheckmodel.SpotScore;
                                    }
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

        #region 获取复查详情页数据
        public void GetReviewDetail(HttpContext context)
        {
            int SpId = Convert.ToInt32(context.Request.Params["SpId"]);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                var name = "";
                var model = db.SpotProblem.FirstOrDefault(x => x.SpId == SpId);
                var spotcheck = db.SpotCheck.FirstOrDefault(x => x.SpotId == model.SpotId);
                var dormitory = db.Dormitory.FirstOrDefault(x => x.ID == spotcheck.DormitoryId);
                name += "{\"ProDesc\":\"" + model.ProDesc + "\",\"DutyUser\":\"" + model.DutyUser + "\",\"CreateUser\":\"" + model.CreateUser + "\",\"dormitory\":\"" + dormitory.DormiName + "\"},";
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
                //int uid = 1;
                var Review = context.Request.Params["Review"];
                int SpId = Convert.ToInt32(context.Request.Params["SpId"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    var model = db.SpotProblem.FirstOrDefault(x => x.SpId == SpId);
                    model.ReviewMemo = Review;
                    model.ReviewUser = db.User.FirstOrDefault(x => x.ID == uid).RealName;
                    model.ReviewDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    model.IsreView = true;
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