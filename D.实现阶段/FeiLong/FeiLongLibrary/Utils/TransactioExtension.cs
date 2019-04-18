﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FeiLongLibrary.Utils
{
    public static class TransactioExtension
    {
        public static void Excute(params Action[] actions)
        {
            //使用ReadCommitted隔离级别，保持与Sql Server的默认隔离级别一致
            Excute(IsolationLevel.ReadCommitted, null, actions);
        }

        public static void Excute(IsolationLevel level, params Action[] actions)
        {
            Excute(level, null, actions);
        }

        public static void Excute(int timeOut, params Action[] actions)
        {
            Excute(IsolationLevel.ReadCommitted, timeOut, actions);
        }

        public static void Excute(IsolationLevel level, int? timeOut, params Action[] actions)
        {
            if (actions == null || actions.Length == 0)
                return;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = level; //默认为Serializable,这里根据参数来进行调整
            if (timeOut.HasValue)
                options.Timeout = new TimeSpan(0, 0, timeOut.Value); //默认60秒
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Array.ForEach<Action>(actions, action => action());
                tran.Complete(); //通知事务管理器它可以提交事务
            }
        }
    }
}
