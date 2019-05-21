using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IFMPLibrary.Entities
{
    [Table("Tb_SysRole_Right")]
    public class RoleRight
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public string Buttons { get; set; }
    }
}
