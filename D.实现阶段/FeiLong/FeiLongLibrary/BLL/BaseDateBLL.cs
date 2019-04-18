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
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace FeiLongLibrary.BLL
{
    public class BaseDateBLL
    {
        public ApiResult Add(List<BaseDateFlow> BaseDateFlowList)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    //这里要改的，要对flowid和classtype均相同的进行替换
                    List<Flow> FlowList = db.Flow.ToList();

                    foreach (BaseDateFlow bdf in BaseDateFlowList)
                    {
                        if (FlowList.FirstOrDefault(t => t.ID == bdf.FlowID) == null)
                        {
                            message = "请选择正确的班次类型或表单类型";
                            goto Response;
                        }
                        bdf.Name = FlowList.FirstOrDefault(t => t.ID == bdf.FlowID).Name + "-" + Enum.GetName(typeof(ClassTypeEnums), bdf.ClassType);
                    }
                    //添加时要验证完整性？BaseClass
                    db.BaseDateFlow.AddRange(BaseDateFlowList);
                    db.SaveChanges();

                    new SysLogDAO().AddLog(LogType.Success, message: "成功添加基础班次时间");
                    result = ApiResult.NewSuccessJson("成功添加基础班次时间");
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

        public ApiResult GetAll(ClassTypeEnums? ClassType = 0, TableTypeEnums? TableType = 0, DateTime? BeginDate = null, DateTime? EndDate = null, int pageindex = 1, int pagesize = 10)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;
            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    BaseUtils bu = new BaseUtils();
                    BeginDate = bu.InitDate(BeginDate, true);
                    EndDate = bu.InitDate(EndDate, false);
                    int total = db.BaseDateFlow.Where(t => t.BeginDate > DbFunctions.CreateDateTime(t.BeginDate.Year, t.BeginDate.Month, t.BeginDate.Day, BeginDate.Value.Hour, BeginDate.Value.Minute, BeginDate.Value.Second)
                        && t.EndDate < DbFunctions.CreateDateTime(t.EndDate.Year, t.EndDate.Month, t.EndDate.Day, EndDate.Value.Hour, EndDate.Value.Minute, EndDate.Value.Second)
                        && (ClassType == 0 || t.ClassType == ClassType)
                        && (TableType == 0 || t.TableType == TableType)).Count();
                    List<BaseDateFlow> bdfList = db.BaseDateFlow.Where(t => t.BeginDate > DbFunctions.CreateDateTime(t.BeginDate.Year, t.BeginDate.Month, t.BeginDate.Day, BeginDate.Value.Hour, BeginDate.Value.Minute, BeginDate.Value.Second)
                        && t.EndDate < DbFunctions.CreateDateTime(t.EndDate.Year, t.EndDate.Month, t.EndDate.Day, EndDate.Value.Hour, EndDate.Value.Minute, EndDate.Value.Second)
                        && (ClassType == 0 || t.ClassType == ClassType)
                        && (TableType == 0 || t.TableType == TableType)).OrderBy(t => t.TableType).ThenBy(t => t.FlowID).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    List<object> returnlist = new List<object>();
                    foreach (BaseDateFlow bdf in bdfList)
                    {
                        returnlist.Add(new
                        {
                            bdf.ID,
                            bdf.Name,
                            TableType = Enum.GetName(typeof(TableTypeEnums), bdf.TableType),
                            ClassType = Enum.GetName(typeof(ClassTypeEnums), bdf.ClassType),
                            BeginDate = bdf.BeginDate.ToString("HH:mm"),
                            EndDate = bdf.EndDate.ToString("HH:mm"),
                            RemindDate = bdf.RemindDate.ToString("HH:mm"),
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
