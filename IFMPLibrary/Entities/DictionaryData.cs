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
    [Table("Tb_DictionaryData")]
    public class DictionaryData
    {
        [Key]
        public int ID { get; set; }
        public string Data { get; set; }
        public int DictionaryID { get; set; }
        ////正则判断
        //public RegexType RegexType { get; set; }

        ////在特殊匹配时需要做的匹配数据,考虑实际使用次数有限的原因，不单独做一个附表了
        //public string RegexData { get; set; }

        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUserID { get; set; }
    }
}
