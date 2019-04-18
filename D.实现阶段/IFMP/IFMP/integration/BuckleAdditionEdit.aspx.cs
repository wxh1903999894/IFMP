/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月18日 18时45分47秒
** 描    述:     积分奖扣添加修改页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.DAO;

namespace IFMP.integration
{
    public partial class BuckleAdditionEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();


        #region 参数集合
        public int SUserID
        {
            get
            {
                return GetQueryString<int>("id", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ddl_FirstAduitUser.Items.Add(new ListItem("--请选择--", "-2"));
                this.ddl_LastAduitUser.Items.Add(new ListItem("--请选择--", "-2"));
                this.ddl_First.Items.Add(new ListItem("--请选择--", "-2"));
                this.ddl_Second.Items.Add(new ListItem("--请选择--", "-2"));
                this.ddl_EventName.Items.Add(new ListItem("--请选择--", "-2"));
                List<User> UserList = db.User.Where(t => db.ScoreAuditUser.Where(m => m.ScoreAuditUserType == ScoreAuditUserType.初审人).Select(m => m.UserID).Contains(t.ID) && t.IsDel != true).ToList();
                if (UserList.Count > 0)
                {
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        this.ddl_FirstAduitUser.Items.Add(new ListItem(UserList[i].RealName.ToString(), UserList[i].ID.ToString()));
                    }
                }
                List<User> UserLast = db.User.Where(t => db.ScoreAuditUser.Where(m => m.ScoreAuditUserType == ScoreAuditUserType.终审人).Select(m => m.UserID).Contains(t.ID) && t.IsDel != true).ToList();
                if (UserLast.Count > 0)
                {
                    for (int i = 0; i < UserLast.Count; i++)
                    {
                        this.ddl_LastAduitUser.Items.Add(new ListItem(UserLast[i].RealName.ToString(), UserLast[i].ID.ToString()));
                    }
                }
                if (UserList != null && UserList.Count > 0)
                {

                    User model = db.User.FirstOrDefault(t => db.ScoreAuditUser.Where(m => m.ScoreAuditUserType == ScoreAuditUserType.初审人 && m.UserID == UserID).Select(m => m.UserID).Contains(t.ID) && t.IsDel != true);
                    if (model != null)
                    {
                        this.ddl_FirstAduitUser.SelectedValue = UserID.ToString();
                    }
                    else
                    {
                        List<Department> departList = db.Department.Where(t => db.DepartmentUser.Where(m => m.UserID == UserID).Select(m => m.DepartmentID).Contains(t.ParentID)).ToList();

                        if (departList.Count > 0)
                        { }
                        else
                        {
                            Department depart = db.Department.FirstOrDefault(t => db.DepartmentUser.Where(m => m.UserID == UserID).Select(m => m.DepartmentID).Contains(t.ID));
                            if (depart != null)
                            {
                                this.ddl_FirstAduitUser.SelectedValue = depart.MasterUserID.ToString();
                            }
                        }
                    }
                }
                CommonFunction.BindEnum<CommonEnum.IsorNot>(this.rbl_IsReward);
                this.rbl_IsReward.SelectedIndex = 1;

                List<ScoreEventType> eventTypeList = db.ScoreEventType.Where(t => t.ParentID == 0 && t.IsDel != true).ToList();
                if (eventTypeList.Count > 0)
                {
                    for (int i = 0; i < eventTypeList.Count; i++)
                    {
                        this.ddl_First.Items.Add(new ListItem(eventTypeList[i].Name.ToString(), eventTypeList[i].ID.ToString()));
                    }
                }
                int secpid = Convert.ToInt32(this.ddl_First.SelectedValue.ToString());
                List<ScoreEventType> secType = db.ScoreEventType.Where(t => t.ParentID == secpid && t.IsDel != true).ToList();
                if (secType.Count > 0)
                {
                    for (int i = 0; i < secType.Count; i++)
                    {
                        this.ddl_Second.Items.Add(new ListItem(secType[i].Name.ToString(), secType[i].ID.ToString()));
                    }
                }
                int eventpid = Convert.ToInt32(this.ddl_Second.SelectedValue.ToString());
                List<ScoreEvent> eventlist = db.ScoreEvent.Where(t => t.ScoreEventTypeID == eventpid && t.IsDel != true).ToList();
                if (eventlist.Count > 0)
                {
                    for (int i = 0; i < eventlist.Count; i++)
                    {
                        this.ddl_EventName.Items.Add(new ListItem(eventlist[i].Name.ToString(), eventlist[i].ID.ToString()));
                    }
                }
                if (SUserID != -1)
                {
                    BindInfo();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        public void BindInfo()
        {
            //var list = from scoreuser in db.ScoreUser.Where(t => t.IsDel != true)
            //           join score in db.Score.Where(t => t.IsDel != true) on scoreuser.ScoreID equals score.ID
            //           join scoreevent in db.ScoreEvent.Where(t => t.IsDel != true) on score.ScoreEventID equals scoreevent.ID
            //           join scoreeventtype in db.ScoreEventType.Where(t => t.IsDel != true) on scoreevent.ScoreEventTypeID equals scoreeventtype.ID
            //           where scoreuser.ID == SUserID
            //           select new
            //           {
            //               scoreuser.ID,
            //               scoreuser.UserID,
            //               URealName = db.User.FirstOrDefault(t => t.ID == scoreuser.UserID && t.IsDel != true).RealName,
            //               scoreuser.BScore,
            //               scoreuser.IsPrint,
            //               scoreuser.IsDel,
            //               RecordUserName = db.User.FirstOrDefault(t => t.ID == score.CreateUserID && t.IsDel != true).RealName,
            //               scoreevent.Name,
            //               scoreevent.ScoreEventTypeID,
            //               stid = scoreeventtype.ID,
            //           };

            //if (list != null && list.Count() > 0)
            //{
            //    //this.ddl_First.SelectedValue =list.ToList() .Rows[0]["SDID"].ToString();
            //    //DataTable dt2 = SysDataBLL.GetListByPID((int)CommonEnum.Deleted.未删除, -4, Convert.ToInt32(this.ddl_First.SelectedValue));
            //    //CommonFunction.DDlTypeBind(this.ddl_Second, dt2, "SDID", "DataName", "-2");
            //    //this.ddl_Second.SelectedValue = dt.Rows[0]["EType"].ToString();
            //    //DataTable dt3 = ScoreEventBLL.GetListByEtype((int)CommonEnum.Deleted.未删除, Convert.ToInt32(this.ddl_Second.SelectedValue));
            //    //CommonFunction.DDlTypeBind(this.ddl_EventName, dt3, "SEID", "EventName", "-2");

            //    //this.ddl_EventName.SelectedValue = dt.Rows[0]["EventName"].ToString();
            //    //this.txt_STitle.Text = dt.Rows[0]["STitle"].ToString();
            //    //this.txt_VDate.Text = Convert.ToDateTime(dt.Rows[0]["VDate"].ToString()).ToString("yyyy-MM-dd");
            //    //this.txt_EventMark.Text = dt.Rows[0]["EventMark"].ToString();
            //    //this.rbl_IsReward.SelectedValue = Convert.ToInt32(dt.Rows[0]["IsReward"].ToString()).ToString();
            //    //this.ddl_FirstAduitUser.SelectedValue = dt.Rows[0]["FirstAduitUser"].ToString();
            //    //this.ddl_LastAduitUser.SelectedValue = dt.Rows[0]["LastAduitUser"].ToString();
            //    //this.vuser.InnerText = dt.Rows[0]["RewardUserName"].ToString();
            //    //this.btn_plancom.Visible = false;
            //    //this.img.Visible = true;
            //    //if (dt.Rows[0]["SImage"].ToString() == "")
            //    //{
            //    //    this.img.ImageUrl = "";
            //    //    this.img.Visible = false;
            //    //}
            //    //else
            //    //{
            //    //    this.img.Visible = true;
            //    //    this.img.ImageUrl = dt.Rows[0]["SImage"].ToString().Substring(0, 8) == "Templete" ? "../../jfz/" + dt.Rows[0]["SImage"].ToString() : dt.Rows[0]["SImage"].ToString();
            //    //}
            //    //this.hf_SID.Value = dt.Rows[0]["SID"].ToString();
            //}
        }
        #endregion


        #region 提交
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.hf_UID.Value == "" && SUserID == -1)
                {
                    ShowMessage("请选择奖扣人员");
                    return;
                }
                int allscore = 0;
                string alluser = "";
                Score model = db.Score.FirstOrDefault(t => t.ID == SUserID && t.IsDel != true);
                if (model == null)
                {
                    model = new Score();
                    model.CreateUserID = UserID;
                    model.AuditState = AuditState.待初审;
                    model.Title = this.txt_STitle.Text.ToString().Trim();
                    model.ScoreEventID = Convert.ToInt32(this.ddl_EventName.SelectedValue.ToString());
                    model.Content = this.txt_EventMark.Text.ToString().Trim();
                    model.FirstAuditUserID = Convert.ToInt32(this.ddl_FirstAduitUser.SelectedValue.ToString());
                    model.LastAuditUserID = Convert.ToInt32(this.ddl_LastAduitUser.SelectedValue.ToString());
                    model.IsDel = false;
                    int iisreward = Convert.ToInt32(this.rbl_IsReward.SelectedValue.ToString());
                    model.IsReward = Convert.ToBoolean(iisreward);
                    model.CreateDate = this.txt_VDate.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(this.txt_VDate.Text.Trim());

                    #region 上传图片
                    if (this.fl_UpFile.HasFile)
                    {
                        int upsize = 4000000;
                        try
                        {
                            upsize = Convert.ToInt32(ConfigurationManager.AppSettings["upsize"].ToString());
                        }
                        catch (Exception) { }

                        string attaurl = "";
                        string pname = "/uploadfile/";
                        string path = System.Web.HttpContext.Current.Server.MapPath(pname);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string name = this.fl_UpFile.PostedFile.FileName.ToString();
                        string[] filename = name[name.Length - 1].ToString().Split('.');

                        string filepath = path + name;

                        if (this.fl_UpFile.PostedFile.ContentLength < upsize)
                        {
                            this.fl_UpFile.SaveAs(filepath);

                            int j = filepath.IndexOf("uploadfile");
                            attaurl = filepath.Substring(j - 1);
                        }
                        model.Image = attaurl;
                    }
                    else
                    {
                        model.Image = "";
                    }
                    #endregion
                    db.Score.Add(model);
                    db.SaveChanges();

                    //如果当前记录人是初审人则改变信息
                    List<ScoreAuditUser> list = db.ScoreAuditUser.Where(t => t.ScoreAuditUserType == ScoreAuditUserType.初审人 && t.UserID == model.FirstAuditUserID && model.CreateUserID == model.FirstAuditUserID).ToList();
                    if (list.Count > 0)
                    {
                        Score score = db.Score.FirstOrDefault(t => t.ID == model.ID && t.IsDel != true);
                        score.AuditState = AuditState.待终审;
                        score.FirstAuditDate = DateTime.Now;
                        db.SaveChanges();
                    }
                    /*****************************************/

                    #region 添加奖扣人员
                    string[] uids = this.hf_UID.Value.Split(',');
                    for (int i = 0; i < uids.Length; i++)
                    {
                        ScoreUser sumodel = new ScoreUser();
                        sumodel.UserID = Convert.ToInt32(uids[i].ToString());
                        sumodel.AScore = 0;
                        string[] scores = this.hf_Score.Value.ToString().Split(',');
                        for (int j = 0; j < scores.Length; j++)
                        {
                            if (j == i)
                            {
                                sumodel.BScore = Convert.ToInt32(scores[j].ToString());
                                if (sumodel.BScore < 0)
                                {
                                    model.IsReward = false;
                                }
                                break;
                            }
                        }
                        allscore += sumodel.BScore;
                        alluser += db.User.FirstOrDefault(t => t.ID == sumodel.UserID).RealName + ",";

                        sumodel.ScoreID = model.ID;
                        sumodel.IsPrint = false;
                        sumodel.IsDel = false;
                        db.ScoreUser.Add(sumodel);
                        db.SaveChanges();
                    }
                    #endregion
                }
                else
                {//没有修改
                    //model.Title = this.txt_STitle.ToString().Trim();
                    //model.Content = this.txt_EventMark.ToString().Trim();
                    //model.ScoreEventID = Convert.ToInt32(this.ddl_EventName.SelectedValue.ToString());
                    //model.FirstAuditUserID = Convert.ToInt32(this.ddl_FirstAduitUser.SelectedValue.ToString());
                    //model.LastAuditUserID = Convert.ToInt32(this.ddl_LastAduitUser.SelectedValue.ToString());
                    //model.IsReward = Convert.ToBoolean(this.rbl_IsReward.SelectedValue);
                    //model.CreateDate = Convert.ToDateTime(this.txt_VDate.Text);
                    //db.SaveChanges();
                }

                #region 添加通知消息
                User createuser = db.User.FirstOrDefault(t => t.ID == model.CreateUserID);
                User firstuser = db.User.FirstOrDefault(t => t.ID == model.FirstAuditUserID);
                User lastuser = db.User.FirstOrDefault(t => t.ID == model.LastAuditUserID);
                Notice nmodel = new Notice();
                nmodel.SendDate = DateTime.Now;
                nmodel.SendUserID = 1;
                nmodel.NoticeType = NoticeType.积分制消息;
                nmodel.SourceID = model.ID;
                nmodel.IsSend = false;
                nmodel.URL = "";
                Score scoreaudit = db.Score.FirstOrDefault(t => t.ID == model.ID && t.AuditState == AuditState.待初审);
                if (scoreaudit != null)
                {
                    nmodel.Contenet = "当前有一条奖扣记录需要审核|主题：" + this.txt_STitle.Text + "(" + allscore.ToString() + "分)|记录人：" + createuser.RealName + "|参与人：" + alluser + "|初审人：" + firstuser.RealName + "|终审人：" + lastuser.RealName + "|状态：待初审";
                    nmodel.ReciveUserID = model.FirstAuditUserID.Value;
                    db.Notice.Add(nmodel);
                    db.SaveChanges();

                    string[] uids = this.hf_UID.Value.Split(',');
                    for (int i = 0; i < uids.Length; i++)
                    {
                        nmodel.Contenet = "当前有一条奖扣记录需要审核|主题：" + this.txt_STitle.Text + "(" + allscore.ToString() + "分)|记录人：" + createuser.RealName + "|参与人：" + alluser + "|初审人：" + firstuser.RealName + "|终审人：" + lastuser.RealName + "|状态：待初审";
                        nmodel.ReciveUserID = Convert.ToInt32(uids[i].ToString());
                        db.Notice.Add(nmodel);
                        db.SaveChanges();
                    }
                }
                else
                {
                    nmodel.Contenet = "当前有一条奖扣记录需要审核|主题：" + this.txt_STitle.Text + "(" + allscore.ToString() + "分)|记录人：" + createuser.RealName + "|参与人：" + alluser + "|初审人：" + firstuser.RealName + "|终审人：" + lastuser.RealName + "|状态：待终审";
                    nmodel.ReciveUserID = model.LastAuditUserID.Value;
                    db.Notice.Add(nmodel);
                    db.SaveChanges();

                    string[] uids = this.hf_UID.Value.Split(',');
                    for (int i = 0; i < uids.Length; i++)
                    {
                        nmodel.Contenet = "当前有一条奖扣记录需要审核|主题：" + this.txt_STitle.Text + "(" + allscore.ToString() + "分)|记录人：" + createuser.RealName + "|参与人：" + alluser + "|初审人：" + firstuser.RealName + "|终审人：" + lastuser.RealName + "|状态：待终审";
                        nmodel.ReciveUserID = Convert.ToInt32(uids[i].ToString());
                        db.Notice.Add(nmodel);
                        db.SaveChanges();
                    }
                }
                #endregion

                ShowMessage();
                LogType log = (SUserID == 0 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                new SysLogDAO().AddLog(log, (SUserID == 0 ? "增加" : "修改") + "主题为" + this.txt_STitle.Text + "的积分奖扣信息");
            }
            catch (Exception ex)
            {
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                ShowMessage(ex.Message);
            }
        }
        #endregion


        #region 获取事件
        /// <summary>
        /// 一级类型绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_First_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddl_Second.Items.Clear();
            this.ddl_EventName.Items.Clear();
            this.ddl_Second.Items.Add(new ListItem("--请选择--", "-2"));
            this.ddl_EventName.Items.Add(new ListItem("--请选择--", "-2"));
            int secpid = Convert.ToInt32(this.ddl_First.SelectedValue.ToString());
            List<ScoreEventType> secType = db.ScoreEventType.Where(t => t.ParentID == secpid && t.IsDel != true).ToList();
            if (secType.Count > 0)
            {
                for (int i = 0; i < secType.Count; i++)
                {
                    this.ddl_Second.Items.Add(new ListItem(secType[i].Name.ToString(), secType[i].ID.ToString()));
                }
            }
            this.ddl_Second.SelectedValue = "-2";
        }

        /// <summary>
        /// 二级类型绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Second_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddl_EventName.Items.Clear();
            this.ddl_EventName.Items.Add(new ListItem("--请选择--", "-2"));
            int eventpid = Convert.ToInt32(this.ddl_Second.SelectedValue.ToString());
            List<ScoreEvent> eventlist = db.ScoreEvent.Where(t => t.ScoreEventTypeID == eventpid && t.IsDel != true).ToList();
            if (eventlist.Count > 0)
            {
                for (int i = 0; i < eventlist.Count; i++)
                {
                    this.ddl_EventName.Items.Add(new ListItem(eventlist[i].Name.ToString(), eventlist[i].ID.ToString()));
                }
            }
            this.ddl_EventName.SelectedValue = "-2";
        }

        /// <summary>
        /// 事件绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_EventName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(this.ddl_EventName.SelectedValue.ToString());
            ScoreEvent scoremodel = db.ScoreEvent.FirstOrDefault(t => t.ID == id);
            if (scoremodel != null)
            {
                this.ddl_LastAduitUser.SelectedValue = scoremodel.LastAuditUserID.ToString(); ;
            }
        }
        #endregion
    }
}