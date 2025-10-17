using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUintOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUintOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (!IsTrainerExists(createSession.TrainerId)) return false;
                if (!IsCategoryExists(createSession.CategoryId)) return false;
                if (!IsValidDateRange(createSession.StartDate, createSession.EndDate)) return false;
                // CreateSessionViewModel - Session

                var MappedSession = _mapper.Map<CreateSessionViewModel, Session>(createSession);
                _unitOfWork.GetRepository<Session>().Add(MappedSession);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.sessionRepository.GetAllSessionsWithTrainersAndCateogries();
            if (Sessions is null || !Sessions.Any()) return [];

            #region Manual Mapping
            //return Sessions.Select(X => new SessionViewModel()
            //{
            //    Id = X.Id,
            //    Capacity = X.Capcity,
            //    Description = X.Description,
            //    EndDate = X.EndDate,
            //    StartDate = X.StartDate,
            //    TrainerName = X.SessionTrainer.Name,
            //    CategoryName = X.SessionCategory.CategoryName,
            //    AvailableSlots = X.Capcity - _unitOfWork.sessionRepository.GetCountOfBookedSlots(X.Id)

            //}); 
            #endregion

            #region Automatic Mapping
            var MappedSessions = _mapper.Map<IEnumerable<Session> , IEnumerable<SessionViewModel>>(Sessions);
            return MappedSessions;
            #endregion
        }

        public SessionViewModel? GetSessionById(int id)
        {
            var Session = _unitOfWork.sessionRepository.GetAllSessionByIdWithTrainersAndCateogries(id);
            if (Session is null) return null;

           var MappedSessions = _mapper.Map<Session, SessionViewModel>(Session);
            return MappedSessions;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int SessionId)
        {
            var Session = _unitOfWork.sessionRepository.GetById(SessionId);
            if (!IsSessionAvailableForUpdateing(Session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updateSession, int SessionId)
        {

            try
            {
                var Session = _unitOfWork.sessionRepository.GetById(SessionId);
                if (!IsSessionAvailableForUpdateing(Session!)) return false;
                if (!IsTrainerExists(updateSession.TrainerId)) return false;
                if (!IsValidDateRange(updateSession.StartDate, updateSession.EndDate)) return false;

                _mapper.Map(updateSession, Session);
                Session!.UpdatedAt = DateTime.Now;
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool RemoveSession(int SessionId)
        {
            var Session = _unitOfWork.sessionRepository.GetById(SessionId);

            if(!IsSessionAvailableForRemoving(Session!)) return false;

            _unitOfWork.sessionRepository.Delete(SessionId!);
            return _unitOfWork.SaveChanges() > 0;

        }

        #region HelperMethods
        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }

        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsValidDateRange(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < EndDate && StartDate > DateTime.Now;
        }

        private bool IsSessionAvailableForUpdateing(Session session)
        {
            if (session == null) return false;

            if (session.EndDate < DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now) return false;

            var HasActiveBooking = _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBooking) return false;

            return true;

        }
        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session == null) return false;

            if (session.StartDate > DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            var HasActiveBooking = _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBooking) return false;

            return true;

        }

        #endregion
    }
}
