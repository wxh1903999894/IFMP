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
    [Table("Tb_TableType")]
    public class TableType
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsMulti { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
