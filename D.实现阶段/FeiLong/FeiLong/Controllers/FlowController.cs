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
    public class FlowController : ApiController
    {

        [HttpGet, Route("flow/getAll")]
        public ApiResult GetAll(TableTypeEnums? TableType = null)
        {
            return new FlowBLL().GetAll(TableType);
        }


        [HttpGet, Route("flow/getAllTableTypeFlow")]
        public ApiResult GetAllTableTypeFlow(ClassTypeEnums ClassType)
        {
            return new FlowBLL().GetAllTableTypeFlow(ClassType);
        }

    }
}
