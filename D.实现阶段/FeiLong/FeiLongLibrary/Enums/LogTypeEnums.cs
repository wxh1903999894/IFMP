using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiLongLibrary.Enums
{

    /// <summary>
    /// LogType状态,可考虑添加不同类型的操作日志
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("正常")]
        Success = 0,
        /// <summary>
        /// 失败
        /// </summary>
        [Description("错误")]
        Failure = 1,      
    }

}
