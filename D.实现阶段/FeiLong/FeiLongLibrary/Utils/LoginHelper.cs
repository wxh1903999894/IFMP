using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeiLongLibrary.Entities;
using FeiLongLibrary.DBContext;

namespace FeiLongLibrary.Utils
{/// <summary>
    /// 登录辅助类
    /// </summary>
    public class LoginHelper
    {

        /// <summary>
        /// 当前登录的用户，如果没有登录返回Null
        /// </summary>
        public static SysUser CurrentUser
        {
            get;
            set;            
        }
        
        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                if (CurrentUser != null)
                {
                    return true;
                }
                return false;
            }
        }

        public static string GenerateToken(int userID, DateTime expires, FLDbContext db)
        {
            string token = Guid.NewGuid().ToString();
            SysUser user = db.SysUser.FirstOrDefault(t => t.ID == userID);
            if (user != null)
            {
                user.Token = token;
                user.Expires = expires;
                db.SaveChanges();
            }
            return token;
        }
    }
}