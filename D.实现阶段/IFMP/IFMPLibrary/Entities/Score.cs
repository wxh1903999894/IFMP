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
    [Table("Tb_Score")]
    public class Score
    {
        [Key]
        public int ID { get; set; }
        //发起人ID
        public int CreateUserID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ScoreEventID { get; set; }
        public Nullable<int> FirstAuditUserID { get; set; }
        public Nullable<DateTime> FirstAuditDate { get; set; }
        public string FirstAuditMark { get; set; }
        public Nullable<int> LastAuditUserID { get; set; }
        public Nullable<DateTime> LastAuditDate { get; set; }
        public string LastAuditMark { get; set; }
        public string Image { get; set; }
        public AuditState AuditState { get; set; }
        //public ScoreType ScoreType { get; set; }
        public bool IsReward { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDel { get; set; }
       
    }
}
