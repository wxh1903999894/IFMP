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
    [Table("Tb_ScoreTask")]
    public class ScoreTask
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }

        public int CreateUserID { get; set; }
        public DateTime EndDate { get; set; }
        public int FirstAuditUserID { get; set; }
        public Nullable<DateTime> FirstAuditDate { get; set; }
        public string FirstAuditMark { get; set; }
        public int LastAuditUserID { get; set; }
        public Nullable<DateTime> LastAuditDate { get; set; }
        public string LastAuditMark { get; set; }
        public AuditState AuditState { get; set; }
        public int CompleteBScore { get; set; }
        public int SignBScore { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
