using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FeiLongLibrary.BLL;
using FeiLongLibrary.Utils;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;

namespace FeiLong.Controllers
{
    public class BaseController : ApiController
    {
        [HttpGet, Route("base/getAll")]
        public ApiResult GetEnumList(string Type)
        {
            return new BaseBLL().GetEnumList(Type);
        }

        [HttpGet, Route("base/getFullTableList")]
        public ApiResult GetFullTableList(ClassTypeEnums ClassType)
        {
            return new BaseBLL().GetFullTableList(ClassType);
        }
    }
}
