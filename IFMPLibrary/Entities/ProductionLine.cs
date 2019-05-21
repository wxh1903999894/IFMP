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
    //生产线
    [Table("Tb_ProductionLine")]
    public class ProductionLine
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }

        [NotMapped]
        public List<TableType> TableTypeList { get; set; }
    }
}
