using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextDataSeeding
    {
        #region Public Method : SeedData
        public static bool SeedData(GymDbContext dbcontext)
        {
            try
            {
                var HasPlans = dbcontext.Plan.Any();
                var HasCategories = dbcontext.Categories.Any();

                if (HasPlans && HasCategories) return false;
                if (!HasPlans)
                {
                    var Plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (Plans.Any())
                        dbcontext.Plan.AddRange(Plans);
                }

                if (!HasCategories)
                {
                    var Categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (Categories.Any())
                        dbcontext.Categories.AddRange(Categories);
                }
                return dbcontext.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seesing Failed {ex}");
                return false;
            }
        }
        #endregion

        #region Private Method : LoadDataFromJsonFile
        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {
            //D:\Projects\Route\GymManagementSystemSolution\GymManagementPL\wwwroot\Files\plans.json
            //D:\Projects\Route\GymManagementSystemSolution\GymManagementPL\wwwroot\Files\categories.json
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);

            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
        #endregion
    }
}
