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
    //不排名人员
    [Table("Tb_NoScoreUser")]
    public class NoScoreUser
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

    }
}
