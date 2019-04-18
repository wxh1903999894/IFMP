using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeiLongLibrary.Entities
{
    [Table("FL_Role")]
    public class Role
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }

    }

}