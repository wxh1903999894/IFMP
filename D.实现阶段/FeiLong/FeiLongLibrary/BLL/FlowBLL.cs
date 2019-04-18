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
    public class FlowBLL
    {
        public ApiResult GetAll(TableTypeEnums? TableType)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                if (TableType != 0)
                {
                    using (FLDbContext db = new FLDbContext())
                    {
                        List<Flow> FlowList = db.Flow.Where(t => t.TableType == TableType).OrderBy(t => t.ID).ToList();
                        result = ApiResult.NewSuccessJson(FlowList);
                    }
                }
                else
                {
                    message = "请选择正确的表单类型";
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

        public ApiResult GetAllTableTypeFlow(ClassTypeEnums? ClassType)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                List<object> returnlist = new List<object>();
                using (FLDbContext db = new FLDbContext())
                {
                    List<Flow> FlowList = db.Flow.ToList();
                    foreach (int item in Enum.GetValues(typeof(TableTypeEnums)))
                    {
                        returnlist.Add(new
                        {
                            ID = item,
                            Name = Enum.GetName(typeof(TableTypeEnums), item),
                            FlowList = FlowList.Where(t => t.TableType == (TableTypeEnums)item).ToList()
                        });
                    }
                }
                result = ApiResult.NewSuccessJson(returnlist);
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
