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
using System.Text;


namespace IFMP.integration
{
    public partial class BuckleInquiryList : PageBase
    {
        #region 参数集合 1 自己 2 全部
        public int Flag
        {
            get
            {
                return GetQueryString<int>("flag", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Flag == 1)
                {
                    this.txt_VUser.Text = UserRealName;
                    this.txt_VUser.Enabled = false;
                    this.lbl_Menuname.Text = "我的积分";
                }

                CommonFunction.BindEnum<AuditState>(this.ddl_AduitState, "-2");
                this.ddl_AduitState.Items.Remove(new ListItem("确认完成", "5"));

                this.ddl_SState.Items.Add(new ListItem("所有", "-1"));
                this.ddl_SState.Items.Add(new ListItem("未作废", "0"));
                this.ddl_SState.Items.Add(new ListItem("作废", "1"));


                GetCondition();
                DataBindList();
            }
        }
        #endregion

        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["VUser"] = CommonFunction.GetCommoneString(this.txt_VUser.Text.Trim());
            ViewState["EventName"] = CommonFunction.GetCommoneString(this.txt_EventName.Text.Trim());
            ViewState["AduitState"] = this.ddl_AduitState.SelectedValue;
            ViewState["SState"] = this.ddl_SState.SelectedValue;
            ViewState["begin"] = this.txt_Begin.Text == "" ? "1900-01-01" : this.txt_Begin.Text;
            ViewState["end"] = this.txt_End.Text == "" ? "9999-12-31" : this.txt_End.Text;
        }
        #endregion


        #region 数据绑定
        public void DataBindList()
        {
            //Pager.PageSize = 10;
            string username = CommonFunction.GetCommoneString(this.txt_VUser.Text.Trim());
            string eventname = CommonFunction.GetCommoneString(this.txt_EventName.Text.Trim());
            int auditstate = Convert.ToInt32(this.ddl_AduitState.SelectedValue);
            int isdel = Convert.ToInt32(this.ddl_SState.SelectedValue);
            //DateTime begin = Convert.ToDateTime(ViewState["begin"].ToString());
            //DateTime end = Convert.ToDateTime(ViewState["end"].ToString());
            DateTime begin = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"].ToString()));
            DateTime end = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"].ToString()), false);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                var list = from scoreuser in db.ScoreUser
                           //.Where(t => t.IsDel != true)
                           join score in db.Score.Where(t => t.IsDel != true) on scoreuser.ScoreID equals score.ID
                           join user in db.User.Where(t => t.IsDel != true) on scoreuser.UserID equals user.ID
                           join recorduser in db.User.Where(t => t.IsDel != true) on score.CreateUserID equals recorduser.ID
                           join scoreevent in db.ScoreEvent.Where(t => t.IsDel != true) on score.ScoreEventID equals scoreevent.ID
                           where ((scoreuser.UserID == UserID && Flag == 1) || Flag == 2)
                           && user.RealName.Contains(username)
                           && (score.AuditState == (AuditState)auditstate || auditstate == -2)
                           && score.CreateDate >= begin
                           && score.CreateDate <= end
                           && scoreevent.Name.Contains(eventname)
                           && (isdel == -1 || scoreuser.IsDel == (isdel == 0 ? false : true))
                           orderby scoreuser.IsDel, score.CreateDate descending
                           select new
                                  {
                                      scoreuser.ID,
                                      user.RealName,
                                      score.CreateDate,
                                      score.Title,
                                      EventName = scoreevent.Name,
                                      scoreuser.BScore,
                                      RecordUName = recorduser.RealName,
                                      score.AuditState,
                                      scoreuser.IsDel,
                                      score.Content
                                  };
                int total = list.Count();

                if (total > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                rp_List.DataSource = list.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList(); ;
                Pager.RecordCount = total;
                rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }

        }
        #endregion


        #region 查询
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion


