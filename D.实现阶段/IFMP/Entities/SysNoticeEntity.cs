/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 09点44分
** 描   述:      通知信息实体类
** 修 改 人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;

namespace GK.IFMP.Entities
{

    public class SysNoticeEntity
    {

        /// <summary>
        /// SysNotice表实体
        ///</summary>
        public SysNoticeEntity()
        {
        }


        /// <summary>
        /// SysNotice表实体
        /// </summary>
        /// <param name="snid">通知ID</param>
        /// <param name="ncontent">通知内容</param>
        /// <param name="senddate">发送日期</param>
        /// <param name="ntype">消息类型</param>
        /// <param name="objid">对象ID</param>
        /// <param name="isread">是否已读</param>
        /// <param name="issendmess">是否发送短信</param>
        /// <param name="issendwei">是否发送微信</param>
        /// <param name="acceptuser">接收人</param>
        /// <param name="readdate">阅读时间</param>
        public SysNoticeEntity(int snid, string ncontent, DateTime senddate, int ntype, string objid, int isread, int issendmess, int issendwei, string acceptuser, DateTime readdate)
        {
            this.SNID = snid;
            this.NContent = ncontent;
            this.SendDate = senddate;
            this.NType = ntype;
            this.ObjID = objid;
            this.IsRead = isread;
            this.IsSendMess = issendmess;
            this.IsSendWei = issendwei;
            this.AcceptUser = acceptuser;
            this.ReadDate = readdate;
        }

        private int snid;//通知ID
        private string ncontent;//通知内容
        private DateTime senddate;//发送日期
        private int ntype;//消息类型
        private string objid;//对象ID
        private int isread;//是否已读
        private int issendmess;//是否发送短信
        private int issendwei;//是否发送微信
        private string acceptuser;//接收人
        private DateTime readdate;//阅读时间


        ///<summary>
        ///通知ID
        ///</summary>
        public int SNID
        {
            get
            {
                return snid;
            }
            set
            {
                snid = value;
            }
        }

        ///<summary>
        ///通知内容
        ///</summary>
        public string NContent
        {
            get
            {
                return ncontent;
            }
            set
            {
                ncontent = value;
            }
        }

        ///<summary>
        ///发送日期
        ///</summary>
        public DateTime SendDate
        {
            get
            {
                return senddate;
            }
            set
            {
                senddate = value;
            }
        }

        ///<summary>
        ///消息类型
        ///</summary>
        public int NType
        {
            get
            {
                return ntype;
            }
            set
            {
                ntype = value;
            }
        }

        ///<summary>
        ///对象ID
        ///</summary>
        public string ObjID
        {
            get
            {
                return objid;
            }
            set
            {
                objid = value;
            }
        }

        ///<summary>
        ///是否已读
        ///</summary>
        public int IsRead
        {
            get
            {
                return isread;
            }
            set
            {
                isread = value;
            }
        }

        ///<summary>
        ///是否发送短信
        ///</summary>
        public int IsSendMess
        {
            get
            {
                return issendmess;
            }
            set
            {
                issendmess = value;
            }
        }

        ///<summary>
        ///是否发送微信
        ///</summary>
        public int IsSendWei
        {
            get
            {
                return issendwei;
            }
            set
            {
                issendwei = value;
            }
        }

        ///<summary>
        ///接收人
        ///</summary>
        public string AcceptUser
        {
            get
            {
                return acceptuser;
            }
            set
            {
                acceptuser = value;
            }
        }

        ///<summary>
        ///阅读时间
        ///</summary>
        public DateTime ReadDate
        {
            get
            {
                return readdate;
            }
            set
            {
                readdate = value;
            }
        }
    }
}

