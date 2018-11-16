using Example.MSSQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MSSQL.Repository.EntityConfigurations
{
    public class PaymentConfiguration : EntityTypeConfiguration<Payment>
    {
        public PaymentConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);
        }
    }
}
