using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class UintOfWork : IUintOfWork /*, IDisposable*/
    {
        private readonly GymDbContext _dbContext;

        public UintOfWork(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly Dictionary<Type, object> _repositories = new();

        // key => Member , Trainer , session
        // value => GenericRepository<Member>() , GenericRepository<Trainer>() , GenericRepository<session>()
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntitiy, new()
        {
            var entityType = typeof(TEntity);
            if (_repositories.ContainsKey(entityType))
                return (IGenericRepository<TEntity>)_repositories[entityType];

            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[entityType] = NewRepo;
            return NewRepo;


            //return new GenericRepository<TEntity>(_dbContext);

            //GenericRepository<Member>
            //GenericRepository<Member>
            //GenericRepository<Member>
        }

        public int SaveChanges()
        {
           return _dbContext.SaveChanges();
        }

        //public void Dispose()
        //{
        //  _dbContext.Dispose();
        //}
    }
}
