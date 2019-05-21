using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;

namespace IFMPLibrary.Utils
{
    public class LoginHelper
    {

        /// <summary>
        /// 当前登录的用户，如果没有登录返回Null
        /// </summary>
        public static User CurrentUser
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

        public static string GenerateToken(int userID, DateTime expires, IFMPDBContext db)
        {
            string token = Guid.NewGuid().ToString();
            User user = db.User.FirstOrDefault(t => t.ID == userID);
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
