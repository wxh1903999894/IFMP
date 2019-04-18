﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFMPLibrary.Enums
{
    public enum Sex
    {
        男 = 1,
        女 = 2,
        其他 = 99
    }

    public enum Nationality
    {
        汉族 = 1,
        蒙古族 = 2,
        回族 = 3,
        藏族 = 4,
        维吾尔族 = 5,
        苗族 = 6,
        彝族 = 7,
        壮族 = 8,
        布依族 = 9,
        朝鲜族 = 10,
        满族 = 11,
        侗族 = 12,
        瑶族 = 13,
        白族 = 14,
        土家族 = 15,
        哈尼族 = 15,
        哈萨克族 = 16,
        傣族 = 18,
        黎族 = 19,
        傈僳族 = 20,
        佤族 = 21,
        畲族 = 22,
        高山族 = 23,
        拉祜族 = 24,
        水族 = 25,
        东乡族 = 26,
        纳西族 = 27,
        景颇族 = 28,
        柯尔克孜族 = 29,
        土族 = 30,
        达斡尔族 = 31,
        仫佬族 = 32,
        羌族 = 33,
        布朗族 = 34,
        撒拉族 = 35,
        毛南族 = 36,
        仡佬族 = 37,
        锡伯族 = 38,
        阿昌族 = 39,
        普米族 = 40,
        塔吉克族 = 41,
        怒族 = 42,
        乌孜别克族 = 43,
        俄罗斯族 = 44,
        鄂温克族 = 45,
        德昂族 = 46,
        保安族 = 47,
        裕固族 = 48,
        京族 = 49,
        塔塔尔族 = 50,
        独龙族 = 51,
        鄂伦春族 = 52,
        赫哲族 = 53,
        门巴族 = 54,
        珞巴族 = 55,
        基诺族 = 56,
        其他 = 99
    }

    public enum Polity
    {
        中国共产党党员 = 1,
        中国共产党预备党员 = 2,
        中国共产主义青年团团员 = 3,
        中国国民党革命委员会会员 = 4,
        中国民主同盟盟员 = 5,
        中国民主建国会会员 = 6,
        中国民主促进会会员 = 7,
        中国农工民主党党员 = 8,
        中国致公党党员 = 9,
        九三学社社员 = 10,
        台湾民主自治同盟盟员 = 11,
        无党派民主人士 = 12,
        群众 = 13
    }

    public enum UserType
    {
        合同制 = 1,
        临时 = 2,
        其他 = 3
    }

    public enum UserState
    {
        在职 = 1,
        试用期 = 2,
        离职 = 3,

        其他 = 99
    }

    public enum LeaveState
    {
        未审核 = 0,
        审核中 = 1,
        通过 = 2,
        不通过 = 3
    }

    public enum LeaveType
    {
        病假 = 1,
        事假 = 2,
        差假 = 3,

        其他 = 99
    }

    public enum UserLeaveType
    {
        车间人员 = 1,
        后勤人员 = 2,
    }
}
