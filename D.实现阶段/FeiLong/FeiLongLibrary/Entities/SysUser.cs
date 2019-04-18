using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeiLongLibrary.Entities
{
    [Table("FL_SysUser")]
    public class SysUser
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string TelNumber { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        ///// <summary>
        ///// 手机号码是否验证
        ///// </summary>
        //public Nullable<bool> IsValidTelNumber { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeaderUrl { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public System.Nullable<DateTime> LastLogindate { get; set; }

        /// <summary>
        /// 账号是不是被禁用
        /// </summary>
        public Nullable<bool> IsAccountDisabled { get; set; }

        /// <summary>
        /// 登陆后的Token值
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Token过期时间
        /// </summary>
        public System.Nullable<DateTime> Expires { get; set; }

        /// <summary>
        /// 微信的ID，发送消息时用
        /// </summary>
        public string WXID { get; set; }

        [NotMapped]
        public List<int> RoleList { get; set; }
    }


}