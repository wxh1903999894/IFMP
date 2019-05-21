using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFMPLibrary.Enums;

namespace IFMPLibrary.Entities
{
    //不排名人员可查看的部门
    [Table("Tb_NoScoreUserDepartment")]
    public class NoScoreUserDepartment
    {
        [Key]
        public int ID { get; set; }
        public int NoScoreUserID { get; set; }
        public int DepartmentID { get; set; }

    }
}
