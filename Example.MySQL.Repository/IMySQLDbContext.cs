using Example.MySQL.Repository.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.MySQL.Repository
{
    public interface IMySQLDbContext
    {
        Database Database { get; }

        #region DbSet

        DbSet<Member> Member { get; set; }
        DbSet<Account> Account { get; set; }
        DbSet<AccountRecord> AccountRecord { get; set; }
        DbSet<Order> Order { get; set; }
        DbSet<OrderStatusChange> OrderStatusChange { get; set; }
        DbSet<Payment> Payment { get; set; }
        DbSet<Thirdpay> Thirdpay { get; set; }
        DbSet<Recharge> Recharge { get; set; }

        #endregion


        bool Save();

        bool Save(out Exception exception);
    }
}
