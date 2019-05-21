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
    [Table("Tb_TableColumnRange")]
    //TableColumn的附表，纪录
    public class TableColumnRange
    {
        [Key]
        public int ID { get; set; }
        //无需填写的表单字段ID
        public int TableColumnID { get; set; }
        //需要计算的ID
        public int SourceID { get; set; }
    }
}
