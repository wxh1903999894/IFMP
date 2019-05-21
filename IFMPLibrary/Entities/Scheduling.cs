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
    [Table("Tb_Scheduling")]
    public class Scheduling
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public WeekDate Date { get; set; }

        /// <summary>
        /// 点检人员
        /// </summary>
        public string CheckName { get; set; }
    }
}
