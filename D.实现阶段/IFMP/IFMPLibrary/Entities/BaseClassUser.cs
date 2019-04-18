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
    [Table("Tb_BaseClassUser")]
    public class BaseClassUser
    {
        [Key]
        public int ID { get; set; }
        public int BaseClassID { get; set; }
        public int FlowID { get; set; }
        public int UserID { get; set; }
    }
}
