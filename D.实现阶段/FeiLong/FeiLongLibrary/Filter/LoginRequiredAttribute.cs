using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using FeiLongLibrary.BLL;
using FeiLongLibrary.Enums;
using FeiLongLibrary.Utils;
using FeiLongLibrary.Entities;
using FeiLongLibrary.DBContext;
using FeiLongLibrary;

namespace WHIHMPLibrary.Filter
{
    /// <summary>
    ///internal 只能在内部使用
    /// </summary>
    public class LoginRequiredAttribute : System.Web.Http.AuthorizeAttribute
    {
        public string[] Roles { get; set; }
        //public string AuthName { get; set; }
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool hasAccess = false;
            if (actionContext.Request.Headers.Authorization != null)
            {
                string token = actionContext.Request.Headers.Authorization.Scheme;

                //用户验证逻辑
                string message = string.Empty;
                if (new AccountBLL().ValidateTicket(token, out message))
                {
                    hasAccess = false;
                    //判断权限
                    SysUser user = LoginHelper.CurrentUser;
                    if (user != null && user.IsAccountDisabled != true)
                    {
                        if (Roles.Length > 0)
                        {
                            using (FLDbContext db = new FLDbContext())
                            {
                                List<AuthorizationRole> arlist = db.AuthorizationRole.Where(t => db.Authorization.Where(m => Roles.Contains(m.Name)).Select(m => m.ID).Contains(t.AuthorizationID)).ToList();
                                List<UserRole> urlist = db.UserRole.Where(t => t.UserID == user.ID).ToList();
                                if (arlist.FirstOrDefault(t => t.UserID == user.ID || urlist.Select(m => m.RoleID).Contains(t.RoleID.Value)) != null)
                                {
                                    hasAccess = true;
                                }
                            }
                        }
                        else
                        {
                            hasAccess = true;
                        }
                    }

                    if (!hasAccess)
                    {
                        message = "没有操作权限";
                    }
                }
            }

            if (hasAccess == false)
            {
                var content = new ApiResult
                {
                    Status = ApiResultCodeType.Failure,
                    Data = "没有操作权限",
                };
                var response = actionContext.Response = actionContext.Response ?? new HttpResponseMessage();
                response.Content = new StringContent(System.Web.Helpers.Json.Encode(content), Encoding.UTF8, "application/json");
            }
        }

    }
}