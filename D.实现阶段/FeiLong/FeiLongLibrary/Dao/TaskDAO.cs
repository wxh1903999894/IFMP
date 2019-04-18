using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Utils;

namespace FeiLongLibrary.DAO
{
    public class TaskDAO
    {
        public string GetApplyTypeName(bool? IsAudit, ApplyTypeEnums ApplyType)
        {
            string result = "";
            if (IsAudit == true)
            {
                if (ApplyType == ApplyTypeEnums.正常)
                {
                    result = "已审核";
                }
                else
                {
                    result = "未审核";
                }
            }else
            {
                if (ApplyType == ApplyTypeEnums.正常)
                {
                    result = "已填写";
                }
                else
                {
                    result = "未填写";
                }
            }



            return result;
        }
    }
}
