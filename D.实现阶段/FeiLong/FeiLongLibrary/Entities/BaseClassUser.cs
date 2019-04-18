using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //基础班次-用户
    [Table("FL_BaseClassUser")]
    public class BaseClassUser
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int BaseClassID { get; set; }
        public int FlowID { get; set; }
        public int UserID { get; set; }
    }
}
