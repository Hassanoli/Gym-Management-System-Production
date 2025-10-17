using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntitiy, new()
    {
        private readonly GymDbContext _dbcontext;
        public GenericRepository(GymDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public void Add(TEntity entity) => _dbcontext.Set<TEntity>().Add(entity);
  

        public void Delete(int id) => _dbcontext.Set<TEntity>().Remove(new TEntity() { Id = id });

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if(condition is null)
                return _dbcontext.Set<TEntity>().AsNoTracking().ToList();
            else
                return _dbcontext.Set<TEntity>().AsNoTracking().Where(condition).ToList();

        }
        

        public TEntity? GetById(int id) => _dbcontext.Set<TEntity>().Find(id); 

        public void Update(TEntity entity) => _dbcontext.Set<TEntity>().Update(entity);


    }
}
