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
    [Table("Tb_ScoreAuditUser")]
    public class ScoreAuditUser
    {
        [Key]
        public int ID { get; set; }
       
        public int UserID { get; set; }

        public ScoreAuditUserType ScoreAuditUserType { get; set; }
        
    }
}
