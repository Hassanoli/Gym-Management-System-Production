using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        #region Configuration
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "SessionCapacityCheck",
                    "Capcity Between 1 and 25"
                );
                tb.HasCheckConstraint(
                    "SessionEndDateCheck",
                    "EndDate > StartDate"
                );
            });

            builder.HasOne(X => X.SessionCategory)
                   .WithMany(X => X.Sessions)
                   .HasForeignKey(X => X.CategoryId);

            builder.HasOne(X => X.SessionTrainer)
                   .WithMany(X => X.TrainerSession)
                   .HasForeignKey(X => X.TrainerId);
        }
        #endregion
    }
}
