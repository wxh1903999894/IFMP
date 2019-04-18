using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //权限-角色或用户
    [Table("FL_AuthorizationRole")]
    public class AuthorizationRole
    {
        [Key]
        public int ID { get; set; }
        public int AuthorizationID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> UserID { get; set; }
    }
}
