using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IFMPLibrary.Entities
{
    [Table("Tb_SysModule")]
    public class SysModule
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string ModuleUrl { get; set; }
        public string ModuleIcon { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<int> ModuleOrder { get; set; }
        public Nullable<int> IsRight { get; set; }
        public string ModuleButton { get; set; }
    }
}
