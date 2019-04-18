/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月13日 08时08分48秒
** 描    述:      部门操作类
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Data;

using GK.IFMP.Entities;
using gk.rjb_Y.DataEntityProvider;
using gk.rjb_Y.Libraries;
using gk.rjb_Y.DBAccessConvertorProvider;


namespace GK.IFMP.DAL
{
    public partial class DepartmentDAL : DataEntity<DepartmentEntity>
    {
        #region 添加一条记录
        /// <summary>
        /// 添加一条记录
        ///</summary>
        public int Edit(DepartmentEntity model)
        {
            int result = 0;
            DbParameters.Clear();
            ProcedureName = "up_Tb_Department_Add";
            DataAccessChannelProtection = true;

            DbParameters.Add(new DatabaseParameter("result", result, DatabaseType.SQL_Int, 4, ParameterDirection.Output));
            DbParameters.Add(new DatabaseParameter("DID", model.DID, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("DepName", model.DepName, DatabaseType.SQL_NVarChar, 50));
            DbParameters.Add(new DatabaseParameter("Master", model.Master, DatabaseType.SQL_NVarChar, 40));
            DbParameters.Add(new DatabaseParameter("PID", model.PID, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("DepOrder", model.DepOrder, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("DepType", model.DepType, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("DepMark", model.DepMark, DatabaseType.SQL_NVarChar, 500));
            DbParameters.Add(new DatabaseParameter("Isdel", model.Isdel, DatabaseType.SQL_Int, 4));

            STMessage stmessage = ExecuteStoredProcedure(DataOperationValue.IDU_OPERATION).DataReturn;
            if (stmessage.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            DataAccessChannel.CommitRelease();
            DataAccessChannelProtection = false;

            result = Convert.ToInt32(DbParameters[0].Value);
            return result;
        }
        #endregion


        #region 根据主键编号集合删除记录
        /// <summary>
        /// 根据主键编号集合删除记录
        ///</summary>
        public int DeleteBat(string ids, int isdel)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_Department_DelBat";
            DataAccessChannelProtection = true;

            DbParameters.Add(new DatabaseParameter("Ids", ids, DatabaseType.SQL_NVarChar, 2000));
            DbParameters.Add(new DatabaseParameter("Isdel", isdel, DatabaseType.SQL_Int, 4));
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



        #region 获取部门列表信息
        public DataTable GetTable(int isdel, int pid, int deptype)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_Department_GetList";
            DbParameters.Add(new DatabaseParameter("DepType", deptype, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("Isdel", isdel, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("PID", pid, DatabaseType.SQL_Int, 4));
            if (ExecuteStoredProcedure(DataOperationValue.SEL_OPERATION).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            return DataReflectionContainer;
        }
        #endregion


        #region 根据编号（主键）获取项:返回实体对象
        /// <summary>
        /// 根据编号（主键）获取项:返回实体对象
        /// </summary>
        /// <returns></returns>
        public DepartmentEntity GetObjByID(int id)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_Department_Get";
            DbParameters.Add(new DatabaseParameter("DID", id, DatabaseType.SQL_Int, 4));
            if (ExecuteStoredProcedure(DataOperationValue.SEL_OPERATION).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            return First();
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
        public DataTable GetPaged(int pagesize, int pageindex, ref int recordCount, DepartmentEntity model)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_Department_Paged";
            DbParameters.Add(new DatabaseParameter("pagesize", pagesize, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("pageindex", pageindex, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("recordCount", recordCount, DatabaseType.SQL_Int, 4, ParameterDirection.Output));

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