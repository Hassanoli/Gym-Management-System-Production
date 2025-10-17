using GymManagementDAL.Data.Context;
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
            var builder = WebApplication.CreateBuilder(args);

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
            builder.Services.AddScoped<IUintOfWork , UintOfWork>();

            var app = builder.Build();

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

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
