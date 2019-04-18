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
    public class BaseClassController : ApiController
    {

        [HttpGet, Route("baseClass/getAll")]
        public ApiResult GetAll(string Name = "", ClassTypeEnums? ClassType = null, int pageindex = 1, int pagesize = 10)
        {
            return new BaseClassBLL().GetAll(Name, ClassType, pageindex, pagesize);
        }
    }
}
