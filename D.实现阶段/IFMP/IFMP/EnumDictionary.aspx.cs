using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.Enums;

namespace IFMP
{
    public partial class EnumDictionary : PageBase
    {
        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        #endregion


        #region 下拉框绑定事件
        protected void ddl_EnumName_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<object> list = new List<object>();
            string name=this.ddl_EnumName.SelectedValue.ToString();
            Type enumSource = null;
            switch (name)
            {
                case "UserType":
                    enumSource =typeof(UserType);
                    break;
                case "ClassTypeEnums":
                    enumSource = typeof(ClassTypeEnums);
                    break;
                case "ApplyTypeEnums":
                    enumSource = typeof(ApplyTypeEnums);
                    break;
                case "RegexType":
                    enumSource = typeof(RegexType);
                    break;
                case "UserLeaveType":
                    enumSource = typeof(UserLeaveType);
                    break;
                case "LeaveType":
                    enumSource = typeof(LeaveType);
                    break;
            }

            if (enumSource != null)
            {
                foreach (int itemValue in Enum.GetValues(enumSource))
                {
                    list.Add(new
                    {
                        EnumContent = Enum.GetName(enumSource, itemValue)
                    });
                }
                if (list.Count > 0)
                {
                    this.rp_List.DataSource = list;
                    this.rp_List.DataBind();
                }
            }
        }
        #endregion
    }
}