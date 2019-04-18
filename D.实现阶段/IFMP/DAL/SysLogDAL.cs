/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 10时54分56秒
** 描    述:      日志操作类
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using GK.IFMP.Entities;
using gk.rjb_Y.DataEntityProvider;
using gk.rjb_Y.Libraries;
using gk.rjb_Y.DBAccessConvertorProvider;


namespace GK.IFMP.DAL
{
    public partial class SysLogDAL : DataEntity<SysLogEntity>
    {
        #region 添加一条记录
        /// <summary>
        /// 添加一条记录
        ///</summary>
        public int Edit(SysLogEntity model)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_SysLog_Add";
            DataAccessChannelProtection = true;

            DbParameters.Add(new DatabaseParameter("LogType", model.LogType, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("LogContent", model.LogContent, DatabaseType.SQL_NVarChar, 200));
            DbParameters.Add(new DatabaseParameter("CreateUser", model.CreateUser, DatabaseType.SQL_NVarChar, 40));

            STMessage stmessage = ExecuteStoredProcedure(DataOperationValue.IDU_OPERATION).DataReturn;
            if (stmessage.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            DataAccessChannel.CommitRelease();
            DataAccessChannelProtection = false;
            return stmessage.AffectRows;
        }
        #endregion


        #region 根据实体条件分页获取数据集，返回DataSet
        /// <summary>
        /// 根据实体条件分页获取数据集，返回DataSet
        /// </summary>
        /// <param name="pagesize">每页显示条数</param>
        /// <param name="pageindex">当前页码,从1开始</param>
        /// <param name="recordCount">为-1时统计满足条件的记录总数</param>
        /// <param name="model">条件实体</param>
        public DataTable GetPaged(int pagesize, int pageindex, ref int recordCount, SysLogEntity model, DateTime begin, DateTime end)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_SysLog_Paged";
            DbParameters.Add(new DatabaseParameter("pagesize", pagesize, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("pageindex", pageindex, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("recordCount", recordCount, DatabaseType.SQL_Int, 4, ParameterDirection.Output));
            DbParameters.Add(new DatabaseParameter("Begin", begin, DatabaseType.SQL_DateTime, 8));
            DbParameters.Add(new DatabaseParameter("End", end, DatabaseType.SQL_DateTime, 8));
            DbParameters.Add(new DatabaseParameter("CreateUser", model.CreateUser, DatabaseType.SQL_NVarChar, 40));
            DbParameters.Add(new DatabaseParameter("LogType", model.LogType, DatabaseType.SQL_Int, 4));

            if (ExecuteStoredProcedure(DataOperationValue.SEL_OPERATION).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            recordCount = Convert.ToInt32(DbParameters[2].Value);
            return DataReflectionContainer;
        }
        #endregion
    }
}