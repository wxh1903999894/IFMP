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
    public class RoleBLL
    {
        public ApiResult Add(Role Role)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    if (string.IsNullOrEmpty(Role.Name))
                    {
                        message = "请输入权限名称";
                        goto Response;
                    }

                    if (db.Role.FirstOrDefault(t => t.Name == Role.Name) != null)
                    {
                        message = "权限名称重复";
                        goto Response;
                    }

                    Role.IsDel = false;
                    Role.CreateDate = DateTime.Now;
                    db.Role.Add(Role);

                    new SysLogDAO().AddLog(LogType.Success, message: "成功添加权限:" + Role.Name);
                    result = ApiResult.NewSuccessJson("成功添加权限:" + Role.Name);
                }
            }
            catch
            {
                result = ApiResult.NewErrorJson("请检查网络状态或联系系统管理员");
            }
        Response:
            if (!string.IsNullOrEmpty(message))
            {
                result = ApiResult.NewErrorJson(message);
            }

            return result;
        }

        public ApiResult GetAll(string Name = "")
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    int total = db.Role.Where(t => t.Name.Contains(Name) && t.IsDel != true).Count();
                    List<Role> RoleList = db.Role.Where(t => t.Name.Contains(Name) && t.IsDel != true).ToList();
                    result = ApiResult.NewSuccessJson(new
                    {
                        Total = total,
                        List = RoleList
                    });
                }
            }
            catch
            {
                result = ApiResult.NewErrorJson("请检查网络状态或联系系统管理员");
            }
        
            if (!string.IsNullOrEmpty(message))
            {
                result = ApiResult.NewErrorJson(message);
            }


            return result;
        }
    }
}
