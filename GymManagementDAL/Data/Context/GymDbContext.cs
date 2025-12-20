using GymManagementDAL.Data.Configurations;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagementDAL.Data.Context
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Constructor

        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        #endregion

        #region On Model Creating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
            );

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FirstName)
                      .HasColumnType("varchar")
                      .HasMaxLength(50);

                entity.Property(u => u.LastName)
                      .HasColumnType("varchar")
                      .HasMaxLength(50);
            });
        }

        #endregion

        #region DbSets

        #region Already Inherited From IdentityDbContext
        // public DbSet<ApplicationUser> Users { get; set; }
        // public DbSet<IdentityRole> Roles { get; set; }
        // public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        #endregion

        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }

        #endregion
    }
}
