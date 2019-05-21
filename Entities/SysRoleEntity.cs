/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 09点42分
** 描   述:      权限实体类
** 修 改 人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;

namespace GK.IFMP.Entities
{

    public class SysRoleEntity
    {

        /// <summary>
        /// SysRole表实体
        ///</summary>
        public SysRoleEntity()
        {
        }


        /// <summary>
        /// SysRole表实体
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="rolename">角色名称</param>
        /// <param name="roledesc">角色备注</param>
        /// <param name="roletype">角色类型 1:业务角色   2：数据角色</param>
        /// <param name="aid">单位ID</param>
        /// <param name="isdel">是否删除</param>
        public SysRoleEntity(int roleid, string rolename, string roledesc, int roletype, int aid, int isdel)
        {
            this.RoleID = roleid;
            this.RoleName = rolename;
            this.RoleDesc = roledesc;
            this.RoleType = roletype;
            this.AID = aid;
            this.Isdel = isdel;
        }

        private int roleid;//角色ID
        private string rolename;//角色名称
        private string roledesc;//角色备注
        private int roletype;//角色类型 1:业务角色   2：数据角色
        private int aid;//单位ID
        private int isdel;//是否删除


        ///<summary>
        ///角色ID
        ///</summary>
        public int RoleID
        {
            get
            {
                return roleid;
            }
            set
            {
                roleid = value;
            }
        }

        ///<summary>
        ///角色名称
        ///</summary>
        public string RoleName
        {
            get
            {
                return rolename;
            }
            set
            {
                rolename = value;
            }
        }

        ///<summary>
        ///角色备注
        ///</summary>
        public string RoleDesc
        {
            get
            {
                return roledesc;
            }
            set
            {
                roledesc = value;
            }
        }

        ///<summary>
        ///角色类型 1:业务角色   2：数据角色
        ///</summary>
        public int RoleType
        {
            get
            {
                return roletype;
            }
            set
            {
                roletype = value;
            }
        }

        ///<summary>
        ///单位ID
        ///</summary>
        public int AID
        {
            get
            {
                return aid;
            }
            set
            {
                aid = value;
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

