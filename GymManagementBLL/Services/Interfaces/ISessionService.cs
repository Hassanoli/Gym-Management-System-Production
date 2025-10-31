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
        IEnumerable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionById(int SessionId);

        bool CreateSession(CreateSessionViewModel createSession);

        UpdateSessionViewModel? GetSessionToUpdate(int SessionId);
        bool UpdateSession(UpdateSessionViewModel updateSession, int SessionId);
        bool RemoveSession(int SessionId);
        IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropDown();
        IEnumerable<CategorySelectViewModel> GetAllTCategoriesForDropDown();


    }
}
