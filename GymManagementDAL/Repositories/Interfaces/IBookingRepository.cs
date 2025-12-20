using GymManagementDAL.Entities;
using System.Collections.Generic;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<MemberSession>
    {
        #region Booking Methods

        IEnumerable<MemberSession> GetSessionsById(int sessionId);

        #endregion
    }
}
