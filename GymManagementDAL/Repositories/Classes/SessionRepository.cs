using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository
        : GenericRepository<Session>, ISessionRepository
    {
        #region Fields

        private readonly GymDbContext _dbContext;

        #endregion

        #region Constructor

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public Methods

        public IEnumerable<Session> GetAllSessionsWithTrainersAndCateogries()
        {
            return _dbContext.Sessions
                             .Include(x => x.SessionTrainer)
                             .Include(x => x.SessionCategory)
                             .ToList();
        }

        public Session? GetAllSessionByIdWithTrainersAndCateogries(int id)
        {
            return _dbContext.Sessions
                             .Include(x => x.SessionTrainer)
                             .Include(x => x.SessionCategory)
                             .FirstOrDefault(x => x.Id == id);
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions
                             .Count(x => x.SessionId == sessionId);
        }

        #endregion
    }
}
