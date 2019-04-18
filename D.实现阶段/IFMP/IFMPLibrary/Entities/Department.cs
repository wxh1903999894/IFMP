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
    [Table("Tb_Department")]
    public class Department
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int MasterUserID { get; set; }

        public int ParentID { get; set; }

        public Nullable<int> DingID { get; set; }

        public Nullable<int> Order { get; set; }

        //是否作为行政分组,非行政分组的作为积分制排名用
        public bool IsAdmin { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
