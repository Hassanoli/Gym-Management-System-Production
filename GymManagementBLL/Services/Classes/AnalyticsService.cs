using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        #region Fields
        private readonly IUintOfWork _uintOfWork;
        #endregion

        #region Constructor
        public AnalyticsService(IUintOfWork uintOfWork)
        {
            _uintOfWork = uintOfWork;
        }
        #endregion

        #region Get Analytics Data
        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = _uintOfWork.sessionRepository.GetAll();

            return new AnalyticsViewModel()
            {
                ActiveMembers = _uintOfWork.GetRepository<MemberShip>().GetAll(X => X.Status == "Active").Count(),
                TotalMembers = _uintOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _uintOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = Sessions.Count(X => X.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(X => X.StartDate <= DateTime.Now && X.EndDate > DateTime.Now),
                CompletedSessions = Sessions.Count(X => X.EndDate < DateTime.Now),
            };
        }
        #endregion
    }
}
