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
    [Table("Tb_Leave")]
    public class Leave
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public double Day { get; set; }
        public string Content { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveState LeaveState { get; set; }
        public LeaveType LeaveType { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
