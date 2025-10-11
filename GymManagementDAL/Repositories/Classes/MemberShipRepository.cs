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
    internal class MemberShipRepository : IMemberShipRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberShipRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(MemberShip memberShip)
        {
            _dbContext.MemberShips.Add(memberShip);
            return _dbContext.SaveChanges();
        }

        public int Delete(int memberId, int planId)
        {
            var membership = _dbContext.MemberShips
                .FirstOrDefault(ms => ms.MemberId == memberId && ms.PlanId == planId);

            if (membership == null) return 0;

            _dbContext.MemberShips.Remove(membership);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<MemberShip> GetAll() => _dbContext.MemberShips.ToList();

        public MemberShip? GetByIds(int memberId, int planId)
            => _dbContext.MemberShips.FirstOrDefault(ms => ms.MemberId == memberId && ms.PlanId == planId);

        public int Update(MemberShip memberShip)
        {
            _dbContext.MemberShips.Update(memberShip);
            return _dbContext.SaveChanges();
        }
    }
}
