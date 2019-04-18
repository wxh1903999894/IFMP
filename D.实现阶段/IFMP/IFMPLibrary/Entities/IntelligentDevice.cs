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
    [Table("Tb_IntelligentDevice")]
    public class IntelligentDevice
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        //位置
        public string Place { get; set; }
        //机器标识
        public string Identity { get; set; }
        //负责人
        public int UserID { get; set; }   
        public DeviceType DeviceType { get; set; }
        //获取数据开始时间
        public Nullable<DateTime> BeginDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
