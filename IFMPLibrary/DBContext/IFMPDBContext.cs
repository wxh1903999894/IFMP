using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using IFMPLibrary;
using IFMPLibrary.Utils;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;

namespace IFMPLibrary.DBContext
{
    public class IFMPDBContext : DbContext
    {
        public IFMPDBContext()
            : base(ConfigurationManager.ConnectionStrings["GK_IFMP"].ConnectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<IFMPDBContext, Configuration<IFMPDBContext>>());
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<Department> Department { get; set; }
        public DbSet<DepartmentUser> DepartmentUser { get; set; }
        public DbSet<Dormitory> Dormitory { get; set; }

        public DbSet<Scheduling> Scheduling { get; set; }

        public DbSet<DormitoryUser> DormitoryUser { get; set; }
        public DbSet<SpotCheck> SpotCheck { get; set; }

        public DbSet<SpotProblem> SpotProblem { get; set; }

        public DbSet<Post> Post { get; set; }
        public DbSet<PostUser> PostUser { get; set; }

        public DbSet<SysLog> SysLog { get; set; }
        public DbSet<Leave> Leave { get; set; }
        public DbSet<LeaveAudit> LeaveAudit { get; set; }

        public DbSet<Score> Score { get; set; }
        public DbSet<ScoreTask> ScoreTask { get; set; }
        public DbSet<ScoreTaskUser> ScoreTaskUser { get; set; }
        public DbSet<ScoreUser> ScoreUser { get; set; }
        public DbSet<ScoreEvent> ScoreEvent { get; set; }
        public DbSet<ScoreEventType> ScoreEventType { get; set; }
        public DbSet<ScoreAuditUser> ScoreAuditUser { get; set; }
        public DbSet<NoScoreUser> NoScoreUser { get; set; }
        public DbSet<NoScoreUserDepartment> NoScoreUserDepartment { get; set; }

        public DbSet<Notice> Notice { get; set; }

        public DbSet<SysModule> SysModule { get; set; }
        public DbSet<SysButton> SysButton { get; set; }
        public DbSet<RoleRight> RoleRight { get; set; }
        public DbSet<TableColumn> TableColumn { get; set; }

        public DbSet<Dictionary> Dictionary { get; set; }
        public DbSet<DictionaryData> DictionaryData { get; set; }


        public DbSet<Flow> Flow { get; set; }
        public DbSet<BaseDateFlow> BaseDateFlow { get; set; }

        public DbSet<BaseClass> BaseClass { get; set; }
        public DbSet<BaseClassUser> BaseClassUser { get; set; }
        public DbSet<BaseFlowRole> BaseFlowRole { get; set; }


        public DbSet<Task> Task { get; set; }
        public DbSet<TaskSet> TaskSet { get; set; }
        public DbSet<TaskFlow> TaskFlow { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<TableData> TableData { get; set; }
        public DbSet<TableType> TableType { get; set; }
        public DbSet<TableLine> TableLine { get; set; }
        public DbSet<TableColumnRange> TableColumnRange { get; set; }


        public DbSet<IntelligentDevice> IntelligentDevice { get; set; }
        public DbSet<IntelligentDeviceData> IntelligentDeviceData { get; set; }

        public DbSet<ResourceData> ResourceData { get; set; }
        public DbSet<ResourcePath> ResourcePath { get; set; }

        public DbSet<ProductionLine> ProductionLine { get; set; }

        public DbSet<SpotSelectProblem> SpotSelectProblem { get; set; }
    }

    public class Configuration<T> : DbMigrationsConfiguration<T> where T : DbContext
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; //任何Model Class的修改將會直接更新DB
            AutomaticMigrationDataLossAllowed = true;
        }
        /// <summary>
        /// 初始化基本数据
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(T context)
        {
            base.Seed(context);
            Init(context);
        }


