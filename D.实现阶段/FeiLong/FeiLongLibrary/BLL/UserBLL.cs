using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeiLongLibrary.DAO;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Utils;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Enums;
using System.Transactions;

namespace FeiLongLibrary.BLL
{
    public class UserBLL
    {
        public ApiResult Add(SysUser User)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    if (string.IsNullOrEmpty(User.UserName) || string.IsNullOrEmpty(User.Password))
                    {
                        message = "请输入用户名和密码";
                        goto Response;
                    }

                    if (User.RoleList.Count == 0)
                    {
                        message = "请选择正确的权限";
                        goto Response;
                    }

                    if (db.SysUser.FirstOrDefault(t => t.UserName == User.UserName) != null)
                    {
                        message = "用户名重复";
                        goto Response;
                    }
                    User.Password = new BaseUtils().BuildPW(User.UserName, User.Password);
                    User.CreateDate = DateTime.Now;
                    User.IsAccountDisabled = false;

                    db.SysUser.Add(User);
                    db.SaveChanges();

                    //添加Role
                    bool isrole = false;
                    foreach (int roleid in User.RoleList)
                    {
                        Role role = db.Role.FirstOrDefault(t => t.ID == roleid);
                        if (role != null)
                        {
                            isrole = true;
                            UserRole ur = new UserRole();
                            ur.RoleID = roleid;
                            ur.UserID = User.ID;
                            db.UserRole.Add(ur);
                        }
                    }

                    if (!isrole)
                    {
                        message = "请选择正确的权限";
                        goto Response;
                    }

                    db.SaveChanges();

                    //需要同步添加到企业微信
                    new SysLogDAO().AddLog(LogType.Success, message: "成功添加用户:" + User.RealName);
                    result = ApiResult.NewSuccessJson("成功添加用户:" + User.RealName);
                }

            }
            catch
            {
                result = ApiResult.NewErrorJson("请检查网络状态或联系系统管理员");
            }
        Response:
            if (!string.IsNullOrEmpty(message))
            {
                if (User.ID != 0)
                {
                    try
                    {
                        new FLDbContext().SysUser.Remove(new FLDbContext().SysUser.FirstOrDefault(t => t.ID == User.ID));
                    }
                    catch
                    {

                    }
                }
                result = ApiResult.NewErrorJson(message);
            }


            return result;
        }

        public ApiResult GetAll(int RoleID = 0, string RealName = "", int pageindex = 1, int pagesize = 10)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    var userlist = from User in db.SysUser
                                   join UserRole in db.UserRole on User.ID equals UserRole.UserID
                                   where (RoleID == 0 || UserRole.RoleID == RoleID)
                                   && User.IsAccountDisabled != true
                                   && User.RealName.Contains(RealName)
                                   select new
                                   {
                                       User
                                   };
                    int total = userlist.Select(t => t.User).Distinct().Count();

                    List<object> returnlist = new List<object>();
                    RoleDAO roledao = new RoleDAO();
                    foreach (var user in userlist.Select(t => t.User).Distinct().OrderByDescending(t => t.CreateDate).Skip((pageindex - 1) * pagesize).Take(pagesize))
                    {
                        returnlist.Add(new
                        {
                            user.ID,
                            user.RealName,
                            user.UserName,
                            user.TelNumber,
                            user.Gender,
                            RoleList = roledao.RoleList(user.ID)
                        });
                    }

                    result = ApiResult.NewSuccessJson(new
                    {
                        Total = total,
                        List = returnlist
                    });
                }
            }
            catch
            {
                result = ApiResult.NewErrorJson("请检查网络状态或联系系统管理员");
            }
            if (!string.IsNullOrEmpty(message))
            {
                //回滚
                result = ApiResult.NewErrorJson(message);
            }
            return result;
        }

    }
}
