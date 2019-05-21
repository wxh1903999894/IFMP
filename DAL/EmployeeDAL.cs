/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 15时16分19秒
** 描    述:      员工的基本操作类
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GK.IFMP.Entities;
using gk.rjb_Y.Libraries;
using gk.rjb_Y.DataEntityProvider;
using gk.rjb_Y.DBAccessConvertorProvider;

namespace GK.IFMP.DAL
{
    public partial class EmployeeDAL : DataEntity<EmployeeEntity>
    {
        #region 添加一条记录
        /// <summary>
        /// 添加一条记录
        ///</summary>
        public int Edit(EmployeeEntity model)
        {
            //int result = 0;
            DbParameters.Clear();
            ProcedureName = "up_Tb_Employee_Add";
            DataAccessChannelProtection = true;

            //DbParameters.Add(new DatabaseParameter("result", result, DatabaseType.SQL_Int, 4, ParameterDirection.Output));
            DbParameters.Add(new DatabaseParameter("EID", model.EID, DatabaseType.SQL_NVarChar, 40));
            DbParameters.Add(new DatabaseParameter("RealName", model.RealName, DatabaseType.SQL_NVarChar, 30));
            DbParameters.Add(new DatabaseParameter("Sex", model.Sex, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("Birthdate", model.Birthdate, DatabaseType.SQL_DateTime, 8));
            DbParameters.Add(new DatabaseParameter("CreateUser", model.CreateUser, DatabaseType.SQL_NVarChar, 40));
            DbParameters.Add(new DatabaseParameter("Nationality", model.Nationality, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("Polity", model.Polity, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("Begindate", model.Begindate, DatabaseType.SQL_DateTime, 8));
            DbParameters.Add(new DatabaseParameter("EType", model.EType, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("DepID", model.DepID, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("PostID", model.PostID, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("JobName", model.JobName, DatabaseType.SQL_NVarChar, 100));
            DbParameters.Add(new DatabaseParameter("EmpCode", model.EmpCode, DatabaseType.SQL_NVarChar, 20));
            DbParameters.Add(new DatabaseParameter("Censusaddr", model.Censusaddr, DatabaseType.SQL_NVarChar, 500));
            DbParameters.Add(new DatabaseParameter("periodDay", model.periodDay, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("CorrectionDate", model.CorrectionDate, DatabaseType.SQL_DateTime, 8));
            DbParameters.Add(new DatabaseParameter("EState", model.EState, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("Isdel", model.Isdel, DatabaseType.SQL_Int, 4));

            STMessage stmessage = ExecuteStoredProcedure(DataOperationValue.IDU_OPERATION).DataReturn;
            if (stmessage.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            DataAccessChannel.CommitRelease();
            DataAccessChannelProtection = false;

            //result = Convert.ToInt32(DbParameters[0].Value);
            return stmessage.AffectRows;
        }
        #endregion


        #region 根据编号（主键）获取项:返回实体对象
        public EmployeeEntity GetObj(string id)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_Employee_Get";
            DbParameters.Add(new DatabaseParameter("EID", id, DatabaseType.SQL_NVarChar, 40));
            if (ExecuteStoredProcedure(DataOperationValue.SEL_OPERATION).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            return First();
        }
        #endregion


        #region 根据编号（主键）获取项:返回实体对象
        public DataTable GetInfo(string id)
        {
            string sql = "select *,dbo.getDepName(DepID) as DepName from Tb_SysUser a inner join Tb_Employee b on a.SysID=b.EID where EID='" + id + "'";
            if (ExecuteStoredCommandtext(DataOperationValue.SEL_OPERATION, sql).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            return DataReflectionContainer;
        }
        #endregion


        #region 根据部门ID获取人员信息
        /// <summary>
        /// 根据部门ID获取人员信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetByDID(int id, int estate)
        {
            string sql = "SELECT * FROM [Tb_Employee] WHERE DepID=" + id + " and EState=" + estate + " and Isdel=0 order by CreateDate";
            if (ExecuteStoredCommandtext(DataOperationValue.SEL_OPERATION, sql).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            return DataReflectionContainer;
        }
        #endregion
    }
}
