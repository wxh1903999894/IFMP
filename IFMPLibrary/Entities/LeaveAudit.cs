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
    [Table("Tb_LeaveAudit")]
    public class LeaveAudit
    {
        [Key]
        public int ID { get; set; }
        public int LeaveID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public DateTime SendDate { get; set; }
        public Nullable<DateTime> AuditDate { get; set; }
        public LeaveState LeaveState { get; set; }
    }
}
