using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //权限
    [Table("FL_Authorization")]
    public class Authorization
    {

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShowName { get; set; }
        //public List<Role> RoleList { get; set; }
        //public List<SysUser> UserList { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public int CreateUserID { get; set; }
    }
}
