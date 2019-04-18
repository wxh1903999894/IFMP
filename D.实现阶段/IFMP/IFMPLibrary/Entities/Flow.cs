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
    [Table("Tb_Flow")]
    public class Flow
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int TableTypeID { get; set; }
        public int ParentID { get; set; }
        public bool IsAudit { get; set; }
        [NotMapped]
        public Nullable<int> Level { get; set; }
    }
}
