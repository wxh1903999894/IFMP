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
    [Table("Tb_Dormitory_User")]
    public class DormitoryUser
    {
        [Key]
        public int ID { get; set; }
        public int DormitoryID { get; set; }
        public string UserID { get; set; }
    }
}
