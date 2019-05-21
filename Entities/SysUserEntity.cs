/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 09点30分
** 描   述:      用户实体类
** 修 改 人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;

namespace GK.IFMP.Entities
{

    public class SysUserEntity
    {

        /// <summary>
        /// SysUser表实体
        ///</summary>
        public SysUserEntity()
        {
        }


        /// <summary>
        /// SysUser表实体
        /// </summary>
        /// <param name="sysid">用户ID</param>
        /// <param name="sysusername">用户名</param>
        /// <param name="realname">真实姓名</param>
        /// <param name="syspwd">密码</param>
        /// <param name="cellphone">手机号码</param>
        /// <param name="weixin"></param>
        /// <param name="lastdate">最后登录时间</param>
        /// <param name="createdate">创建日期</param>
        /// <param name="createuser">创建人</param>
        /// <param name="token">Token值</param>
        /// <param name="texpires">过期时间</param>
        /// <param name="userstate">用户状态 1:在职  2：离职</param>
        /// <param name="isdel">是否删除</param>
        public SysUserEntity(string sysid, string sysusername, string realname, string syspwd, string cellphone, string weixin, DateTime lastdate, DateTime createdate, string createuser, string token, DateTime texpires, int userstate, int isdel)
        {
            this.SysID = sysid;
            this.SysUserName = sysusername;
            this.RealName = realname;
            this.SysPwd = syspwd;
            this.CellPhone = cellphone;
            this.WeiXin = weixin;
            this.LastDate = lastdate;
            this.CreateDate = createdate;
            this.CreateUser = createuser;
            this.Token = token;
            this.TExpires = texpires;
            this.UserState = userstate;
            this.Isdel = isdel;
        }

        private string sysid;//用户ID
        private string sysusername;//用户名
        private string realname;//真实姓名
        private string syspwd;//密码
        private string cellphone;//手机号码
        private string weixin;//
        private DateTime lastdate;//最后登录时间
        private DateTime createdate;//创建日期
        private string createuser;//创建人
        private string token;//Token值
        private DateTime texpires;//过期时间
        private int userstate;//用户状态 1:在职  2：离职
        private int isdel;//是否删除


        ///<summary>
        ///用户ID
        ///</summary>
        public string SysID
        {
            get
            {
                return sysid;
            }
            set
            {
                sysid = value;
            }
        }

        ///<summary>
        ///用户名
        ///</summary>
        public string SysUserName
        {
            get
            {
                return sysusername;
            }
            set
            {
                sysusername = value;
            }
        }

        ///<summary>
        ///真实姓名
        ///</summary>
        public string RealName
        {
            get
            {
                return realname;
            }
            set
            {
                realname = value;
            }
        }

        ///<summary>
        ///密码
        ///</summary>
        public string SysPwd
        {
            get
            {
                return syspwd;
            }
            set
            {
                syspwd = value;
            }
        }

        ///<summary>
        ///手机号码
        ///</summary>
        public string CellPhone
        {
            get
            {
                return cellphone;
            }
            set
            {
                cellphone = value;
            }
        }

        ///<summary>
        ///
        ///</summary>
        public string WeiXin
        {
            get
            {
                return weixin;
            }
            set
            {
                weixin = value;
            }
        }

        ///<summary>
        ///最后登录时间
        ///</summary>
        public DateTime LastDate
        {
            get
            {
                return lastdate;
            }
            set
            {
                lastdate = value;
            }
        }

        ///<summary>
        ///创建日期
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
        ///创建人
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
        ///Token值
        ///</summary>
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
            }
        }

        ///<summary>
        ///过期时间
        ///</summary>
        public DateTime TExpires
        {
            get
            {
                return texpires;
            }
            set
            {
                texpires = value;
            }
        }

        ///<summary>
        ///用户状态 1:在职  2：离职
        ///</summary>
        public int UserState
        {
            get
            {
                return userstate;
            }
            set
            {
                userstate = value;
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

