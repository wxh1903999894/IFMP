using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFMPLibrary.Enums
{
    //public enum LogFlag
    //{       
    //    Success = 0,

    //    Failure = 1,
    //}

    public enum LogType
    {
        /// <summary>
        /// 登录日志
        /// </summary>
        登录日志 = 1,
        /// <summary>
        /// 注销日志
        /// </summary>
        注销日志 = 2,
        /// <summary>
        /// 操作日志
        /// </summary>
        操作日志_添加 = 31,
        操作日志_删除 = 32,
        操作日志_修改 = 33,
        操作日志_下载 = 34,
        操作日志_导出 = 35,
        操作日志_导入 = 36,
        操作日志_获取 = 37,
        操作日志_其他 = 39,

        /// 系统日志
        /// </summary>
        系统日志 = 4,


        报警日志 = 5,
    }

}
