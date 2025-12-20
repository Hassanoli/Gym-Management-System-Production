using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingService
    {
        #region Read

        IEnumerable<SessionViewModel> GetAllSessionsWithTrainerAndCategory();

        IEnumerable<MemberForSessionViewModel> GetAllMembersSession(int id);

        IEnumerable<MemberForSelectListViewModel> GetMemberForDropDown(int id);

        #endregion

        #region Create

        //bool CreateBooking(CreateBookingViewModel model);

        OperationResult CreateBooking(CreateBookingViewModel model);


        #endregion
    }
}
