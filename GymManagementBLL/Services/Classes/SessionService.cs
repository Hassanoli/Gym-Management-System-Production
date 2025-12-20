using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        #region Fields
        private readonly IUintOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public SessionService(IUintOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods

        public OperationResult CreateSession(CreateSessionViewModel createSession)
        {
            var result = new OperationResult { IsSuccess = false };

            if (!IsTrainerExists(createSession.TrainerId))
            {
                result.Message = "Creation Failed: The specified Trainer ID does not exist.";
                return result;
            }

            if (!IsCategoryExists(createSession.CategoryId))
            {
                result.Message = "Creation Failed: The specified Category ID does not exist.";
                return result;
            }

            if (!IsValidDateRange(createSession.StartDate, createSession.EndDate))
            {
                if (createSession.StartDate >= createSession.EndDate)
                {
                    result.Message = "Creation Failed: Start Date must be before End Date.";
                }
                else if (createSession.StartDate < DateTime.Now)
                {
                    result.Message = "Creation Failed: Start Date must be in the future.";
                }
                else
                {
                    result.Message = "Creation Failed: Invalid date range provided.";
                }
                return result;
            }

            try
            {
                var MappedSession =
                    _mapper.Map<CreateSessionViewModel, Session>(createSession);

                _unitOfWork.GetRepository<Session>().Add(MappedSession);

                if (_unitOfWork.SaveChanges() > 0)
                {
                    return new OperationResult
                    {
                        IsSuccess = true,
                        Message = "Session Created successfully."
                    };
                }

                result.Message =
                    "Database Save Error: No changes were recorded or transaction failed.";
                return result;
            }
            catch (Exception ex)
            {
                result.Message =
                    $"Database Error: An unexpected error occurred during session creation. Details: {ex.InnerException?.Message ?? ex.Message}";
                return result;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions =
                _unitOfWork.sessionRepository
                           .GetAllSessionsWithTrainersAndCateogries();

            if (Sessions is null || !Sessions.Any())
                return [];

            return Sessions.Select(X => new SessionViewModel()
            {
                Id = X.Id,
                Capacity = X.Capcity,
                Description = X.Description,
                EndDate = X.EndDate,
                StartDate = X.StartDate,
                TrainerName = X.SessionTrainer.Name,
                CategoryName = X.SessionCategory.CategoryName,
                AvailableSlots =
                    X.Capcity - _unitOfWork.sessionRepository.GetCountOfBookedSlots(X.Id)
            });
        }

        public SessionViewModel? GetSessionById(int id)
        {
            var Session =
                _unitOfWork.sessionRepository
                           .GetAllSessionByIdWithTrainersAndCateogries(id);

            if (Session is null) return null;

            var MappedSessions =
                _mapper.Map<Session, SessionViewModel>(Session);

            MappedSessions.AvailableSlots =
                Session.Capcity -
                _unitOfWork.sessionRepository.GetCountOfBookedSlots(Session.Id);

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
            catch
            {
                return false;
            }
        }

        public OperationResult RemoveSession(int SessionId)
        {
            var Session = _unitOfWork.sessionRepository.GetById(SessionId);
            var result = new OperationResult { IsSuccess = false };

            if (Session is null)
            {
                result.Message = "Session not found.";
                return result;
            }

            if (!IsSessionAvailableForRemoving(Session))
            {
                var HasActiveBooking =
                    _unitOfWork.sessionRepository.GetCountOfBookedSlots(Session.Id) > 0;

                if (HasActiveBooking)
                {
                    result.Message =
                        "Failed: Cannot delete session because it still has active bookings.";
                }
                else if (Session.StartDate <= DateTime.Now &&
                         Session.EndDate > DateTime.Now)
                {
                    result.Message =
                        "Failed: Cannot delete a session that is currently running.";
                }
                else
                {
                    result.Message =
                        "Failed: The session did not meet the required removal conditions.";
                }

                return result;
            }

            try
            {
                _unitOfWork.sessionRepository.Delete(SessionId);

                if (_unitOfWork.SaveChanges() > 0)
                {
                    return new OperationResult
                    {
                        IsSuccess = true,
                        Message = "Session Deleted successfully."
                    };
                }

                result.Message =
                    "Database Save Error: No changes were recorded.";
                return result;
            }
            catch (Exception ex)
            {
                result.Message =
                    $"Database Error: Failed to delete due to a critical database conflict. Details: {ex.InnerException?.Message ?? ex.Message}";
                return result;
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropDown()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetAllTCategoriesForDropDown()
        {
            var Categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Categories);
        }

        #endregion

        #region Helper Methods

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
            return StartDate < EndDate && StartDate >= DateTime.Now;
        }

        private bool IsSessionAvailableForUpdateing(Session session)
        {
            if (session == null) return false;
            if (session.EndDate < DateTime.Now) return false;
            if (session.StartDate <= DateTime.Now) return false;

            var HasActiveBooking =
                _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            return !HasActiveBooking;
        }

        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session == null) return false;

            if (session.StartDate <= DateTime.Now &&
                session.EndDate > DateTime.Now)
                return false;

            var HasActiveBooking =
                _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            return !HasActiveBooking;
        }

        #endregion
    }
}
