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
    public class BaseDateController : ApiController
    {

        [HttpPost, Route("baseDate/add")]
        public ApiResult Add([FromBody] BaseDatePost BaseDatePost)
        {
            return new BaseDateBLL().Add(BaseDatePost.BaseDateFlowList);
        }

        [HttpGet, Route("baseDate/getAll")]
        public ApiResult GetAll(ClassTypeEnums? ClassType = null, TableTypeEnums? TableType = null, DateTime? BeginDate = null, DateTime? EndDate = null, int pageindex = 1, int pagesize = 10)
        {
            return new BaseDateBLL().GetAll(ClassType, TableType, BeginDate, EndDate, pageindex, pagesize);
        }        
    }

    public class BaseDatePost
    {
        public List<BaseDateFlow> BaseDateFlowList { get; set; }
    }
}
