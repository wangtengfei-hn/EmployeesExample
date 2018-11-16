using Example.MySQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MySQL.Repository.EntityConfigurations
{
    public class MemberConfiguration : EntityTypeConfiguration<Member>
    {
        public MemberConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);

            Property(_ => _._timestamp).IsConcurrencyToken();

            Property(_ => _.PhoneNumber).HasMaxLength(20);
        }
    }
}
