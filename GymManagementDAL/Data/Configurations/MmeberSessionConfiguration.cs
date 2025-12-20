using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class MmeberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        #region Configuration
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(X => X.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => new { x.MemberId, x.SessionId });
            builder.Ignore(X => X.Id);
        }
        #endregion
    }
}
