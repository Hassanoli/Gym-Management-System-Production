using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class MembershibRepository
        : GenericRepository<MemberShip>, IMembershibRepository
    {
        #region Fields

        private readonly GymDbContext _context;

        #endregion

        #region Constructor

        public MembershibRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        public IEnumerable<MemberShip> GetAllMembershipWithMemberAndPlans(
            Func<MemberShip, bool>? filter = null)
        {
            return _context.MemberShips
                           .Include(m => m.Member)
                           .Include(m => m.Plan)
                           .Where(filter ?? (_ => true));
        }

        public MemberShip? GetFirstOrDefault(
            Func<MemberShip, bool>? filter = null)
        {
            return _context.MemberShips
                           .FirstOrDefault(filter ?? (_ => true));
        }

        #endregion
    }
}
