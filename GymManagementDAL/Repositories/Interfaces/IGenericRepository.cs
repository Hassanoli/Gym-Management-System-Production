using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : BaseEntitiy, new()
    {
        #region Read Methods

        TEntity? GetById(int id);
        IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null);

        #endregion

        #region Write Methods

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entity);

        #endregion
    }
}
