/*****************************************************************
** Copyright (c) 芜湖高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 09点39分
** 描   述:      员工档案实体类
** 修 改 人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;

namespace GK.IFMP.Entities
{

    public class EmployeeEntity
    {

        /// <summary>
        /// Employee表实体
        ///</summary>
        public EmployeeEntity()
        {
        }


        /// <summary>
        /// Employee表实体
        /// </summary>
        /// <param name="eid">用户ID</param>
        /// <param name="realname">真实姓名</param>
        /// <param name="sex">性别</param>
        /// <param name="birthdate">生日</param>
        /// <param name="createdate">录入日期</param>
        /// <param name="createuser">创建人</param>
        /// <param name="nationality">民族</param>
        /// <param name="polity">政治面貌</param>
        /// <param name="begindate">入职日期</param>
        /// <param name="etype">人员类别</param>
        /// <param name="depid">部门</param>
        /// <param name="postid">岗位</param>
        /// <param name="jobname">职务</param>
        /// <param name="empcode">员工编号</param>
        /// <param name="censusaddr">家庭住址</param>
        /// <param name="periodday">试用时间</param>
        /// <param name="correctiondate">转正日期</param>
        /// <param name="estate">用户状态 1:试用期   2：在职    3：离职  4：其他</param>
        /// <param name="isdel">是否删除</param>
        public EmployeeEntity(string eid, string realname, int sex, DateTime birthdate, DateTime createdate, string createuser, int nationality, int polity, DateTime begindate, int etype, int depid, int postid, string jobname, string empcode, string censusaddr, int periodday, DateTime correctiondate, int estate, int isdel)
        {
            this.EID = eid;
            this.RealName = realname;
            this.Sex = sex;
            this.Birthdate = birthdate;
            this.CreateDate = createdate;
            this.CreateUser = createuser;
            this.Nationality = nationality;
            this.Polity = polity;
            this.Begindate = begindate;
            this.EType = etype;
            this.DepID = depid;
            this.PostID = postid;
            this.JobName = jobname;
            this.EmpCode = empcode;
            this.Censusaddr = censusaddr;
            this.periodDay = periodday;
            this.CorrectionDate = correctiondate;
            this.EState = estate;
            this.Isdel = isdel;
        }

        private string eid;//用户ID
        private string realname;//真实姓名
        private int sex;//性别
        private DateTime birthdate;//生日
        private DateTime createdate;//录入日期
        private string createuser;//创建人
        private int nationality;//民族
        private int polity;//政治面貌
        private DateTime begindate;//入职日期
        private int etype;//人员类别
        private int depid;//部门
        private int postid;//岗位
        private string jobname;//职务
        private string empcode;//员工编号
        private string censusaddr;//家庭住址
        private int periodday;//试用时间
        private DateTime correctiondate;//转正日期
        private int estate;//用户状态 1:试用期   2：在职    3：离职  4：其他
        private int isdel;//是否删除


        ///<summary>
        ///用户ID
        ///</summary>
        public string EID
        {
            get
            {
                return eid;
            }
            set
            {
                eid = value;
            }
        }

        ///<summary>
        ///真实姓名
        ///</summary>
        public string RealName
        {
            get
            {
                return realname;
            }
            set
            {
                realname = value;
            }
        }

        ///<summary>
        ///性别
        ///</summary>
        public int Sex
        {
            get
            {
                return sex;
            }
            set
            {
                sex = value;
            }
        }

        ///<summary>
        ///生日
        ///</summary>
        public DateTime Birthdate
        {
            get
            {
                return birthdate;
            }
            set
            {
                birthdate = value;
            }
        }

        ///<summary>
        ///录入日期
        ///</summary>
        public DateTime CreateDate
        {
            get
            {
                return createdate;
            }
            set
            {
                createdate = value;
            }
        }

        ///<summary>
        ///创建人
        ///</summary>
        public string CreateUser
        {
            get
            {
                return createuser;
            }
            set
            {
                createuser = value;
            }
        }

        ///<summary>
        ///民族
        ///</summary>
        public int Nationality
        {
            get
            {
                return nationality;
            }
            set
            {
                nationality = value;
            }
        }

        ///<summary>
        ///政治面貌
        ///</summary>
        public int Polity
        {
            get
            {
                return polity;
            }
            set
            {
                polity = value;
            }
        }

        ///<summary>
        ///入职日期
        ///</summary>
        public DateTime Begindate
        {
            get
            {
                return begindate;
            }
            set
            {
                begindate = value;
            }
        }

        ///<summary>
        ///人员类别
        ///</summary>
        public int EType
        {
            get
            {
                return etype;
            }
            set
            {
                etype = value;
            }
        }

        ///<summary>
        ///部门
        ///</summary>
        public int DepID
        {
            get
            {
                return depid;
            }
            set
            {
                depid = value;
            }
        }

        ///<summary>
        ///岗位
        ///</summary>
        public int PostID
        {
            get
            {
                return postid;
            }
            set
            {
                postid = value;
            }
        }

        ///<summary>
        ///职务
        ///</summary>
        public string JobName
        {
            get
            {
                return jobname;
            }
            set
            {
                jobname = value;
            }
        }

        ///<summary>
        ///员工编号
        ///</summary>
        public string EmpCode
        {
            get
            {
                return empcode;
            }
            set
            {
                empcode = value;
            }
        }

        ///<summary>
        ///家庭住址
        ///</summary>
        public string Censusaddr
        {
            get
            {
                return censusaddr;
            }
            set
            {
                censusaddr = value;
            }
        }

        ///<summary>
        ///试用时间
        ///</summary>
        public int periodDay
        {
            get
            {
                return periodday;
            }
            set
            {
                periodday = value;
            }
        }

        ///<summary>
        ///转正日期
        ///</summary>
        public DateTime CorrectionDate
        {
            get
            {
                return correctiondate;
            }
            set
            {
                correctiondate = value;
            }
        }

        ///<summary>
        ///用户状态 1:试用期   2：在职    3：离职  4：其他
        ///</summary>
        public int EState
        {
            get
            {
                return estate;
            }
            set
            {
                estate = value;
            }
        }

        ///<summary>
        ///是否删除
        ///</summary>
        public int Isdel
        {
            get
            {
                return isdel;
            }
            set
            {
                isdel = value;
            }
        }
    }
}

