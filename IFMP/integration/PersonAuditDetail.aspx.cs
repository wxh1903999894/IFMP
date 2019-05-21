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
    public partial class PersonAuditDetail : PageBase
    {
        #region 参数集合
        public int ScoreID
        {
            get
            {
                return GetQueryString<int>("id", 0);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ScoreID != 0)
                {
                    BindInfo();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        public void BindInfo()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                Score Score = db.Score.FirstOrDefault(t => t.ID == ScoreID && t.IsDel != true);
                if (Score != null)
                {
                    ScoreEvent ScoreEvent = db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID);
                    this.ltl_AduitState.Text = Enum.GetName(typeof(AuditState), Score.AuditState);
                    this.ltl_EventMark.Text = Score.Content;
                    this.ltl_EventName.Text = ScoreEvent.Name;
                    this.ltl_FirstAduitDate.Text = Score.FirstAuditDate == null ? "" : Score.FirstAuditDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_FirstAduitMark.Text = Score.FirstAuditMark;
                    this.ltl_FirstAduitUser.Text = Score.FirstAuditUserID == null ? "" : db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID).RealName;
                    this.ltl_LastAduitDate.Text = Score.LastAuditDate == null ? "" : Score.LastAuditDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_LastAduitMark.Text = Score.LastAuditMark;
                    this.ltl_LastAduitUser.Text = Score.LastAuditUserID == null ? "" : db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID).RealName;
                    this.ltl_STitle.Text = Score.Title;
                    this.ltl_VDate.Text = Score.CreateDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_VUser.Text = db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName;
                    //考虑到手机端上传图片的相对路径
                    //this.img.ImageUrl = Score.Image.Length < 8 ? Score.Image : (Score.Image.ToString().Substring(0, 8) == "Templete" ? "../../jfz/" + Score.Image : Score.Image);
                    if (string.IsNullOrEmpty(Score.Image))
                    {
                        this.img.ImageUrl = "";
                        this.img.Visible = false;
                    }
                    else
                    {
                        this.img.ImageUrl = Score.Image.Length < 8 ? Score.Image : (Score.Image.ToString().Substring(0, 8) == "Templete" ? "../../jfz/" + Score.Image : Score.Image);
                    }

                    //List<ScoreUser> ScoreUserList = db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID).ToList();
                    var list = from scoreuser in db.ScoreUser.Where(t => t.IsDel != true && t.ScoreID == Score.ID)
                               join user in db.User.Where(t => t.IsDel != true) on scoreuser.UserID equals user.ID
                               orderby user.ID
                               select new
                                      {
                                          scoreuser.BScore,
                                          user.RealName
                                      };


                    if (list.Count() > 0)
                    {
                        this.tr_null.Visible = false;
                        this.rp_List.DataSource = list.ToList();
                        this.rp_List.DataBind();
                    }
                    else
                    {
                        this.tr.Visible = true;
                    }
                }
            }
        }
        #endregion
    }
}