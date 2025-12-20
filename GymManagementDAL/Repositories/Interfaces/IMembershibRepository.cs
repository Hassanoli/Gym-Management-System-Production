using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMembershibRepository : IGenericRepository<MemberShip>
    {
        #region Membership Methods

        IEnumerable<MemberShip> GetAllMembershipWithMemberAndPlans(
            Func<MemberShip, bool>? filter = null);

        MemberShip? GetFirstOrDefault(
            Func<MemberShip, bool>? filter = null);

        #endregion
    }
}
