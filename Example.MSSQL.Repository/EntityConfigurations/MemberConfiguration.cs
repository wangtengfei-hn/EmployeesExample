using Example.MSSQL.Repository.AggregatesModel;
using System.Data.Entity.ModelConfiguration;

namespace Example.MSSQL.Repository.EntityConfigurations
{
    public class MemberConfiguration : EntityTypeConfiguration<Member>
    {
        public MemberConfiguration()
        {
            HasKey(_ => _.Id);

            HasIndex(_ => _.CreateTime);

            Property(_ => _.PhoneNumber).HasMaxLength(20);
        }
    }
}
