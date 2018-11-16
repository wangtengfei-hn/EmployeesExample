using Example.MySQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MySQL.Repository.EntityConfigurations
{
    public class AccountRecordConfiguration : EntityTypeConfiguration<AccountRecord>
    {
        public AccountRecordConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);

            Property(_ => _._timestamp).IsConcurrencyToken();

            Property(_ => _.Remark).HasMaxLength(50);

        }
    }
}
