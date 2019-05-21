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
    public class DepartmentDAO
    {

        public string GetUserAdminDepartment(int UserID)
        {
            string result = "";
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true
                    && t.IsAdmin
                    && db.DepartmentUser.Where(m => m.UserID == UserID).Select(m => m.DepartmentID).Contains(t.ID)).ToList();


                foreach (Department Department in DepartmentList)
                {
                    result = result + Department.Name + ",";
                }

            }
            return result.TrimEnd(',');
        }
    }
}
