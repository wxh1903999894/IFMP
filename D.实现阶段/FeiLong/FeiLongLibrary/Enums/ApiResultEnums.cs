using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiLongLibrary.Enums
{

    /// <summary>
    /// Result状态
    /// </summary>
    public enum ApiResultCodeType
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("Success")]
        Success = 0,
        /// <summary>
        /// 失败
        /// </summary>
        [Description("Failure")]
        Failure = 1,
    }
}
