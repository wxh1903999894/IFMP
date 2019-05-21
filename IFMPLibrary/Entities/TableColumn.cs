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
    [Table("Tb_TableColumn")]
    //这个表目前计划是不给修改
    public class TableColumn
    {
        [Key]
        public int ID { get; set; }
        public int TableTypeID { get; set; }
        //做特殊化使用
        //public Nullable<int> FlowID { get; set; }
        public string ColumnName { get; set; }
        //这个判断合法，不合法的数据无法提交
        public Nullable<int> DictionaryID { get; set; }
        //这个判断提示，范围以外的数据提交时需要提示
        public Nullable<int> HintDictionaryID { get; set; }
        public string DefaultData { get; set; }//默认文字
        public int Order { get; set; }
        public bool IsStats { get; set; }//是否作为统计字段

        //大屏自动切换用，记录自动切换时统计字段的默认显示类型
        public Nullable<ColumnStatType> ColumnStatType { get; set; }
        //大屏自动切换用，纪录可能存在的统计字段
        public Nullable<int> StatTableColumnID { get; set; }

        //是否必填，这个先不加，考虑到列出的数据应该都是必填的
        //public bool IsNeccesary { get; set; }
        //是否为填写项目
        public bool IsFill { get; set; }
        //填写类型
        public Nullable<ColumnShowType> ColumnShowType { get; set; }
    }
}
