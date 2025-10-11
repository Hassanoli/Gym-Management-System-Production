using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class MmeberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(X => X.CreatedAt)
                  .HasColumnName("BookingDate")
                  .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => new { x.MemberId, x.SessionId });

            builder.Ignore(X => X.Id);
        }
    }
}
