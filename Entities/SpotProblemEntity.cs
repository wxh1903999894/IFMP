/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      俞桂宝
** 创建日期:      2019年04月08日 09点30分
** 描   述:      点检问题实体类
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
  public   class SpotProblemEntity
    {
        private int spid;//ID

        private int spotid;//点检ID
        private string prodesc;//问题描述
        private string dutyuser;//  问题责任人
        private string createuser;//点检人
        private DateTime createdate;//点检日期
        private string reviewUser;//复查人员
        private DateTime reviewDate;//复查日期
        private int isreview;//是否复查

          /// <summary>
        /// 实体
        ///</summary>
        public SpotProblemEntity()
        {
        }


        public int IsReview
        {
            get { return isreview; }
            set { isreview = value; }
        } 
        public string ReviewUser
        {
            get { return reviewUser; }
            set { reviewUser = value; }
        }
     

        public DateTime ReviewDate
        {
            get { return reviewDate; }
            set { reviewDate = value; }
        }
        public string DutyUser
        {
            get { return dutyuser; }
            set { dutyuser = value; }
        }

        public string ProDesc
        {
            get { return prodesc; }
            set { prodesc = value; }
        }
      /// <summary>
      /// ID
      /// </summary>
        public int SpID
        {
            get { return spid; }
            set { spid = value; }
        }


        public int SpotID
        {
            get { return spotid; }
            set { spotid = value; }
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

   
    }
}
