using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly GymDbContext _dbContext;

        public CategoryRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Category category)
        {
            _dbContext.Categories.Add(category);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null) return 0;

            _dbContext.Categories.Remove(category);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Category> GetAll() => _dbContext.Categories.ToList();

        public Category? GetById(int id) => _dbContext.Categories.Find(id);

        public int Update(Category category)
        {
            _dbContext.Categories.Update(category);
            return _dbContext.SaveChanges();
        }
    }
}
