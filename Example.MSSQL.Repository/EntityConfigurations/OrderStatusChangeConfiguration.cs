using Example.MSSQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MSSQL.Repository.EntityConfigurations
{
    public class OrderStatusChangeConfiguration : EntityTypeConfiguration<OrderStatusChange>
    {
        public OrderStatusChangeConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);
        }
    }
}
