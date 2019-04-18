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
    [Table("Tb_TableData")]
    public class TableData
    {
        [Key]
        public int ID { get; set; }
        public int TableID { get; set; }
        public int TableColumnID { get; set; }
        public string Data { get; set; }
        public Nullable<bool> IsAlert { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUserID { get; set; }
    }
}
