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
    [Table("Tb_SysLog")]
    public class SysLog
    {
        [Key]
        public int ID { get; set; }      
        public string IP { get; set; }
        public Nullable<int> InvolvedUser { get; set; } 
        public string Message { get; set; }
        public LogType LogType { get; set; }
   
        public DateTime CreateDate { get; set; }
    }
}
