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
namespace IFMP.dormitory
{
    public partial class DormitoryScoreDetail : PageBase
    {
        #region 参数集合
        public int SpID
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
                if (SpID != -1)
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
                SpotProblem spotproblem = db.SpotProblem.FirstOrDefault(x => x.SpId == SpID);
                if (spotproblem != null)
                {
                    SpotCheck spotcheck = db.SpotCheck.FirstOrDefault(x => x.SpotId == spotproblem.SpotId);
                    Dormitory dormitory = db.Dormitory.FirstOrDefault(x => x.ID == spotcheck.DormitoryId);
                    this.ltl_DorName.Text = dormitory.DormiName.ToString();
                    this.ltl_ProDesc.Text = spotproblem.ProDesc.ToString();
                    this.ltl_DutyUser.Text = spotproblem.DutyUser == null ? "" : db.User.FirstOrDefault(t => t.ID.ToString() == spotproblem.DutyUser).RealName;
                    this.ltl_SState.Text = (spotproblem.IsreView == true ? "是" : "否");
                    this.ltl_CreateUser.Text = spotproblem.CreateUser == null ? "" : db.User.FirstOrDefault(t => t.ID.ToString() == spotproblem.CreateUser).RealName;
                    this.ltl_CreateDate.Text = spotproblem.CreateDate == null ? "" : spotproblem.CreateDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_ReviewUser.Text = spotproblem.ReviewUser == null ? "" : db.User.FirstOrDefault(t => t.ID.ToString() == spotproblem.ReviewUser).RealName;
                    this.ltl_ReviewDate.Text = spotproblem.ReviewDate == null ? "" : spotproblem.ReviewDate.Value.ToString("yyyy-MM-dd");
                    this.ltl_ReviewMemo.Text = spotproblem.ReviewMemo == null ? "" : spotproblem.ReviewMemo.ToString();
                    if (string.IsNullOrEmpty(spotproblem.SImage))
                    {
                        this.img.ImageUrl = "";
                        this.img.Visible = false;
                    }
                    else
                    {
                        this.img.ImageUrl = spotproblem.SImage.Length < 8 ? spotproblem.SImage : (spotproblem.SImage.ToString().Substring(0, 8) == "Templete" ? "../../DormitoryAPP/" + spotproblem.SImage : spotproblem.SImage);
                    }
                }

            }
        }
        #endregion
    }
}