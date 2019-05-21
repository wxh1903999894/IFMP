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
    [Table("Tb_IntelligentDeviceData")]
    public class IntelligentDeviceData
    {
        [Key]
        public int ID { get; set; }
        public int IntelligentDeviceID { get; set; }
        public string Data { get; set; }
        public DeviceDataType DeviceDataType { get; set; }
        public Nullable<bool> IsAlert { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
