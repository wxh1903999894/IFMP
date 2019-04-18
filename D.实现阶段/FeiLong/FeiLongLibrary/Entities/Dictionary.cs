using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //数据字典
    [Table("FL_Dictionary")]
    public class Dictionary
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public DictionaryTypeEnums DisplayType { get; set; }
        //public string DefaultData { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public int CreateUserID { get; set; }

        [NotMapped]
        public List<DictionaryData> DataList { get; set; }
    }
}
