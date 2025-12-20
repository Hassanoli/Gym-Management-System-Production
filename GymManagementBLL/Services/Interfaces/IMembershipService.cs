using GymManagementBLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        #region Read

        IEnumerable<MembershipViewModel> GetAllMemberships();
        IEnumerable<PlanForSelectListViewModel> GetPLanForDropdown();
        IEnumerable<MemberForSelectListViewModel> GetMemberForDropDown();

        #endregion

        #region Create

        bool CreateMembership(CreateMembershipViewModel model);

        #endregion

        #region Delete

        bool DeleteMembership(int memberId);

        #endregion
    }
}
