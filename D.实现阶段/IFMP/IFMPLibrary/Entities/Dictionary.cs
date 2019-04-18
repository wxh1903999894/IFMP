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
    [Table("Tb_Dictionary")]
    public class Dictionary
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        //填写的字段，可能需要做验证
        public DictionaryTypeEnums DisplayType { get; set; }
        //public string DefaultData { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUserID { get; set; }
        //正则判断
        public Nullable<RegexType> RegexType { get; set; }
        //在特殊匹配时需要做的匹配数据（如：有范围的数据，10|15）,考虑实际使用次数有限的原因，不单独做一个附表了
        public string RegexData { get; set; }

        [NotMapped]
        public List<DictionaryData> DataList { get; set; }
    }
}
