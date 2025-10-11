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
    internal class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
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

            //hassan@gmail.com
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("GymUserValidEmailCheck", "Email LIKE '%@%'");
                tb.HasCheckConstraint("GymUserValidPhoneCheck",
                "Phone LIKE '01%' AND LEN(Phone) = 11 AND Phone NOT LIKE '%[^0-9]%'");
            });

            // unique non Clusterd Index
            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();

            builder.OwnsOne(x => x.Address, AddressBuilder =>
            {
                AddressBuilder.Property(x => x.Street)
                    .HasColumnName("Street")
                    .HasColumnType("varchar(30)")
                    .HasMaxLength(30);

                AddressBuilder.Property(x => x.City)
                    .HasColumnName("City")
                    .HasColumnType("varchar(30)")
                    .HasMaxLength(30);

                AddressBuilder.Property(X => X.BuildingNumber)
                              .HasColumnName("BuildingNumber");
            });






        }
    }
}
