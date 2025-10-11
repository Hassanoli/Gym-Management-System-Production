using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    internal class MemberSessionRepository : IMemberSessionRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberSessionRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Add(memberSession);
            return _dbContext.SaveChanges();
        }

        public int Delete(int memberId, int sessionId)
        {
            var record = _dbContext.MemberSessions
                .FirstOrDefault(ms => ms.MemberId == memberId && ms.SessionId == sessionId);

            if (record == null) return 0;

            _dbContext.MemberSessions.Remove(record);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<MemberSession> GetAll() => _dbContext.MemberSessions.ToList();

        public MemberSession? GetByIds(int memberId, int sessionId)
            => _dbContext.MemberSessions.FirstOrDefault(ms => ms.MemberId == memberId && ms.SessionId == sessionId);

        public int Update(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Update(memberSession);
            return _dbContext.SaveChanges();
        }
    }
}
