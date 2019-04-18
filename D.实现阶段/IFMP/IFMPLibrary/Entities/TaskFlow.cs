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
    [Table("Tb_TaskFlow")]
    public class TaskFlow
    {
        [Key]
        public int ID { get; set; }
        public int TaskID { get; set; }
        public int FlowID { get; set; }
        public Nullable<int> BaseClassID { get; set; }
        public int UserID { get; set; }
        public string Remark { get; set; }
        public string AuditMessage { get; set; }
        public AuditResult AuditResult { get; set; }
        public bool IsReminded { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime RemindDate { get; set; }
        public DateTime EndDate { get; set; }
        public Nullable<DateTime> ApplyDate { get; set; }
        public ApplyTypeEnums ApplyType { get; set; }
    }
}
