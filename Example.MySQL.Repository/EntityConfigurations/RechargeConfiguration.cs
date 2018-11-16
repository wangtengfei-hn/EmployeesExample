using Example.MySQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MySQL.Repository.EntityConfigurations
{
    public class RechargeConfiguration : EntityTypeConfiguration<Recharge>
    {
        public RechargeConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);

            Property(_ => _._timestamp).IsConcurrencyToken();
        }
    }
}
