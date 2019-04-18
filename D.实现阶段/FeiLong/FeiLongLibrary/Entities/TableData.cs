using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //实际提交的表单数据
    [Table("FL_TableData")]
    public class TableData
    {
        [Key]
        public int ID { get; set; }
        public int TableID { get; set; }
        public int TableColumnID { get; set; }
        public string Data { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUser { get; set; }

    }
}
