
/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      俞桂宝
** 创建日期:      2019年04月08日 09点30分
** 描   述:      点检实体类
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
    public class SpotCheckEntity
    {
        private int spotid;//点检ID
        private int dormitoryid;//宿舍ID
        private string createuser;//点检人
        private DateTime createdate;//点检日期
        private int spotscore;//打分


        /// <summary>
        /// 实体
        ///</summary>
        public SpotCheckEntity()
        {
        }

        public int SpotScore
        {
            get { return spotscore; }
            set { spotscore = value; }
        }

        public string CreateUser
        {
            get { return createuser; }
            set { createuser = value; }
        }


        public DateTime CreateDate
        {
            get { return createdate; }
            set { createdate = value; }
        }

        public int DormitoryID
        {
            get { return dormitoryid; }
            set { dormitoryid = value; }
        }


        public int SpotID
        {
            get { return spotid; }
            set { spotid = value; }
        }
    }
}
