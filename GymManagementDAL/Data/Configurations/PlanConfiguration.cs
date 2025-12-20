using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class Planconfiguration : IEntityTypeConfiguration<Plan>
    {
        #region Configuration
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(X => X.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(X => X.Description)
                   .HasColumnType("varchar")
                   .HasMaxLength(200);

            builder.Property(X => X.Price)
                   .HasPrecision(10, 2);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "PlanDurationCheck",
                    "DurationDays Between 1 and 365"
                );
            });
        }
        #endregion
    }
}
