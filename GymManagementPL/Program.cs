using GymManagementBLL;
using GymManagementBLL.Services.Attachment_Service;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region 🔧 Builder Configuration

            var builder = WebApplication.CreateBuilder(args);

            #endregion

            #region 🧩 Services Registration

            // MVC
            builder.Services.AddControllersWithViews();

            // DbContext
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                );
            });

            #region Repositories & Unit Of Work

            builder.Services.AddScoped<IUintOfWork, UintOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IMembershibRepository, MembershibRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            #endregion

            #region AutoMapper

            builder.Services.AddAutoMapper(x =>
                x.AddProfile(new MappingProfiles()));

            #endregion

            #region BLL Services

            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberServices, MemberService>();
            builder.Services.AddScoped<ITrainerServices, TrainerService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IPlanServices, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachment_Service, Attachment_Service>();
            builder.Services.AddScoped<IAcountService, AcountService>();

            #endregion

            #region Identity Configuration

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // Identity Core (as-is)
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<GymDbContext>();

            #endregion

            #endregion

            #region 🚀 Build Application

            var app = builder.Build();

            #endregion

            #region 🌱 Database Migration & Data Seeding

            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();

            GymDbContextDataSeeding.SeedData(dbContext);
            IdentityDbContextSeeding.SeedData(roleManager, userManager);

            #endregion

            #region ⚙️ Middleware Configuration

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            #region 🗺️ Endpoint Mapping

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            #endregion

            #region ▶️ Run Application

            app.Run();

            #endregion
        }
    }
}
