using Example.MySQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MySQL.Repository.EntityConfigurations
{
    public class PaymentConfiguration : EntityTypeConfiguration<Payment>
    {
        public PaymentConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);

            Property(_ => _._timestamp).IsConcurrencyToken();
        }
    }
}
