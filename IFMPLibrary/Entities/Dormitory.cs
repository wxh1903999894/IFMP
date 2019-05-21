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
    [Table("Tb_Dormitory")]
    public class Dormitory
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 宿舍名称
        /// </summary>
        public string DormiName { get; set; }

        /// <summary>
        /// 宿舍人员
        /// </summary>
        public string DormiUser { get; set; }

        /// <summary>
        /// 宿舍编号
        /// </summary>
        public string DormiCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string DormiDesc { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// 是否点检
        /// </summary>
        public bool IsCheck { get; set; }
    }
}
