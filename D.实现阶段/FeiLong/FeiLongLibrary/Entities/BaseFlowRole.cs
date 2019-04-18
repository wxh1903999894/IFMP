using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //基础流程权限表
    [Table("FL_BaseFlowRole")]
    public class BaseFlowRole
    {
        [Key]
        public int ID { get; set; }
        public int FlowID { get; set; }
        public int RoleID { get; set; }

        [NotMapped]
        public List<int> UserIDList { get;set;}
    }
}
