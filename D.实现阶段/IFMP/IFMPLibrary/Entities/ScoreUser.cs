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
    [Table("Tb_Score_User")]
    public class ScoreUser
    {
        [Key]
        public int ID { get; set; }

        //奖励人ID
        public int UserID { get; set; }
        public int AScore { get; set; }
        public int BScore { get; set; }
        public int ScoreID { get; set; }
        public bool IsPrint { get; set; }
        public Nullable<bool> IsDel { get; set; }
       
    }
}
