using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using FeiLongLibrary.Utils;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.DBContext
{
    public class FLDbContext : DbContext
    {
        public FLDbContext()
            : base(ParaUtils.GetSql)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FLDbContext, Configuration<FLDbContext>>());
        }
        public DbSet<BaseDateFlow> BaseDateFlow { get; set; }
        public DbSet<BaseFlowRole> BaseFlowRole { get; set; }
        public DbSet<BaseClass> BaseClass { get; set; }
        public DbSet<BaseClassUser> BaseClassUser { get; set; }

        public DbSet<SysUser> SysUser { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Role> Role { get; set; }

        public DbSet<FLClass> FLClass { get; set; }
        public DbSet<ClassTask> ClassTask { get; set; }
        public DbSet<TaskFlow> TaskFlow { get; set; }
        public DbSet<Flow> Flow { get; set; }
        public DbSet<FLTask> FLTask { get; set; }

        public DbSet<Table> Table { get; set; }
        public DbSet<TableColumn> TableColumn { get; set; }
        public DbSet<TableData> TableData { get; set; }

        public DbSet<SysLog> SysLog { get; set; }

        public DbSet<Dictionary> Dictionary { get; set; }
        public DbSet<DictionaryData> DictionaryData { get; set; }

        public DbSet<Authorization> Authorization { get; set; }
        public DbSet<AuthorizationRole> AuthorizationRole { get; set; }
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
            SysUser user = context.Set<SysUser>().FirstOrDefault(t => t.UserName == "admin");
            if (user == null)
            {
                context.Set<SysUser>().AddOrUpdate(t => t.UserName, new SysUser()
                {
                    UserName = "admin",
                    Password = new BaseUtils().BuildPW("admin", "1"),
                    RealName = "管理员",
                    TelNumber = "18895353426",
                    CreateDate = DateTime.Now,
                    IsAccountDisabled = false,
                });
                context.SaveChanges();
                user = context.Set<SysUser>().FirstOrDefault(t => t.UserName == "admin");
            }

            //List<Flow> FlowList = context.Set<Flow>().ToList();
            //if (FlowList == null || FlowList.Count == 0)
            //{
            //    Flow flow = new Flow();
            //    flow.TableType = TableTypeEnums.切削液浓度点检表;
            //    flow.Name = "填写切削液浓度点检表";
            //    flow.ParentID = 0;

            //    context.Set<Flow>().AddOrUpdate(t => t.Name, FlowList.ToArray());
            //    context.SaveChanges();
            //}

            List<Role> RoleList = context.Set<Role>().ToList();
            if (RoleList == null || RoleList.Count == 0)
            {
                Role Role = new Role();
                Role.Name = "操作工";
                Role.CreateDate = DateTime.Now;
                Role.IsDel = false;
                RoleList.Add(Role);

                Role = new Role();
                Role.Name = "班长";
                Role.CreateDate = DateTime.Now;
                Role.IsDel = false;
                RoleList.Add(Role);

                Role = new Role();
                Role.Name = "车间主任";
                Role.CreateDate = DateTime.Now;
                Role.IsDel = false;
                RoleList.Add(Role);

                Role = new Role();
                Role.Name = "生产设备部长";
                Role.CreateDate = DateTime.Now;
                Role.IsDel = false;
                RoleList.Add(Role);

                Role = new Role();
                Role.Name = "工装管理员";
                Role.CreateDate = DateTime.Now;
                Role.IsDel = false;
                RoleList.Add(Role);

                Role = new Role();
                Role.Name = "设备修理工";
                Role.CreateDate = DateTime.Now;
                Role.IsDel = false;
                RoleList.Add(Role);


                Role = new Role();
                Role.Name = "系统管理员";
                Role.CreateDate = DateTime.Now;
                Role.IsDel = false;
                RoleList.Add(Role);

                context.Set<Role>().AddOrUpdate(t => t.Name, RoleList.ToArray());
                context.SaveChanges();
            }


            List<Authorization> AuthorizationList = context.Set<Authorization>().ToList();
            if (AuthorizationList == null || AuthorizationList.Count == 0)
            {
                Authorization Authorization = new Authorization();
                Authorization.CreateDate = DateTime.Now;
                Authorization.CreateUserID = user.ID;
                Authorization.Name = "SysUser";
                //Authorization.RoleList = context.Set<Role>().Where(t => t.Name == "系统管理员").ToList();
                Authorization.ShowName = "用户管理";
                Authorization.Description = "管理用户信息，可添加删除修改用户。";
                //Authorization.UserList = new List<SysUser>();
                AuthorizationList.Add(Authorization);

                Authorization = new Authorization();
                Authorization.CreateDate = DateTime.Now;
                Authorization.CreateUserID = user.ID;
                Authorization.Name = "Role";
                //Authorization.RoleList = context.Set<Role>().Where(t => t.Name == "系统管理员").ToList();
                Authorization.ShowName = "角色管理";
                Authorization.Description = "管理角色信息，可添加删除修改角色。";
                //Authorization.UserList = new List<SysUser>();
                AuthorizationList.Add(Authorization);

                Authorization = new Authorization();
                Authorization.CreateDate = DateTime.Now;
                Authorization.CreateUserID = user.ID;
                Authorization.Name = "BaseData";
                //Authorization.RoleList = context.Set<Role>().Where(t => t.Name == "系统管理员" || t.Name == "生产设备部部长" || t.Name == "车间主任").ToList();
                Authorization.ShowName = "基础信息设置";
                Authorization.Description = "可设置基础信息，包括基础班次，基础时间，字典数据和表单基础信息。";
                //Authorization.UserList = new List<SysUser>();
                AuthorizationList.Add(Authorization);

                Authorization = new Authorization();
                Authorization.CreateDate = DateTime.Now;
                Authorization.CreateUserID = user.ID;
                Authorization.Name = "Authorization";
                //Authorization.RoleList = context.Set<Role>().Where(t => t.Name == "系统管理员").ToList();
                Authorization.ShowName = "权限管理";
                Authorization.Description = "可设置权限对应的角色，也可以给单独的用户添加权限。";
                //Authorization.UserList = new List<SysUser>();
                AuthorizationList.Add(Authorization);

                Authorization = new Authorization();
                Authorization.CreateDate = DateTime.Now;
                Authorization.CreateUserID = user.ID;
                Authorization.Name = "Class";
                //Authorization.RoleList = context.Set<Role>().Where(t => t.Name == "系统管理员" || t.Name == "生产设备部部长" || t.Name == "车间主任" || t.Name == "班长").ToList();
                Authorization.ShowName = "班次管理";
                Authorization.Description = "可添加班次。";
                //Authorization.UserList = new List<SysUser>();
                AuthorizationList.Add(Authorization);

                context.Set<Authorization>().AddOrUpdate(t => t.Name, AuthorizationList.ToArray());
                context.SaveChanges();
            }


            List<AuthorizationRole> AuthorizationRoleList = context.Set<AuthorizationRole>().ToList();
            if (AuthorizationRoleList == null || AuthorizationRoleList.Count == 0)
            {

                AuthorizationRole AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "班次管理").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "系统管理员").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "班次管理").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "生产设备部部长").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "班次管理").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "车间主任").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "班次管理").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "班长").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "用户管理").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "系统管理员").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "角色管理").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "系统管理员").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "基础信息设置").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "系统管理员").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "基础信息设置").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "生产设备部部长").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "基础信息设置").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "车间主任").ID;
                AuthorizationRoleList.Add(AuthorizationRole);

                AuthorizationRole = new AuthorizationRole();
                AuthorizationRole.AuthorizationID = context.Set<Authorization>().FirstOrDefault(t => t.ShowName == "权限管理").ID;
                AuthorizationRole.RoleID = context.Set<Role>().FirstOrDefault(t => t.Name == "系统管理员").ID;
                AuthorizationRoleList.Add(AuthorizationRole);


                context.Set<AuthorizationRole>().AddOrUpdate(t => t.AuthorizationID, AuthorizationRoleList.ToArray());
                context.SaveChanges();
            }

        }

    }
}