        private void Init(T context)
        {
            User user = context.Set<User>().FirstOrDefault(t => t.UserName == "admin");
            if (user == null)
            {
                context.Set<User>().AddOrUpdate(t => t.UserName, new User()
                {
                    UserName = "admin",
                    UserNumber = "000000",
                    Password = new BaseUtils().BuildPW("admin", "1"),
                    RealName = "系统管理员",
                    Cellphone = "18895353426",
                    CreateDate = DateTime.Now,
                    IsDel = false,
                    UserState = UserState.其他,
                    UserLeaveType = UserLeaveType.后勤人员
                });
                context.SaveChanges();
                user = context.Set<User>().FirstOrDefault(t => t.UserName == "admin");


                context.Set<UserDetails>().AddOrUpdate(t => t.UserID, new UserDetails()
                {
                    UserID = user.ID,
                    Sex = Sex.男,
                    Nationality = Nationality.汉族,
                    Polity = Polity.群众,
                    BirthDate = DateTime.Now,
                    Address = "",
                    Job = "",
                    HireDate = DateTime.Now,
                    ProbationDays = 0,
                    QualifiedDate = DateTime.Now,
                    UserType = UserType.合同制,
                    HeaderUrl = "",
                    CreateDate = DateTime.Now,
                    IsDel = false,
                });

                context.SaveChanges();
            }

            List<Department> DepartmentList = context.Set<Department>().ToList();
            if (DepartmentList == null || DepartmentList.Count == 0)
            {
                Department Department = new Department();
                Department.Name = "综合办公室";
                Department.CreateDate = DateTime.Now;
                Department.CreateUserID = user.ID;
                Department.IsDel = false;
                Department.MasterUserID = user.ID;
                Department.Order = 0;
                Department.ParentID = -1;
                Department.IsAdmin = true;
                DepartmentList.Add(Department);


                Department = new Department();
                Department.Name = "全公司组";
                Department.CreateDate = DateTime.Now;
                Department.CreateUserID = user.ID;
                Department.IsDel = false;
                Department.MasterUserID = user.ID;
                Department.Order = 0;
                Department.ParentID = -1;
                Department.IsAdmin = false;
                DepartmentList.Add(Department);


                Department = new Department();
                Department.Name = "男子组";
                Department.CreateDate = DateTime.Now;
                Department.CreateUserID = user.ID;
                Department.IsDel = false;
                Department.MasterUserID = user.ID;
                Department.Order = 0;
                Department.ParentID = -1;
                Department.IsAdmin = false;
                DepartmentList.Add(Department);

                Department = new Department();
                Department.Name = "女子组";
                Department.CreateDate = DateTime.Now;
                Department.CreateUserID = user.ID;
                Department.IsDel = false;
                Department.MasterUserID = user.ID;
                Department.Order = 0;
                Department.ParentID = -1;
                Department.IsAdmin = false;
                DepartmentList.Add(Department);


                context.Set<Department>().AddOrUpdate(t => t.Name, DepartmentList.ToArray());
                context.SaveChanges();

                DepartmentUser DepartmentUser = new DepartmentUser();
                DepartmentUser.DepartmentID = context.Set<Department>().FirstOrDefault(t => t.Name == "综合办公室").ID;
                DepartmentUser.UserID = user.ID;
                context.Set<DepartmentUser>().Add(DepartmentUser);
                context.SaveChanges();
            }


            List<Dictionary> dictionaryList = context.Set<Dictionary>().ToList();
            if (dictionaryList == null || dictionaryList.Count == 0)
            {
                Dictionary dictionary = new Dictionary();
                dictionary.Name = "身份证号码";
                dictionary.CreateDate = DateTime.Now;
                dictionary.CreateUserID = user.ID;
                dictionary.IsDel = false;
                dictionary.DisplayType = DictionaryTypeEnums.填写;
                dictionary.RegexData = "";
                dictionary.RegexType = RegexType.身份证号码;
                dictionaryList.Add(dictionary);

                dictionary = new Dictionary();
                dictionary.Name = "手机号";
                dictionary.CreateDate = DateTime.Now;
                dictionary.CreateUserID = user.ID;
                dictionary.IsDel = false;
                dictionary.DisplayType = DictionaryTypeEnums.填写;
                dictionary.RegexData = "";
                dictionary.RegexType = RegexType.手机号;
                dictionaryList.Add(dictionary);

                dictionary = new Dictionary();
                dictionary.Name = "邮箱";
                dictionary.CreateDate = DateTime.Now;
                dictionary.CreateUserID = user.ID;
                dictionary.IsDel = false;
                dictionary.DisplayType = DictionaryTypeEnums.填写;
                dictionary.RegexData = "";
                dictionary.RegexType = RegexType.邮箱;
                dictionaryList.Add(dictionary);

                dictionary = new Dictionary();
                dictionary.Name = "邮政编码";
                dictionary.CreateDate = DateTime.Now;
                dictionary.CreateUserID = user.ID;
                dictionary.IsDel = false;
                dictionary.DisplayType = DictionaryTypeEnums.填写;
                dictionary.RegexData = "";
                dictionary.RegexType = RegexType.邮政编码;
                dictionaryList.Add(dictionary);

                dictionary = new Dictionary();
                dictionary.Name = "整数";
                dictionary.CreateDate = DateTime.Now;
                dictionary.CreateUserID = user.ID;
                dictionary.IsDel = false;
                dictionary.DisplayType = DictionaryTypeEnums.填写;
                dictionary.RegexData = "";
                dictionary.RegexType = RegexType.整数;
                dictionaryList.Add(dictionary);

                dictionary = new Dictionary();
                dictionary.Name = "英文字母";
                dictionary.CreateDate = DateTime.Now;
                dictionary.CreateUserID = user.ID;
                dictionary.IsDel = false;
                dictionary.DisplayType = DictionaryTypeEnums.填写;
                dictionary.RegexData = "";
                dictionary.RegexType = RegexType.英文字母;
                dictionaryList.Add(dictionary);

                context.Set<Dictionary>().AddOrUpdate(t => t.Name, dictionaryList.ToArray());
                context.SaveChanges();
            }



            List<Post> PostList = context.Set<Post>().ToList();
            if (PostList == null || PostList.Count == 0)
            {
                Post Post = new Post();
                Post.Name = "公司总经理";
                Post.CreateDate = DateTime.Now;
                Post.CreateUserID = user.ID;
                Post.IsDel = false;
                Post.Order = 0;
                PostList.Add(Post);

                Post = new Post();
                Post.Name = "财务副总经理";
                Post.CreateDate = DateTime.Now;
                Post.CreateUserID = user.ID;
                Post.IsDel = false;
                Post.Order = 0;
                PostList.Add(Post);

                Post = new Post();
                Post.Name = "生产副总经理";
                Post.CreateDate = DateTime.Now;
                Post.CreateUserID = user.ID;
                Post.IsDel = false;
                Post.Order = 0;
                PostList.Add(Post);

                Post = new Post();
                Post.Name = "系统管理员";
                Post.CreateDate = DateTime.Now;
                Post.CreateUserID = user.ID;
                Post.IsDel = false;
                Post.Order = 0;
                PostList.Add(Post);

                context.Set<Post>().AddOrUpdate(t => t.Name, PostList.ToArray());
                context.SaveChanges();

                PostUser PostUser = new PostUser();
                PostUser.PostID = context.Set<Post>().FirstOrDefault(t => t.Name == "系统管理员").ID;
                PostUser.UserID = user.ID;
                context.Set<PostUser>().Add(PostUser);
                context.SaveChanges();
            }



            List<ScoreEventType> ScoreEventTypeList = context.Set<ScoreEventType>().ToList();
            if (ScoreEventTypeList == null || ScoreEventTypeList.Count == 0)
            {
                ScoreEventType ScoreEventType = new ScoreEventType();
                ScoreEventType.Name = "工作类事件";
                ScoreEventType.IsDel = false;
                ScoreEventType.CreateDate = DateTime.Now;
                ScoreEventType.ParentID = 0;
                ScoreEventTypeList.Add(ScoreEventType);

                ScoreEventType = new ScoreEventType();
                ScoreEventType.Name = "行为类事件";
                ScoreEventType.IsDel = false;
                ScoreEventType.CreateDate = DateTime.Now;
                ScoreEventType.ParentID = 0;
                ScoreEventTypeList.Add(ScoreEventType);

                ScoreEventType = new ScoreEventType();
                ScoreEventType.Name = "其他类事件";
                ScoreEventType.IsDel = false;
                ScoreEventType.CreateDate = DateTime.Now;
                ScoreEventType.ParentID = 0;
                ScoreEventTypeList.Add(ScoreEventType);

                context.Set<ScoreEventType>().AddOrUpdate(t => t.Name, ScoreEventTypeList.ToArray());
                context.SaveChanges();
            }


            List<Role> RoleList = context.Set<Role>().ToList();
            if (RoleList == null || RoleList.Count == 0)
            {
                foreach (int item in Enum.GetValues(typeof(RoleEnums)))
                {
                    Role Role = new Role();
                    Role.Name = Enum.GetName(typeof(RoleEnums), item);
                    Role.CreateDate = DateTime.Now;
                    Role.IsBase = true;
                    Role.IsDel = false;
                    RoleList.Add(Role);
                }
                context.Set<Role>().AddOrUpdate(t => t.Name, RoleList.ToArray());
                context.SaveChanges();


                UserRole UserRole = new UserRole();
                UserRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "系统管理员").ID;
                UserRole.UserID = user.ID;
                context.Set<UserRole>().Add(UserRole);
                //context.Set<Role>().AddOrUpdate(t => t.Name, RoleList.ToArray());
                context.SaveChanges();
            }

        }

    }
}
