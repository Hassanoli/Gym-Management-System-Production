using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        #region Read

        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int SessionId);
        UpdateSessionViewModel? GetSessionToUpdate(int SessionId);
        IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropDown();
        IEnumerable<CategorySelectViewModel> GetAllTCategoriesForDropDown();

        #endregion

        #region Create

        OperationResult CreateSession(CreateSessionViewModel createSession);

        #endregion

        #region Update

        bool UpdateSession(UpdateSessionViewModel updateSession, int SessionId);

        #endregion

        #region Delete

        OperationResult RemoveSession(int SessionId);

        #endregion
    }
}
