using Example.MSSQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MSSQL.Repository.EntityConfigurations
{
    public class RechargeConfiguration : EntityTypeConfiguration<Recharge>
    {
        public RechargeConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);
        }
    }
}
