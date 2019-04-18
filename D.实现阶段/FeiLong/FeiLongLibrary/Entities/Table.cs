using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //实际提交的表单
    [Table("FL_Table")]
    public class Table
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int TaskID { get; set; }
        public TableTypeEnums TableType { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUser { get; set; }

    }
}
