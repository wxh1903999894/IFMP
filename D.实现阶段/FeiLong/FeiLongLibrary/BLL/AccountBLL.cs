using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Utils;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;
using FeiLongLibrary.DAO;


namespace FeiLongLibrary.BLL
{
    public class AccountBLL
    {


        public ApiResult Login(SysUser user)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;
            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
                    {
                        string md5pw = new BaseUtils().BuildPW(user.UserName, user.Password);
                        SysUser loginuser = db.SysUser.FirstOrDefault(t => t.UserName == user.UserName && t.Password == md5pw);
                        if (loginuser != null)
                        {
                            if (loginuser.IsAccountDisabled == true)
                            {
                                message = "用户被限制登录，请联系管理员";
                            }
                            else
                            {
                                DateTime now = DateTime.Now;
                                DateTime? expires = null;

                                //重新生成token
                                expires = DateTime.Now.AddMinutes(60 * 24 * 7); //一周后过期
                                //expires = DateTime.Now.AddMinutes(1); //测试
                                string token = LoginHelper.GenerateToken(loginuser.ID, expires.Value, db);
                                LoginHelper.CurrentUser = loginuser;

                                loginuser.LastLogindate = DateTime.Now;
                                db.SaveChanges();

                                //new WXDAO().SendMessage("登陆成功");

                                new SysLogDAO().AddLog(LogType.Success, message: "登录");
                                result.Data = new
                                {
                                    RealName = loginuser.RealName,
                                    Token = token,
                                    Expires = expires,
                                    HeaderUrl = loginuser.HeaderUrl,//需要默认的头像
                                };
                                result.Status = ApiResultCodeType.Success;
                            }
                        }
                        else
                        {
                            message = "登录失败，请检查用户名和密码";
                        }

                    }
                    else
                    {
                        message = "请输入用户名和密码";
                    }
                }

            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }

            if (!string.IsNullOrEmpty(message))
            {

                result = ApiResult.NewErrorJson(message);

            }

            return result;
        }

        public ApiResult GetValidateTicket(string token, string realname)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;
            try
            {
                if (ValidateTicket(token, out message))
                {
                    if (LoginHelper.CurrentUser.RealName == realname)
                    {
                        result = ApiResult.NewSuccessJson(LoginHelper.CurrentUser.ID);
                    }
                    else
                    {
                        message = "用户在其他地方登录，请重新登录";
                    }

                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }

            if (!string.IsNullOrEmpty(message))
            {

                result = ApiResult.NewErrorJson(message);

            }

            return result;
        }



        public bool ValidateTicket(string encryptTicket, out string message)
        {
            message = string.Empty;
            bool IsValid = false;
            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    SysUser user = db.SysUser.FirstOrDefault(t => t.Token == encryptTicket);

                    if (user == null)
                    {
                        message = "身份未验证";
                    }
                    else if (user.Expires == null || (user.Expires < DateTime.Now))
                    {
                        message = "token过期超时,请重新登录.";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
                        {
                            IsValid = true;
                            LoginHelper.CurrentUser = user;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return IsValid;
        }



    }
}