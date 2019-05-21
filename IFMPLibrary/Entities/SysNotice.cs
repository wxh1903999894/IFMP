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
    [Table("Tb_SysNotice")]
    public class Notice
    {
        [Key]
        public int ID { get; set; }
        public string Contenet { get; set; }
        public Nullable<DateTime> SendDate { get; set; }
        public int SendUserID { get; set; }
        public NoticeType NoticeType { get; set; }
        //保存对象
        public Nullable<int> SourceID { get; set; }
        public bool IsSend { get; set; }
        public int ReciveUserID { get; set; }

        public string URL { get; set; }
    }
}
