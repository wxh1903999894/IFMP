using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Utils;

namespace FeiLongLibrary.DAO
{
    public class WXDAO
    {
        public void SendMessage(string Message)
        {
            try
            {
                string accesstoken = new WeiXinUtils().GetAccessToken(ParaUtils.WXMessageSecret);
                //发送
                new WeiXinUtils().SendMessage(accesstoken, LoginHelper.CurrentUser.WXID, Message, ParaUtils.WXMessageAgentID);
            }
            catch
            {
                new SysLogDAO().AddLog(LogType.Failure, message: "发送消息失败");
            }
        }
    }
}
