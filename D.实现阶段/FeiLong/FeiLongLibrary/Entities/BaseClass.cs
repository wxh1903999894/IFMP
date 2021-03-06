﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //班次-基础
    [Table("FL_BaseClass")]
    public class BaseClass
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public ClassTypeEnums ClassType { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }

        [NotMapped]
        public List<BaseClassUser> BaseClassUserList { get; set; }
    }
}
