using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Utils;

namespace FeiLongLibrary.DAO
{
    public class RoleDAO
    {
        FLDbContext db = new FLDbContext();
        public string RoleList(int UserID)
        {
            string result = "";
            List<UserRole> urlist = db.UserRole.Where(t => t.UserID == UserID).ToList();
            foreach (UserRole ur in urlist)
            {
                result = result + db.Role.FirstOrDefault(t => t.ID == ur.RoleID).Name + ",";
            }

            return result.TrimEnd(',');
        }
    }
}
