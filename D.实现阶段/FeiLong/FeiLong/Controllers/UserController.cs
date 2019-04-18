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
    public class UserController : ApiController
    {

        [HttpPost, Route("user/add")]
        public ApiResult Add([FromBody] SysUser User)
        {
            return new UserBLL().Add(User);
        }

        [HttpGet, Route("user/getAll")]
        public ApiResult GetAll(int RoleID = 0, string RealName = "", int pageindex = 1, int pagesize = 10)
        {
            return new UserBLL().GetAll(RoleID, RealName, pageindex, pagesize);
        }
    }
}
