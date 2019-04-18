/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月12日 05点55分
** 描   述:      部门实体类
** 修 改 人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;

namespace GK.IFMP.Entities
{

    public class DepartmentEntity
    {

        /// <summary>
        /// Department表实体
        ///</summary>
        public DepartmentEntity()
        {
        }


        /// <summary>
        /// Department表实体
        /// </summary>
        /// <param name="did">部门ID</param>
        /// <param name="depname">部门名称</param>
        /// <param name="master">部门负责人</param>
        /// <param name="pid">上级部门</param>
        /// <param name="deporder">排序</param>
        /// <param name="deptype">类型</param>
        /// <param name="depmark">部门简述</param>
        /// <param name="isdel">是否删除</param>
        public DepartmentEntity(int did, string depname, string master, int pid, int deporder, int deptype, string depmark, int isdel)
        {
            this.DID = did;
            this.DepName = depname;
            this.Master = master;
            this.PID = pid;
            this.DepOrder = deporder;
            this.DepType = deptype;
            this.DepMark = depmark;
            this.Isdel = isdel;
        }

        private int did;//部门ID
        private string depname;//部门名称
        private string master;//部门负责人
        private int pid;//上级部门
        private int deporder;//排序
        private int deptype;//类型
        private string depmark;//部门简述
        private int isdel;//是否删除


        ///<summary>
        ///部门ID
        ///</summary>
        public int DID
        {
            get
            {
                return did;
            }
            set
            {
                did = value;
            }
        }

        ///<summary>
        ///部门名称
        ///</summary>
        public string DepName
        {
            get
            {
                return depname;
            }
            set
            {
                depname = value;
            }
        }

        ///<summary>
        ///部门负责人
        ///</summary>
        public string Master
        {
            get
            {
                return master;
            }
            set
            {
                master = value;
            }
        }

        ///<summary>
        ///上级部门
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
        ///排序
        ///</summary>
        public int DepOrder
        {
            get
            {
                return deporder;
            }
            set
            {
                deporder = value;
            }
        }

        ///<summary>
        ///类型
        ///</summary>
        public int DepType
        {
            get
            {
                return deptype;
            }
            set
            {
                deptype = value;
            }
        }

        ///<summary>
        ///部门简述
        ///</summary>
        public string DepMark
        {
            get
            {
                return depmark;
            }
            set
            {
                depmark = value;
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

