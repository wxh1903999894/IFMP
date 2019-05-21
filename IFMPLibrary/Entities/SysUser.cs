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
    [Table("Tb_SysUser")]
    public class User
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string UserNumber { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// 注册添加人
        /// </summary>
        public Nullable<int> CreateUserID { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public System.Nullable<DateTime> LastLogindate { get; set; }

        /// <summary>
        /// 账号是不是被禁用
        /// </summary>
        public Nullable<bool> IsDel { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserState UserState { get; set; }

        /// <summary>
        /// 登陆后的Token值
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Token过期时间
        /// </summary>
        public System.Nullable<DateTime> Expires { get; set; }

        /// <summary>
        /// 钉钉的ID，发送消息时用
        /// </summary>
        public string DDID { get; set; }

        public UserLeaveType UserLeaveType { get; set; }

        [NotMapped]
        public List<int> RoleList { get; set; }

        [NotMapped]
        public int Total { get; set; }

        [NotMapped]
        public int PlusTotal { get; set; }

        [NotMapped]
        public int NegativeTotal { get; set; }
    }
}
