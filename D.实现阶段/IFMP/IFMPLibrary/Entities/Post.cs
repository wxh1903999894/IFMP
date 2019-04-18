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
    [Table("Tb_Post")]
    public class Post
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        //public int MasterUserID { get; set; }

        //public int ParentID { get; set; }

        public Nullable<int> Order { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
