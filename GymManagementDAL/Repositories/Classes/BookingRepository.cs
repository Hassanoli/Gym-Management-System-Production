using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class BookingRepository
        : GenericRepository<MemberSession>, IBookingRepository
    {
        #region Fields

        private readonly GymDbContext _context;

        #endregion

        #region Constructor

        public BookingRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        public IEnumerable<MemberSession> GetSessionsById(int sessionId)
        {
            return _context.MemberSessions
                           .Where(ms => ms.SessionId == sessionId)
                           .Include(ms => ms.Member)
                           .ToList();
        }

        #endregion
    }
}
