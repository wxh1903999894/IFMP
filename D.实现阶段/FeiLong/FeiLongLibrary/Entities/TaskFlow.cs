using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //任务流程
    [Table("FL_TaskFlow")]
    public class TaskFlow
    {
        [Key]
        public int ID { get; set; }
        public int TaskID { get; set; }
        public int FlowID { get; set; }
        public int MaintainUserID { get; set; }
        public string Remark { get; set; }
        //判断是否为审核，提供不同的通知/提交策略
        public Nullable<bool> IsAudit { get; set; }
        public string AuditMessage { get; set; }
        public Nullable<bool> IsReminded { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime RemindDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ApplyDate { get; set; }
        public ApplyTypeEnums ApplyType { get; set; }

        [NotMapped]
        public bool IsBase { get; set; }
        [NotMapped]
        public List<int> UserIDList { get; set; }

    }
}
