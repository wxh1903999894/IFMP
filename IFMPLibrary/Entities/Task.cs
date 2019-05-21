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
    [Table("Tb_Task")]
    public class Task
    {
        [Key]
        public int ID { get; set; }
        public string TaskName { get; set; }
        public ClassTypeEnums ClassType { get; set; }
        public int TableTypeID { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUserID { get; set; }
    }
}
