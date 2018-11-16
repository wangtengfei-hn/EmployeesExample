using Example.MSSQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MSSQL.Repository.EntityConfigurations
{
    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);

            Property(_ => _.Balance).HasPrecision(18, 4);
        }
    }
}
