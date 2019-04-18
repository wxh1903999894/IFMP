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

namespace FeiLongLibrary.BLL
{
    public class DictionaryBLL
    {
        public ApiResult Add(Dictionary Dictionary)
        {
            ApiResult result = new ApiResult();
            string message = string.Empty;
            try
            {
                using (FLDbContext db = new FLDbContext())
                {
                    if (string.IsNullOrEmpty(Dictionary.Name))
                    {
                        message = "请填写数据字典名称";
                        goto Response;
                    }

                    if (Dictionary.DataList == null || Dictionary.DataList.Count == 0)
                    {
                        message = "请添加数据字典内容";
                        goto Response;
                    }

                    Dictionary.CreateDate = DateTime.Now;
                    Dictionary.IsDel = false;
                    Dictionary.CreateUserID = LoginHelper.CurrentUser.ID;
                    db.Dictionary.Add(Dictionary);
                    db.SaveChanges();

                    new DictionaryDAO().AddDictionaryData(Dictionary);
                    
                    new SysLogDAO().AddLog(LogType.Success, message: "成功添加数据字典");
                    result = ApiResult.NewSuccessJson("成功添加数据字典");
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


    }
}
