using GymManagementDAL.Entities;
using System.Collections.Generic;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        #region Session Methods

        IEnumerable<Session> GetAllSessionsWithTrainersAndCateogries();

        Session? GetAllSessionByIdWithTrainersAndCateogries(int id);

        int GetCountOfBookedSlots(int sessionId);

        #endregion
    }
}
