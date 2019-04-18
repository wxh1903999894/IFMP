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
    public class AccountController : ApiController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">必填：UserName Password</param>
        /// <returns></returns>
        [HttpPost, Route("account/login")]
        public ApiResult Login([FromBody] SysUser user)
        {
            return new AccountBLL().Login(user);
        }
    }
}
