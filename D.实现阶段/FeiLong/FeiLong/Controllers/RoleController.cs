using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FeiLongLibrary.BLL;
using FeiLongLibrary.Utils;
using FeiLongLibrary.Entities;

namespace FeiLong.Controllers
{
    public class RoleController : ApiController
    {

        [HttpGet, Route("role/getAll")]
        public ApiResult GetAll(string Name = "")
        {
            return new RoleBLL().GetAll(Name);
        }
    }
}
