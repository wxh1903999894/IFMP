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
    public class BaseClassBLL
    {
        public ApiResult Add(BaseClass BaseClass)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    if (string.IsNullOrEmpty(BaseClass.Name))
                    {
                        message = "请输入基础班次名称";
                        goto Response;
                    }

                    if (db.BaseClass.FirstOrDefault(t => t.Name == BaseClass.Name && t.IsDel == true) != null)
                    {
                        message = "基础班次名称重复";
                        goto Response;
                    }

                    BaseClass.IsDel = false;
                    BaseClass.CreateDate = DateTime.Now;
                    db.BaseClass.Add(BaseClass);

                    //添加时要验证完整性？BaseDate
                    foreach (BaseClassUser bcu in BaseClass.BaseClassUserList)
                    {
                        bcu.BaseClassID = BaseClass.ID;
                    }

                    db.BaseClassUser.AddRange(BaseClass.BaseClassUserList);

                    new SysLogDAO().AddLog(LogType.Success, message: "成功添加基础班次:" + BaseClass.Name);
                    result = ApiResult.NewSuccessJson("成功添加基础班次:" + BaseClass.Name);
                }

            }
            catch
            {
                result = ApiResult.NewErrorJson("请检查网络状态或联系系统管理员");
            }
        Response:
            if (!string.IsNullOrEmpty(message))
            {
                //回滚
                if (BaseClass.ID != 0)
                {
                    try
                    {
                        new FLDbContext().BaseClass.Remove(new FLDbContext().BaseClass.FirstOrDefault(t => t.ID == BaseClass.ID));
                        new FLDbContext().BaseClassUser.RemoveRange(new FLDbContext().BaseClassUser.Where(t => t.BaseClassID == BaseClass.ID));
                    }
                    catch
                    {

                    }
                }
                result = ApiResult.NewErrorJson(message);
            }
            return result;
        }

        public ApiResult GetAll(string Name = "", ClassTypeEnums? ClassType = null, int pageindex = 1, int pagesize = 10)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;
            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    int total = db.BaseClass.Where(t => t.Name.Contains(Name) && (ClassType == null || t.ClassType == ClassType) && t.IsDel != true).Count();
                    List<BaseClass> BaseClassList = db.BaseClass.Where(t => t.Name.Contains(Name) && (ClassType == null || t.ClassType == ClassType) && t.IsDel != true).OrderByDescending(t => t.CreateDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    List<object> returnlist = new List<object>();
                    List<BaseClassUser> BaseClassUserList = db.BaseClassUser.ToList();
                    foreach (BaseClass bc in BaseClassList)
                    {
                        returnlist.Add(new
                        {
                            bc.ID,
                            bc.Name,
                            ClassType = Enum.GetName(typeof(ClassTypeEnums), bc.ClassType),
                            UserCount = BaseClassUserList.Count(t => t.BaseClassID == bc.ID)
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
                result = ApiResult.NewErrorJson(message);
            }
            return result;
        }

    }
}
