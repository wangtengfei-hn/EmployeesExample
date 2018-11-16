using Example.MSSQL.Repository.AggregatesModel;
using Example.MSSQL.Repository.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Example.MSSQL.Repository
{
    public class MSSQLDbContext : DbContext, IMSSQLDbContext
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(typeof(MSSQLDbContext));

        public MSSQLDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            base.Database.CommandTimeout = 1800;

        }

        Database IMSSQLDbContext.Database => base.Database;

        #region DbSet

        public DbSet<Member> Member { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<AccountRecord> AccountRecord { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderStatusChange> OrderStatusChange { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Thirdpay> Thirdpay { get; set; }
        public DbSet<Recharge> Recharge { get; set; }

        #endregion

        #region Fluent API配置

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //移除EF映射表名复数
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Configurations.Add(new MemberConfiguration());
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        #endregion

        public bool Save()
        {
            try
            {
                this.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Db-Save-Error", ex);
                return false;
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                await this.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Db-Save-Error", ex);
                return false;
            }
        }

        public bool Save(out Exception exception)
        {
            exception = null;

            try
            {
                this.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                exception = e;
                return false;
            }
        }

    }
}
