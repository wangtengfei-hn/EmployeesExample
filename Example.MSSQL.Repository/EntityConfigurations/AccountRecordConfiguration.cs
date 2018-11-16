using Example.MSSQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MSSQL.Repository.EntityConfigurations
{
    public class AccountRecordConfiguration : EntityTypeConfiguration<AccountRecord>
    {
        public AccountRecordConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);

            Property(_ => _.Remark).HasMaxLength(50);

        }
    }
}
