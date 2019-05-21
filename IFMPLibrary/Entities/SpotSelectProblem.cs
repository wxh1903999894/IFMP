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
    [Table("Tb_SpotSelectProblem")]
    public class SpotSelectProblem
    {
        /// <summary>
        /// 点检ID
        /// </summary>
        [Key]
        public int ID { get; set; }
        public string Problem { get; set; }
        public int Order { get; set; }
        public Nullable<bool> IsDel { get; set; }

    }
}
