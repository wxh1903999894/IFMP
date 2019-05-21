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
    [Table("Tb_BaseClass")]
    public class BaseClass
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public ClassTypeEnums ClassType { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        //生产线ID
        public int ProductionLineID { get; set; }

        [NotMapped]
        public string ProductionLineName { get; set; }
    }
}
