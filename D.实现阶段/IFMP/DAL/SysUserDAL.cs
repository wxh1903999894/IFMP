/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月11日 10时28分19秒
** 描    述:      用户信息的基本操作类
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Data;

using GK.IFMP.Entities;
using gk.rjb_Y.Libraries;
using gk.rjb_Y.DataEntityProvider;
using gk.rjb_Y.DBAccessConvertorProvider;
using System.Transactions;

namespace GK.IFMP.DAL
{
    public partial class SysUserDAL : DataEntity<SysUserEntity>
    {
        #region 添加一条记录
        /// <summary>
        /// 添加一条记录
        ///</summary>
        public int Edit(SysUserEntity model)
        {
            int result = 0;
            DbParameters.Clear();
            ProcedureName = "up_Tb_SysUser_Add";
            DataAccessChannelProtection = true;

            DbParameters.Add(new DatabaseParameter("result", result, DatabaseType.SQL_Int, 4, ParameterDirection.Output));
            DbParameters.Add(new DatabaseParameter("SysID", model.SysID, DatabaseType.SQL_NVarChar, 40));
            DbParameters.Add(new DatabaseParameter("SysUserName", model.SysUserName, DatabaseType.SQL_NVarChar, 30));
            DbParameters.Add(new DatabaseParameter("RealName", model.RealName, DatabaseType.SQL_NVarChar, 30));
            DbParameters.Add(new DatabaseParameter("SysPwd", model.SysPwd, DatabaseType.SQL_NVarChar, 100));
            DbParameters.Add(new DatabaseParameter("CellPhone", model.CellPhone, DatabaseType.SQL_NVarChar, 50));
            DbParameters.Add(new DatabaseParameter("WeiXin", model.WeiXin, DatabaseType.SQL_NVarChar, 100));
            DbParameters.Add(new DatabaseParameter("CreateUser", model.CreateUser, DatabaseType.SQL_NVarChar, 40));
            DbParameters.Add(new DatabaseParameter("UserState", model.UserState, DatabaseType.SQL_Int, 4));
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
            ProcedureName = "up_Tb_SysUser_DelBat";
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


        #region 根据编号（主键）获取项:返回实体对象
        /// <summary>
        /// 根据编号（主键）获取项:返回实体对象
        /// </summary>
        /// <returns></returns>
        public SysUserEntity GetObjByID(string id)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_SysUser_Get";
            DbParameters.Add(new DatabaseParameter("SysID", id, DatabaseType.SQL_NVarChar, 40));
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
        public DataTable GetPaged(int pagesize, int pageindex, ref int recordCount, string realname, int roleid,int isdel)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_SysUser_Paged";
            DbParameters.Add(new DatabaseParameter("pagesize", pagesize, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("pageindex", pageindex, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("recordCount", recordCount, DatabaseType.SQL_Int, 4, ParameterDirection.Output));
            DbParameters.Add(new DatabaseParameter("RealName", realname, DatabaseType.SQL_NVarChar, 50));
            DbParameters.Add(new DatabaseParameter("RoleID", roleid, DatabaseType.SQL_Int, 4));
            DbParameters.Add(new DatabaseParameter("Isdel", isdel, DatabaseType.SQL_Int, 4));

            if (ExecuteStoredProcedure(DataOperationValue.SEL_OPERATION).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            recordCount = Convert.ToInt32(DbParameters[2].Value);
            return DataReflectionContainer;
        }
        #endregion


        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public SysUserEntity UserLogin(string uname, string pwd)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_SysUser_Login";
            DbParameters.Add(new DatabaseParameter("SysUserName", uname, DatabaseType.SQL_NVarChar, 100));
            DbParameters.Add(new DatabaseParameter("SysPwd", pwd, DatabaseType.SQL_NVarChar, 100));
            if (ExecuteStoredProcedure(DataOperationValue.SEL_OPERATION).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            return First();
        }
        #endregion


        #region 修改密码
        /// <summary>
        /// 修改密码
        ///</summary>
        public int PwdSet(string Ids, string pwd)
        {
            DbParameters.Clear();
            ProcedureName = "up_Tb_SysUser_PwdSet";
            DataAccessChannelProtection = true;
            DbParameters.Add(new DatabaseParameter("Ids", Ids, DatabaseType.SQL_NVarChar, 2000));
            DbParameters.Add(new DatabaseParameter("PassWord", pwd, DatabaseType.SQL_NVarChar, 100));

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


        #region 学生信息导入
        /// <summary>
        /// 学生信息导入
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string Import(SysUserEntity[] list)
        {
            string str = "";
            int resultvalue = -99;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    resultvalue = 0;
                    for (int i = 0; i < list.Length; i++)
                    {
                        int result = 0;
                        SysUserEntity model = list[i];
                        result = Edit(model);
                        if (result == -1)
                        {
                            resultvalue = -1;
                            str = "姓名为：" + model.RealName + "，用户名为：" + model.SysUserName + "的用户信息有误，请检查后重新导入";
                            break;
                        }
                        else if (result == -2)
                        {
                            resultvalue = -2;
                            str = "姓名为：" + model.RealName + "，用户名为：" + model.SysUserName + "的用户信息重复，请检查后重新导入";
                            break;
                        }
                    }
                    if (resultvalue == 0)
                    {
                        ts.Complete();
                    }
                    else if (resultvalue == -2)
                    {
                        resultvalue = -2;
                    }
                    else
                    {
                        resultvalue = -99;
                    }
                }
                catch (Exception)
                {
                    resultvalue = -99;
                }
                finally
                {
                    ts.Dispose();
                }
            }
            return str;
        }
        #endregion
    }
}
