using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //班次-任务
    [Table("FL_ClassTask")]
    public class ClassTask
    {
        [Key]
        public int ID { get; set; }

        public int ClassID { get; set; }
        public int TaskID { get; set; }
    }
}
