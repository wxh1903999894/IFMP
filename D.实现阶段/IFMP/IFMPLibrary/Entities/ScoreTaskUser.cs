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
    [Table("Tb_ScoreTask_User")]
    public class ScoreTaskUser
    {
        [Key]
        public int ID { get; set; }
        public int ScoreTaskID { get; set; }
        public int UserID { get; set; }
        public Nullable<DateTime> CompleteDate { get; set; }
    }
}
