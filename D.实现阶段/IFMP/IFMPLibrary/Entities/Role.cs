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
    [Table("Tb_SysRole")]
    public class Role
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        //不可修改
        public bool IsBase { get; set; }

    }
}
