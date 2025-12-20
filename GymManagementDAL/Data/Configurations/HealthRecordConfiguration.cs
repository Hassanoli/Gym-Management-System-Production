using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        #region Configuration
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members");
            builder.HasKey(x => x.Id);

            builder.HasOne<Member>()
                   .WithOne(x => x.HealthRecord)
                   .HasForeignKey<HealthRecord>(x => x.Id);

            builder.Ignore(x => x.CreatedAt);
            builder.Ignore(x => x.UpdatedAt);
        }
        #endregion
    }
}
