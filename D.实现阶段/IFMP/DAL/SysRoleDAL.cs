/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 9时10分19秒
** 描    述:      权限的基本操作类
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
    public partial class SysRoleDAL : DataEntity<SysRoleEntity>
    {
        #region 获取角色信息
        public DataTable GetList(int isdel)
        {
            string sql = "select * from Tb_SysRole where Isdel=" + isdel;
            if (ExecuteStoredCommandtext(DataOperationValue.SEL_OPERATION, sql).DataReturn.SqlCode != 0)
            {
                throw new Exception(DataReturn.SqlMessage);
            }
            return DataReflectionContainer;
        }
        #endregion
    }
}
