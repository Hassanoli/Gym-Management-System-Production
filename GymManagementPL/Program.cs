using GymManagementBLL;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
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

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                //options.UseSqlServer("Server=.;Database=GymManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);

                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            /*********************************
            //builder.Services.AddScoped<GenericRepository<Member>, GenericRepository<Member>>();
            //builder.Services.AddScoped<GenericRepository<Trainer>, GenericRepository<Trainer>>();
            //builder.Services.AddScoped<GenericRepository<Plan>, GenericRepository<Plan>>();
            *********************************************/

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<IUintOfWork, UintOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberServices, MemberService>();
            builder.Services.AddScoped<ITrainerServices, TrainerService>();
            builder.Services.AddScoped<IPlanServices, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();


            #endregion

            #region 🚀 App Build
            var app = builder.Build();

            #region Seed Data -Migrate Database
            using var Scoped = app.Services.CreateScope();
            var dbContext = Scoped.ServiceProvider.GetRequiredService<GymDbContext>();
            var PendingMigartions = dbContext.Database.GetPendingMigrations();
            if (PendingMigartions?.Any() ?? false)
                dbContext.Database.Migrate();
            GymDbContextDataSeeding.SeedData(dbContext);

            #endregion
            #endregion

            #region ⚙️ Middleware Configuration

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            #endregion

            #region 🗺️ Endpoint Mapping
            //app.MapStaticAssets();
            //app.MapControllerRoute(
            //    name: "Trainers",
            //    pattern: "coach/{action}",
            //    defaults: new { controller = "Trainer"  , action = "Index"})
            //    .WithStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            #endregion

            #region ▶️ Run Application
            app.UseStaticFiles();
            app.Run();
            #endregion
        }
    }
}
