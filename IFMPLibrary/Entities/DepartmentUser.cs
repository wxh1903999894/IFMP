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
    [Table("Tb_Department_User")]
    public class DepartmentUser
    {
        [Key]
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public int UserID { get; set; }

    }
}
