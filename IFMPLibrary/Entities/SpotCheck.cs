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
    [Table("Tb_SpotCheck")]
    public class SpotCheck
    {
        /// <summary>
        /// 点检ID
        /// </summary>
        [Key]
        public int SpotId { get; set; }

        /// <summary>
        /// 宿舍ID
        /// </summary>
        public int DormitoryId { get; set; }

        /// <summary>
        /// 点检人
        /// </summary>
        public int CreateUser { get; set; }

        /// <summary>
        /// 点检日期
        /// </summary>
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// 打分
        /// </summary>
        public int SpotScore { get; set; }

    }
}
