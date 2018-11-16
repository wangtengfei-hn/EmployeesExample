using Example.MySQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MySQL.Repository.EntityConfigurations
{
    public class ThirdpayConfiguration : EntityTypeConfiguration<Thirdpay>
    {
        public ThirdpayConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);

            Property(_ => _._timestamp).IsConcurrencyToken();
        }
    }
}
