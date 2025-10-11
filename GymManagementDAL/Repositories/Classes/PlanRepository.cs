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
    internal class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Plan plan)
        {
            _dbContext.Plan.Add(plan);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var plan = _dbContext.Plan.Find(id);
            if (plan == null) return 0;

            _dbContext.Plan.Remove(plan);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Plan> GetAll() => _dbContext.Plan.ToList();

        public Plan? GetById(int id) => _dbContext.Plan.Find(id);

        public int Update(Plan plan)
        {
            _dbContext.Plan.Update(plan);
            return _dbContext.SaveChanges();
        }
    }
}
