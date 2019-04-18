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

namespace IFMP
{
    public partial class Top : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.hf_ID.Value = UserID.ToString();
                //User user = db.User.FirstOrDefault(t => t.IsDel != true && t.ID == UserID);
                //if (user != null)
                //{
                //    this.hf_RealName.Value = user.RealName;
                //}
            }
        }
    }
}