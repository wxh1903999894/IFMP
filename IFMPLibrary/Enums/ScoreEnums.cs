using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFMPLibrary.Enums
{
    public enum AuditState
    {
        待初审 = 1,
        待终审 = 2,
        通过 = 3,
        驳回 = 4,
        确认完成 = 5
    }

    public enum ScoreType
    {
        积分制 = 1,
        任务发布 = 2
    }

    public enum ScoreEventTypeEnum
    {
        固定事件 = 1,
        其他事件 = 2
    }

    public enum ScoreAuditUserType
    {
        初审人 = 1,
        终审人 = 2
    }
}
