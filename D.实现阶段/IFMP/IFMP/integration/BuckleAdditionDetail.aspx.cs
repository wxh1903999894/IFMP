using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using GK.IFMP.Common;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.DAO;

namespace IFMP.integration
{
    public partial class BuckleAdditionDetail : PageBase
    {
        #region 参数集合
        public int ScoreUserID
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
                if (ScoreUserID != -1)
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
                //ScoreUser ScoreUser = db.ScoreUser.FirstOrDefault(t => t.ID == ScoreUserID && t.IsDel != true);
                ScoreUser ScoreUser = db.ScoreUser.FirstOrDefault(t => t.ID == ScoreUserID);
                if (ScoreUser != null)
                {
                    Score Score = db.Score.FirstOrDefault(t => t.ID == ScoreUser.ScoreID);
                    ScoreEvent ScoreEvent = db.ScoreEvent.FirstOrDefault(t => t.ID == Score.ScoreEventID);
                    this.ltl_AduitState.Text = Enum.GetName(typeof(AuditState), Score.AuditState);
                    this.ltl_BScore.Text = ScoreUser.BScore.ToString();
                    this.ltl_EventMark.Text = Score.Content;
                    this.ltl_EventName.Text = ScoreEvent.Name;
                    this.ltl_FirstAduitDate.Text = Score.FirstAuditDate == null ? "" : Score.FirstAuditDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_FirstAduitMark.Text = Score.FirstAuditMark;
                    this.ltl_FirstAduitUser.Text = Score.FirstAuditUserID == null ? "" : db.User.FirstOrDefault(t => t.ID == Score.FirstAuditUserID.Value).RealName;
                    this.ltl_LastAduitDate.Text = Score.LastAuditDate == null ? "" : Score.LastAuditDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_LastAduitMark.Text = Score.LastAuditMark;
                    this.ltl_LastAduitUser.Text = Score.LastAuditUserID == null ? "" : db.User.FirstOrDefault(t => t.ID == Score.LastAuditUserID.Value).RealName;
                    this.ltl_RewardUser.Text = db.User.FirstOrDefault(t => t.ID == ScoreUser.UserID).RealName;
                    this.ltl_STitle.Text = Score.Title;
                    this.ltl_SState.Text = (ScoreUser.IsDel == true ? "是" : "否");
                    this.ltl_VDate.Text = Score.CreateDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_VUser.Text = db.User.FirstOrDefault(t => t.ID == Score.CreateUserID).RealName;

                    if (string.IsNullOrEmpty(Score.Image))
                    {
                        this.img.ImageUrl = "";
                        this.img.Visible = false;
                    }
                    else
                    {
                        this.img.ImageUrl = Score.Image.Length < 8 ? Score.Image : (Score.Image.ToString().Substring(0, 8) == "Templete" ? "../../jfz/" + Score.Image : Score.Image);
                    }
                }
            }
        }
        #endregion
    }
}