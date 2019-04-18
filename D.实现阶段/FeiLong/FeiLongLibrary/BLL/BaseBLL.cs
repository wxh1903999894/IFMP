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
    public class BaseBLL
    {
        public ApiResult GetEnumList(string Type)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                List<object> returnlist = null;
                switch (Type)
                {
                    case "ClassType":
                        returnlist = new BaseUtils().GetEnumList(typeof(ClassTypeEnums));
                        break;
                    case "TableType":
                        returnlist = new BaseUtils().GetEnumList(typeof(TableTypeEnums));
                        break;
                    default:
                        message = "基础信息参数不合法";
                        goto Response;
                }
                result = ApiResult.NewSuccessJson(returnlist);
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


        public ApiResult GetFullTableList(ClassTypeEnums ClassType)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;

            try
            {
                List<object> returnlist = new List<object>();

                using (FLDbContext db = new FLDbContext())
                {
                    List<Flow> FlowList = db.Flow.ToList();
                    List<BaseDateFlow> BaseDateFlowList = db.BaseDateFlow.Where(t => t.ClassType == ClassType).OrderBy(t => t.FlowID).ToList();
                    List<FullFlowList> FullFlowList = new List<FullFlowList>();
                    foreach (BaseDateFlow BaseDateFlow in BaseDateFlowList)
                    {
                        var userlist = from user in db.SysUser
                                       join userrole in db.UserRole on user.ID equals userrole.UserID
                                       join baseflowrole in db.BaseFlowRole on userrole.RoleID equals baseflowrole.RoleID
                                       where user.IsAccountDisabled != true && baseflowrole.FlowID == BaseDateFlow.FlowID
                                       select new
                                       {
                                           user.RealName,
                                           UserID = user.ID,
                                           //FlowName = flow.Name
                                       };

                        FullFlowList.Add(new FullFlowList
                        {
                            FlowID = BaseDateFlow.FlowID,
                            FlowName = FlowList.FirstOrDefault(t => t.ID == BaseDateFlow.FlowID).Name,
                            TableType = BaseDateFlow.TableType,
                            UserList = userlist.ToList(),
                            BeginDate = BaseDateFlow.BeginDate.ToString("HH:mm"),
                            EndDate = BaseDateFlow.EndDate.ToString("HH:mm"),
                            RemindDate = BaseDateFlow.RemindDate.ToString("HH:mm"),
                        });
                    }

                    FullFlowList = FullFlowList.Where(t => t.TableType == TableTypeEnums.切削液浓度点检表).ToList();
                    foreach (TableTypeEnums TableType in Enum.GetValues(typeof(TableTypeEnums)))
                    {
                        returnlist.Add(new
                        {
                            Name = Enum.GetName(typeof(TableTypeEnums), TableType),
                            ID = TableType,
                            FullFlowList = FullFlowList.Where(t => t.TableType == TableType).ToList()
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






        private class FullFlowList
        {
            public int FlowID { get; set; }
            public string FlowName { get; set; }
            public TableTypeEnums TableType { get; set; }
            public object UserList { get; set; }

            public string BeginDate { get; set; }
            public string EndDate { get; set; }
            public string RemindDate { get; set; }
        }

    }
}
