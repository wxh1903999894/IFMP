using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFMPLibrary.Enums
{
    //public enum TableTypeEnums
    //{
    //    数控车床日常点检卡 = 1,
    //    作业前过程点检表 = 2,
    //    切削液浓度点检表 = 3,
    //    产品工序检查记录表 = 4,
    //    换刀频次表 = 5,
    //    换刀表 = 6,
    //    交接班记录 = 7,
    //    生产日报表 = 8,
    //    三检表 = 9,
    //    加工参数检查表 = 10,
    //    自动钻床日常点检表 = 11,
    //    XBAR控制图 = 12
    //    //交接班纪录 = 100
    //}

    public enum StatsFrequencyEnums
    {
        日 = 1,
        周 = 2,
        月 = 3
    }

    public enum ColumnStatType
    {
        折线图 = 1,
        饼图 = 2,
        柱状图 = 3
    }

    public enum ColumnShowType
    {
        极值 = 1,
        平均值 = 2,
        最大值 = 3,
        最小值 = 4,
        求和 = 5,
    }

}
