/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 09点46分
** 描   述:      日志实体类
** 修 改 人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;

namespace GK.IFMP.Entities
{

    public class SysLogEntity
    {

        /// <summary>
        /// SysLog表实体
        ///</summary>
        public SysLogEntity()
        {
        }


        /// <summary>
        /// SysLog表实体
        /// </summary>
        /// <param name="logid">日志ID</param>
        /// <param name="logtype">日志类型</param>
        /// <param name="logcontent">日志内容</param>
        /// <param name="createuser">操作人</param>
        /// <param name="createdate">操作日期</param>
        /// <param name="logflag">日志标识</param>
        public SysLogEntity( int logtype, string logcontent, string createuser)
        {
            this.LogType = logtype;
            this.LogContent = logcontent;
            this.CreateUser = createuser;
        }

        public SysLogEntity( string createuser,int logtype)
        {
            this.LogType = logtype;
            this.CreateUser = createuser;
        }

        private int logid;//日志ID
        private int logtype;//日志类型
        private string logcontent;//日志内容
        private string createuser;//操作人
        private DateTime createdate;//操作日期
        private int logflag;//日志标识


        ///<summary>
        ///日志ID
        ///</summary>
        public int LogID
        {
            get
            {
                return logid;
            }
            set
            {
                logid = value;
            }
        }

        ///<summary>
        ///日志类型
        ///</summary>
        public int LogType
        {
            get
            {
                return logtype;
            }
            set
            {
                logtype = value;
            }
        }

        ///<summary>
        ///日志内容
        ///</summary>
        public string LogContent
        {
            get
            {
                return logcontent;
            }
            set
            {
                logcontent = value;
            }
        }

        ///<summary>
        ///操作人
        ///</summary>
        public string CreateUser
        {
            get
            {
                return createuser;
            }
            set
            {
                createuser = value;
            }
        }

        ///<summary>
        ///操作日期
        ///</summary>
        public DateTime CreateDate
        {
            get
            {
                return createdate;
            }
            set
            {
                createdate = value;
            }
        }

        ///<summary>
        ///日志标识
        ///</summary>
        public int LogFlag
        {
            get
            {
                return logflag;
            }
            set
            {
                logflag = value;
            }
        }
    }
}

