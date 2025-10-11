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
    internal class TrainerConfiguration : GymUserConfigurations<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        //public void Configure(EntityTypeBuilder<Trainer> builder)
        //{
        //    throw new NotImplementedException();
        //}

        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(x => x.CreatedAt)
                  .HasColumnName("HireDate")
                  .HasDefaultValueSql("GETDATE()");
          

          base.Configure(builder);
        }


    }
}
