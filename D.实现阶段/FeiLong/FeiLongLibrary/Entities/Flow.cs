using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //流程结构表
    [Table("FL_Flow")]
    public class Flow
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public TableTypeEnums TableType { get; set; }      
        public int ParentID { get; set; }

    }
}
