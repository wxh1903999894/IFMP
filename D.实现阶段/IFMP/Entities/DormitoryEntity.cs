
/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      俞桂宝
** 创建日期:      2019年04月08日 09点30分
** 描   述:      宿舍实体类
** 修 改 人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.IFMP.Entities
{
    public class DormitoryEntity
    {



        private int dormitoryid;//宿舍ID
        private string dorminame;//宿舍名称
        private string dormiuser;//宿舍人员
        private string dormicode;//宿舍编号
        private string dormidesc;//备注
        private string createuser;//操作人
        private DateTime createdate;//操作日期

        public DormitoryEntity()
        {
        }

        /// <summary>
        /// 宿舍
        /// </summary>
        public int DormitoryID
        {
            get { return dormitoryid; }
            set { dormitoryid = value; }
        }

  
        /// <summary>
        /// 宿舍名称
        /// </summary>
        public string DormiName
        {
            get { return dorminame; }
            set { dorminame = value; }
        }

        /// <summary>
        /// 宿舍人员
        /// </summary>
        public string DormiUser
        {
            get { return dormiuser; }
            set { dormiuser = value; }
        }

        public string DormiCode
        {
            get { return dormicode; }
            set { dormicode = value; }
        }

        public string DormiDesc
        {
            get { return dormidesc; }
            set { dormidesc = value; }
        }
      
        public string Createuser
        {
            get { return createuser; }
            set { createuser = value; }
        }
    

        public DateTime Createdate
        {
            get { return createdate; }
            set { createdate = value; }
        }
 

    }
}
