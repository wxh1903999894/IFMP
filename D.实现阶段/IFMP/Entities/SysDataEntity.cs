/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 09点47分
** 描   述:      基础数据实体类
** 修 改 人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;

namespace GK.IFMP.Entities
{

    public class SysDataEntity
    {

        /// <summary>
        /// SysData表实体
        ///</summary>
        public SysDataEntity()
        {
        }


        /// <summary>
        /// SysData表实体
        /// </summary>
        /// <param name="sdid">ID</param>
        /// <param name="datacode">编码</param>
        /// <param name="dataname">名称</param>
        /// <param name="datadesc">描述</param>
        /// <param name="dataflag">类型</param>
        /// <param name="issysset">是否系统预设</param>
        /// <param name="pid">父ID</param>
        /// <param name="isdel">是否删除</param>
        public SysDataEntity(int sdid, string datacode, string dataname, string datadesc, int dataflag, int issysset, int pid, int isdel)
        {
            this.SDID = sdid;
            this.DataCode = datacode;
            this.DataName = dataname;
            this.DataDesc = datadesc;
            this.DataFlag = dataflag;
            this.IsSysSet = issysset;
            this.PID = pid;
            this.Isdel = isdel;
        }

        private int sdid;//ID
        private string datacode;//编码
        private string dataname;//名称
        private string datadesc;//描述
        private int dataflag;//类型
        private int issysset;//是否系统预设
        private int pid;//父ID
        private int isdel;//是否删除


        ///<summary>
        ///ID
        ///</summary>
        public int SDID
        {
            get
            {
                return sdid;
            }
            set
            {
                sdid = value;
            }
        }

        ///<summary>
        ///编码
        ///</summary>
        public string DataCode
        {
            get
            {
                return datacode;
            }
            set
            {
                datacode = value;
            }
        }

        ///<summary>
        ///名称
        ///</summary>
        public string DataName
        {
            get
            {
                return dataname;
            }
            set
            {
                dataname = value;
            }
        }

        ///<summary>
        ///描述
        ///</summary>
        public string DataDesc
        {
            get
            {
                return datadesc;
            }
            set
            {
                datadesc = value;
            }
        }

        ///<summary>
        ///类型
        ///</summary>
        public int DataFlag
        {
            get
            {
                return dataflag;
            }
            set
            {
                dataflag = value;
            }
        }

        ///<summary>
        ///是否系统预设
        ///</summary>
        public int IsSysSet
        {
            get
            {
                return issysset;
            }
            set
            {
                issysset = value;
            }
        }

        ///<summary>
        ///父ID
        ///</summary>
        public int PID
        {
            get
            {
                return pid;
            }
            set
            {
                pid = value;
            }
        }

        ///<summary>
        ///是否删除
        ///</summary>
        public int Isdel
        {
            get
            {
                return isdel;
            }
            set
            {
                isdel = value;
            }
        }
    }
}

