using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiLongLibrary.Enums
{
    public enum DictionaryTypeEnums
    {
        填写 = 1,
        单选 = 2,
    }

    public enum RegexType
    {
        英文字母 = 1,
        大写英文字母 = 2,
        小写英文字母 = 3,

        整数 = 11,
        实数 = 12,
        非负整数 = 13,
        正整数 = 14,

        手机号 = 51,
        邮箱 = 52,
        身份证号码 = 53,
        邮政编码 = 54,


        英文字母与数字 = 91,
        特殊的一组字符 = 92,

    }
}