        protected void lbtn_ZF_Click(object sender, EventArgs e)
        {
            try
            {
                string ids = this.hf_CheckIDS.Value.TrimEnd(',');
                if (string.IsNullOrEmpty(ids))
                {
                    ShowMessage("请至少选择一条记录");
                    return;
                }

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (string id in ids.Split(','))
                    {
                        int selid = Convert.ToInt32(id);
                        ScoreUser ScoreUser = db.ScoreUser.FirstOrDefault(t => t.ID == selid);
                        ScoreUser.IsDel = true;
                    }

                    db.SaveChanges();
                    new SysLogDAO().AddLog(LogType.操作日志_删除, "作废积分奖扣信息", UserID);
                    ShowMessage("作废奖扣记录成功");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message.ToString());
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message);
            }
            this.hf_CheckIDS.Value = "";
            DataBindList();
        }


        #region 分页
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 导出
        protected void btn_OutPut_Click(object sender, EventArgs e)
        {
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    string username = CommonFunction.GetCommoneString(this.txt_VUser.Text.Trim());
                    string eventname = CommonFunction.GetCommoneString(this.txt_EventName.Text.Trim());
                    int auditstate = Convert.ToInt32(this.ddl_AduitState.SelectedValue);
                    int isdel = Convert.ToInt32(this.ddl_SState.SelectedValue);
                    DateTime begin = Convert.ToDateTime(ViewState["begin"].ToString());
                    DateTime end = Convert.ToDateTime(ViewState["end"].ToString());

                    var list = from scoreuser in db.ScoreUser
                               //.Where(t => t.IsDel != true)
                               join score in db.Score on scoreuser.ScoreID equals score.ID
                               join user in db.User.Where(t => t.IsDel != true) on scoreuser.UserID equals user.ID
                               join recorduser in db.User.Where(t => t.IsDel != true) on score.CreateUserID equals recorduser.ID
                               join scoreevent in db.ScoreEvent.Where(t => t.IsDel != true) on score.ScoreEventID equals scoreevent.ID
                               where ((scoreuser.UserID == UserID && Flag == 1) || Flag == 2)
                               && user.RealName.Contains(username)
                               && (score.AuditState == (AuditState)auditstate || auditstate == -2)
                               && score.CreateDate > begin
                               && score.CreateDate < end
                               && scoreevent.Name.Contains(eventname)
                               && (isdel == -1 || scoreuser.IsDel == (isdel == 0 ? false : true))
                               && (score.IsDel != true)
                               orderby score.CreateDate descending
                               select new
                               {
                                   scoreuser.ID,
                                   user.RealName,
                                   score.CreateDate,
                                   score.Title,
                                   EventName = scoreevent.Name,
                                   scoreuser.BScore,
                                   RecordUName = recorduser.RealName,
                                   score.AuditState,
                                   scoreuser.IsDel,
                                   score.Content
                               };


                    if (list.Count() > 0)
                    {
                        StringBuilder str = new StringBuilder("");
                        str.Append("<table border=\"1\" cellpadding=\"0\" cellspaccing=\"0\"><tr><th>奖扣对象</th><th>奖扣日期</th><th>主题</th><th>B分</th><th>事件描述</th></tr>");
                        foreach (var data in list.ToList())
                        {
                            str.Append("<tr>");
                            str.AppendFormat("<td>{0}</td>", data.RealName);
                            str.AppendFormat("<td>{0}</td>", data.CreateDate.Value.ToString("yyyy-MM-dd"));
                            str.AppendFormat("<td>{0}</td>", data.Title);
                            str.AppendFormat("<td>{0}</td>", data.BScore.ToString());
                            str.AppendFormat("<td>{0}</td>", data.Content);
                            str.Append("</tr>");
                        }
                        str.Append("</table>");
                        new SysLogDAO().AddLog(LogType.操作日志_导出, "导出积分奖扣信息", UserID);
                        new BaseUtils().ExportExcel("积分奖扣信息", str.ToString());
                    }
                    else
                    {
                        ShowMessage("暂无数据导出");
                        return;
                    }



                }

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message.ToString());
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message);
            }
        }
        #endregion
    }
}