using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //任务-实际
    [Table("FL_Task")]
    public class FLTask
    {
        [Key]
        public int ID { get; set; }
        public string TaskName { get; set; }

        public ClassTypeEnums ClassType { get; set; }

        public TableTypeEnums TableType { get; set; }
        //public int TableID { get; set; }

        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        //public int ClassID { get; set; }
        //是否使用默认的班次
        [NotMapped]
        public Nullable<bool> IsBaseClass { get; set; }      
        [NotMapped]
        public List<TaskFlow> TaskFlowList { get; set; }
    }
}
