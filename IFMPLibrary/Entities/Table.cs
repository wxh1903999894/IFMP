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
    [Table("Tb_Table")]
    public class Table
    {
        [Key]
        public int ID { get; set; }
        //public string Name { get; set; }
        public int TaskID { get; set; }
        public int FlowID { get; set; }
        public int TableTypeID { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUserID { get; set; }

    }
}
