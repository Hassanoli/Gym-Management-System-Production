using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class Planconfiguration : IEntityTypeConfiguration<Plan>

    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Plan> builder)
        {
            builder.Property(X => X.Name)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

            builder.Property(X => X.Description)
                  .HasColumnType("varchar")
                  .HasMaxLength(200);

            builder.Property(X => X.Price)
                .HasPrecision(10, 2);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("PlanDurationCheck", "DurationDays Between 1 and 365");
            });



        }
    }
}
