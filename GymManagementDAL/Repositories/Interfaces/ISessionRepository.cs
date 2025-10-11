using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface ISessionRepository
    {
        IEnumerable<Session> GetAll();
        Session? GetById(int id);
        int Add(Session session);
        int Update(Session session);
        int Delete(int id);
    }
}
