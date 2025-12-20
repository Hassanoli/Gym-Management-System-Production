using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IUintOfWork
    {
        #region Repositories

        ISessionRepository sessionRepository { get; }
        IMembershibRepository membershibRepository { get; }
        IBookingRepository bookingRepository { get; }

        #endregion

        #region Generic Repository Resolver

        IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntitiy, new();

        #endregion

        #region Save Changes

        int SaveChanges();

        #endregion
    }
}
