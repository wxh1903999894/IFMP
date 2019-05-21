using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text;

namespace IFMP.dormitory
{
    public partial class DormitoryScoreMonthList : PageBase
    {
        public static string title = "年度";
        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ltl_M1.Visible = this.ddl_Month.Visible = this.ddl_Year.Visible = false;
                GetYear();
                GetMonth();
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取年
        public void GetYear()
        {
            int year = DateTime.Now.Year;
            for (int i = 0; i < 3; i++)
            {
                this.ddl_Year.Items.Add(new ListItem((year + i - 1).ToString(), (year + i - 1).ToString()));
            }
            this.ddl_Year.SelectedValue = year.ToString();
        }
        #endregion


        #region 获取月
        public void GetMonth()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i <= 9)
                {
                    this.ddl_Month.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    this.ddl_Month.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            this.ddl_Month.SelectedValue = DateTime.Now.Month <= 9 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
        }
        #endregion



        #region 获取查询条件
        public void GetCondition()
        {
            if (title == "月度")
            {
                ViewState["beign"] = this.ddl_Year.SelectedValue.ToString() + "-" + this.ddl_Month.SelectedValue.ToString() + "-01";
                ViewState["end"] = Convert.ToDateTime(ViewState["beign"]).AddMonths(1).AddDays(-1);
            }
            else
            {
                ViewState["beign"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
                ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
            }
        }
        #endregion

         #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void DataBindList()
        {
            //分情况计算

            using (IFMPDBContext db = new IFMPDBContext())
            {
                StringBuilder sb = new StringBuilder("");
                List<object> returnlist = new List<object>();
                DateTime begindate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["beign"]));
                DateTime enddate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"]), false);
                List<Dormitory> DormitoryList = db.Dormitory.ToList();
                string name = "";
                foreach (Dormitory dormitory in DormitoryList)
                {
                    List<SpotCheck> model = db.SpotCheck.Where(x => x.DormitoryId == dormitory.ID && x.CreateDate >= begindate && x.CreateDate < enddate).ToList();
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
                    int k = 0;
                    int lastcount = 0;
                    int samecount = 0;
                    foreach (DataRow row in dt.Rows.Cast<DataRow>().OrderByDescending(r => int.Parse(r["score"].ToString())))
                    {
                        var score = row["score"].ToString();
                        if (lastcount == int.Parse(score))
                        {
                            samecount++;
                        }
                        else
                        {
                            k = k + samecount + 1;
                            samecount = 0;
                        }
                        returnlist.Add(new
                        {
                            Number = k,
                            RealName = row["name"],
                            Total = row["score"]
                        });
                        lastcount = int.Parse(score);
                    }
                }
                if (returnlist.Count > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                this.rp_List.DataSource = returnlist;
                rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
                LinkButton lbtn = lbtn_Saturday;
                if (title == "月度")
                {
                    lbtn = lbtn_Staff;
                }

                this.lbtn_Staff.BackColor = lbtn_Saturday.BackColor = System.Drawing.Color.FromName("#67b7ef");
                this.lbtn_Staff.ForeColor = lbtn_Saturday.ForeColor = System.Drawing.Color.FromName("#e6e8c9");
                lbtn.BackColor = System.Drawing.Color.FromName("#4F7ECF");
                lbtn.ForeColor = System.Drawing.Color.FromName("#f0f5ff");
            }
        }
         #endregion

        #region 查询
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 分类
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtn_Monday_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            string op = lbtn.CommandName.ToString();
            if (op == "nd")
            {
                title = "年度";
                lbtn = lbtn_Saturday;
                this.ltl_M2.Visible = this.txt_Begin.Visible = this.txt_End.Visible = this.ltl_zhi.Visible = true;
                this.ltl_M1.Visible = this.ddl_Month.Visible = this.ddl_Year.Visible = false;
            }
            else if (op == "yd")
            {
                title = "月度";
                lbtn = lbtn_Staff;
                this.ltl_M2.Visible = this.txt_Begin.Visible = this.txt_End.Visible = this.ltl_zhi.Visible = false;
                this.ltl_M1.Visible = this.ddl_Month.Visible = this.ddl_Year.Visible = true;
            }

            GetCondition();
            DataBindList();
            this.lbtn_Staff.BackColor = lbtn_Saturday.BackColor = System.Drawing.Color.FromName("#67b7ef");
            this.lbtn_Staff.ForeColor = lbtn_Saturday.ForeColor = System.Drawing.Color.FromName("#e6e8c9");
            lbtn.BackColor = System.Drawing.Color.FromName("#4F7ECF");
            lbtn.ForeColor = System.Drawing.Color.FromName("#f0f5ff");
        }
        #endregion



        public string GetName(object xh, object name)
        {
            if (Convert.ToInt32(xh) <= 3)
            {
                return "<span style='color:red'>" + name.ToString() + "</span>";
            }
            else
            {
                return name.ToString();
            }
        }

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
    }
}