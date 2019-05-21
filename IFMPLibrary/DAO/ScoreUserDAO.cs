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
    public class ScoreUserDAO
    {
        public string GetScoreUser(int ScoreID)
        {
            string UserNameList = "";
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<User> UserList = db.User.Where(t => t.IsDel != true && db.ScoreUser.Where(m => m.IsDel != true && m.ScoreID == ScoreID).Select(m => m.UserID).Contains(t.ID)).ToList();
                
                foreach (User User in UserList)
                {
                    UserNameList += User.RealName;
                }
            }
            return UserNameList;
        }
    }
}
