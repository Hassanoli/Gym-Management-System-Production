using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<TEntity>
        : IGenericRepository<TEntity>
        where TEntity : BaseEntitiy, new()
    {
        #region Fields

        private readonly GymDbContext _dbcontext;

        #endregion

        #region Constructor

        public GenericRepository(GymDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        #endregion

        #region CRUD Methods

        public void Add(TEntity entity)
            => _dbcontext.Set<TEntity>().Add(entity);

        public void Delete(int id)
        {
            var entity = _dbcontext.Set<TEntity>().Find(id);
            if (entity != null)
                _dbcontext.Set<TEntity>().Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            if (_dbcontext.Entry(entity).State == EntityState.Detached)
                _dbcontext.Set<TEntity>().Attach(entity);

            _dbcontext.Set<TEntity>().Remove(entity);
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition is null)
                return _dbcontext.Set<TEntity>()
                                 .AsNoTracking()
                                 .ToList();

            return _dbcontext.Set<TEntity>()
                             .AsNoTracking()
                             .Where(condition)
                             .ToList();
        }

        public TEntity? GetById(int id)
            => _dbcontext.Set<TEntity>().Find(id);

        public void Update(TEntity entity)
            => _dbcontext.Set<TEntity>().Update(entity);

        #endregion
    }
}
