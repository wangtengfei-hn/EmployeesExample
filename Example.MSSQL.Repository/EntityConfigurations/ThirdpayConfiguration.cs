using Example.MSSQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MSSQL.Repository.EntityConfigurations
{
    public class ThirdpayConfiguration : EntityTypeConfiguration<Thirdpay>
    {
        public ThirdpayConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);
        }
    }
}
