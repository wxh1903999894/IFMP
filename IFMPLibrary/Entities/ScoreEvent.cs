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
    [Table("Tb_ScoreEvent")]
    public class ScoreEvent
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public int AScore { get; set; }
        public int BScore { get; set; }
        public string Content { get; set; }
        public int ScoreEventTypeID { get; set; }

        public Nullable<bool> IsSpecialUserAudit { get; set; }
        public Nullable<int> FirstAuditUserID { get; set; }
        public Nullable<int> LastAuditUserID { get; set; }
        public ScoreEventTypeEnum ScoreEventType { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
