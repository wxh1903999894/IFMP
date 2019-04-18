using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Entities
{
    //基础表单结构
    [Table("FL_TableColumn")]
    public class TableColumn
    {
        [Key]
        public int ID { get; set; }
        public TableTypeEnums TableType { get; set; }
        public string ColumnName { get; set; }
        public Nullable<int> DictionaryID { get; set; }
        public string DefaultData { get; set; }
        public int Order { get; set; }
        public bool IsStats { get; set; }
        //public DateTime StatsDate { get; set; }
        //public StatsFrequencyEnums StatsFrequency { get; set; }
    }
}
