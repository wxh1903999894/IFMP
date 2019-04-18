using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{

    [Table("FL_SysLog")]
    public class SysLog
    {
        [Key]
        public int ID { get; set; }
 
        /// <summary>
        /// 用户IP
        /// </summary>
        public string IP { get; set; }   
        
        /// <summary>
        ///涉及用户
        /// </summary>
        public string InvolvedUser { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Message { get; set; }
              
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType Type { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
}