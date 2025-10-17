 using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainersAndCateogries();

        Session? GetAllSessionByIdWithTrainersAndCateogries(int id);

        int GetCountOfBookedSlots(int sessionId);
    }
}
