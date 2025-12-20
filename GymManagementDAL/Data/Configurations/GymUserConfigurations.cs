using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class GymUserConfigurations<T> : IEntityTypeConfiguration<T>
        where T : GymUser
    {
        #region Configuration
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(X => X.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.Property(X => X.Phone)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("GymUserValidEmailCheck", "Email LIKE '%@%'");
                tb.HasCheckConstraint(
                    "GymUserValidPhoneCheck",
                    "Phone LIKE '01%' AND LEN(Phone) = 11 AND Phone NOT LIKE '%[^0-9]%'"
                );
            });

            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();

            builder.OwnsOne(x => x.Address, addressBuilder =>
            {
                addressBuilder.Property(x => x.Street)
                    .HasColumnName("Street")
                    .HasColumnType("varchar(30)")
                    .HasMaxLength(30);

                addressBuilder.Property(x => x.City)
                    .HasColumnName("City")
                    .HasColumnType("varchar(30)")
                    .HasMaxLength(30);

                addressBuilder.Property(X => X.BuildingNumber)
                    .HasColumnName("BuildingNumber");
            });
        }
        #endregion
    }
}
