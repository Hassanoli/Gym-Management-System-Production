using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDbContextSeeding
    {
        #region Public Methods

        public static bool SeedData(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            try
            {
                var HasUsers = userManager.Users.Any();
                var HasRoles = userManager.Users.Any();

                if (HasUsers && HasRoles)
                    return false;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>
                    {
                        new() { Name = "SuperAdmin" },
                        new() { Name = "Admin" }
                    };

                    foreach (var role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
                }

                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser
                    {
                        FirstName = "Alhassan",
                        LastName = "Mohamed",
                        UserName = "Alhassan.Mohamed",
                        Email = "hassanmohamedali0113@gmail.com",
                        PhoneNumber = "01550122173",
                    };

                    userManager.CreateAsync(MainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser
                    {
                        FirstName = "ahmed",
                        LastName = "rahmo",
                        UserName = "ahmed.rahmo",
                        Email = "ahmedrahmo@gmail.com",
                        PhoneNumber = "01126989009",
                    };

                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Identity Seeding Failed: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
