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
    [Table("Tb_SysUserDetails")]
    public class UserDetails
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public Sex Sex { get; set; }
        public Nationality Nationality { get; set; }
        public Polity Polity { get; set; }
        public Nullable<DateTime> BirthDate { get; set; }

        public string Address { get; set; }

        public string Job { get; set; }

        //进入时间
        public DateTime HireDate { get; set; }
        public int ProbationDays { get; set; }
        //转正时间
        public Nullable<DateTime> QualifiedDate { get; set; }
        public UserType UserType { get; set; }

        public string HeaderUrl { get; set; }

        //身份证
        public string Identity { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
