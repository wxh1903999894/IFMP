using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using System.Web;
using IFMPLibrary.Utils;

namespace IFMPLibrary.DAO
{
    public class AccountDAO
    {
        public bool ValidatedAccountPage(string UserName, string EncryptPassword)
        {
            bool result = false;
            using (IFMPDBContext db = new IFMPDBContext())
            {
                if (db.User.FirstOrDefault(t => t.UserName == UserName && t.IsDel != true && t.Password == EncryptPassword) != null)
                    result = true;
            }
            return result;
        }

        public bool ValidatedAccount(string UserName, string Password)
        {
            bool result = false;
            Password = new BaseUtils().BuildPW(UserName, Password);
            using (IFMPDBContext db = new IFMPDBContext())
            {
                if (db.User.FirstOrDefault(t => t.UserName == UserName && t.IsDel != true && t.Password == Password) != null)
                    result = true;
            }
            return result;
        }
    }
}
