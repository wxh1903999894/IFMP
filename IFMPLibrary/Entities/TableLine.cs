/**************************************************
 * 创建人：     樊紫红
 * 创建时间：   2019年3月26日 9时16分
 * 描述：       流程线与表单类型关系表
 * 
**************************************************/
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
    [Table("Tb_TableLine")]
    public class TableLine
    {
        [Key]
        public int ID { get; set; }
        public int LineID { get; set; }
        public int TableTypeID { get; set; }
    }
}
