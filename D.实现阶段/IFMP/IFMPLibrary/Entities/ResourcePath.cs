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

    //文件路径
    [Table("Tb_ResourcePath")]
    public class ResourcePath
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }

        [NotMapped]
        public int Count { get; set; }
    }
}
