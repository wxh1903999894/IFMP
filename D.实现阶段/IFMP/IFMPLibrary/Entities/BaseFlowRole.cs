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
    [Table("Tb_BaseFlowRole")]
    public class BaseFlowRole
    {
        [Key]
        public int ID { get; set; }
        public int FlowID { get; set; }
        public int RoleID { get; set; }
    }
}
