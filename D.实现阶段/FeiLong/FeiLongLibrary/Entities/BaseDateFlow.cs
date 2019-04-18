using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //基础时间节点表
    [Table("FL_BaseDateFlow")]
    public class BaseDateFlow
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public ClassTypeEnums ClassType { get; set; }
        public TableTypeEnums TableType { get; set; }
        public int FlowID { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RemindDate { get; set; }
      
    }
}
