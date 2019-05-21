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
    [Table("Tb_SpotProblem")]
    public class SpotProblem
    {
        [Key]
        public int SpId { get; set; }

        /// <summary>
        /// 点检ID
        /// </summary>
        public int SpotId { get; set; }

        /// <summary>
        /// 宿舍问题描述
        /// </summary>
        public string ProDesc { get; set; }

        /// <summary>
        /// 宿舍选择的问题
        /// </summary>
        public string SelectDesc { get; set; }

        /// <summary>
        /// 问题责任人
        /// </summary>
        public string DutyUser { get; set; }

        /// <summary>
        /// 点检人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 点检日期
        /// </summary>
        public Nullable<DateTime> CreateDate { get; set; }

        /// <summary>
        /// 复查人员
        /// </summary>
        public string ReviewUser { get; set; }

        /// <summary>
        /// 复查意见
        /// </summary>
        public string ReviewMemo { get; set; }

        /// <summary>
        /// 复查日期
        /// </summary>
        public Nullable<DateTime> ReviewDate { get; set; }

        /// <summary>
        /// 是否复查
        /// </summary>
        public Nullable<bool> IsreView { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string SImage { get; set; }

    }
}
