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
    //文件地址
    [Table("Tb_ResourceData")]
    public class ResourceData
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int ResourcePathID { get; set; }
        //是否用作首页轮询
        public Nullable<bool> IsCarousel { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
    }
}
