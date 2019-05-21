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

namespace IFMP.integration
{
    public partial class ScoreMonthList : PageBase
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
                //获取该用户的组别
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    NoScoreUser NoScoreUser = db.NoScoreUser.FirstOrDefault(t => t.UserID == UserID);
                    if (NoScoreUser != null)
                    {
                        List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && db.NoScoreUserDepartment.Where(m => m.NoScoreUserID == NoScoreUser.ID).Select(m => m.DepartmentID).Contains(t.ID)).ToList();
                        this.ddl_Group.DataSource = DepartmentList;
                        this.ddl_Group.DataValueField = "ID";
                        this.ddl_Group.DataTextField = "Name";
                        this.ddl_Group.DataBind();
                        this.ddl_Group.Items.Add(new ListItem("--请选择--", "-1"));
                        this.ddl_Group.SelectedValue = "-1";
                    }
                    else
                    {
                        List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && db.DepartmentUser.Where(m => m.UserID == UserID).Select(m => m.DepartmentID).Contains(t.ID)).ToList();
                        this.ddl_Group.DataSource = DepartmentList;
                        this.ddl_Group.DataValueField = "ID";
                        this.ddl_Group.DataTextField = "Name";
                        this.ddl_Group.DataBind();
                        this.ddl_Group.Items.Add(new ListItem("--请选择--", "-1"));
                        this.ddl_Group.SelectedValue = "-1";
                    }

                    this.ltl_M1.Visible = this.ddl_Month.Visible = this.ddl_Year.Visible = false;

                }

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
            ViewState["group"] = this.ddl_Group.SelectedValue;
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
                int group = Convert.ToInt32(ViewState["group"]);

                List<User> UserList = db.User.Where(t => (group == -1 || db.DepartmentUser.Where(m => m.DepartmentID == group).Select(m => m.UserID).Contains(t.ID)) && t.IsDel != true && !db.NoScoreUser.Select(m => m.UserID).Contains(t.ID)).ToList();
                List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.IsDel != true).ToList();
                List<Score> ScoreList = db.Score.Where(t => t.IsDel != true && t.AuditState == AuditState.通过).ToList();

                DateTime begindate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["beign"]));
                DateTime enddate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"]), false);

                foreach (User User in UserList)
                {
                    User.Total = ScoreUserList.Where(t => t.UserID == User.ID && ScoreList.Where(m => m.IsDel != true && m.CreateDate.Value > begindate && m.CreateDate.Value < enddate).Select(m => m.ID).Contains(t.ScoreID)).Sum(t => t.BScore);
                }

                List<object> returnlist = new List<object>();
                UserList = UserList.OrderByDescending(t => t.Total).ThenBy(t => t.CreateDate).ToList();
                int k = 0;
                int lastcount = 0;
                int samecount = 0;
                foreach (User User in UserList)
                {
                    if (lastcount == User.Total)
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
                        User.RealName,
                        User.Total,
                        isbr = User.ID == UserID ? "1" : "0"
                    });
                    lastcount = User.Total;
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



        public string GetName(object xh, object name, object isbr)
        {
            if (Convert.ToInt32(xh) <= 3 || isbr.ToString() == "1")
            {
                return "<span style='color:red'>" + name.ToString() + "</span>";
            }
            else
            {
                return name.ToString();
            }
        }
    }
}