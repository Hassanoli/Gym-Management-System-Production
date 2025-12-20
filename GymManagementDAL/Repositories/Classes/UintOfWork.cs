using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
    public class UintOfWork : IUintOfWork
    {
        #region Fields

        private readonly GymDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        #endregion

        #region Constructor

        public UintOfWork(
            GymDbContext dbContext,
            ISessionRepository sessionRepository,
            IMembershibRepository membershibRepository,
            IBookingRepository bookingRepository)
        {
            _dbContext = dbContext;
            this.sessionRepository = sessionRepository;
            this.membershibRepository = membershibRepository;
            this.bookingRepository = bookingRepository;
        }

        #endregion

        #region Repositories

        public ISessionRepository sessionRepository { get; }
        public IMembershibRepository membershibRepository { get; }
        public IBookingRepository bookingRepository { get; }

        #endregion

        #region Generic Repository Resolver

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntitiy, new()
        {
            var entityType = typeof(TEntity);

            if (_repositories.ContainsKey(entityType))
                return (IGenericRepository<TEntity>)_repositories[entityType];

            var newRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[entityType] = newRepo;

            return newRepo;
        }

        #endregion

        #region Save Changes

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        #endregion
    }
}
