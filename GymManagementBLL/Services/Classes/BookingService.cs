using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
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
    public class BookingService : IBookingService
    {
        #region Fields
        private readonly IUintOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public BookingService(IUintOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Create Booking
        public OperationResult CreateBooking(CreateBookingViewModel model)
        {
            var session = _unitOfWork.sessionRepository.GetById(model.SessionId);

            if (session is null)
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Session not found."
                };

            if (session.StartDate <= DateTime.UtcNow)
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "You cannot book a session that has already started."
                };

            var membershipRepo = _unitOfWork.membershibRepository;
            var activeMembership = membershipRepo
                .GetFirstOrDefault(m =>
                    m.MemberId == model.MemberId &&
                    m.Status.ToLower() == "active");

            if (activeMembership is null)
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Member does not have an active membership."
                };

            var bookedSlots = _unitOfWork.sessionRepository
                .GetCountOfBookedSlots(model.SessionId);

            if (session.Capcity - bookedSlots <= 0)
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "No available slots for this session."
                };

            var booking = _mapper.Map<MemberSession>(model);
            booking.IsAttended = false;

            _unitOfWork.bookingRepository.Add(booking);
            _unitOfWork.SaveChanges();

            return new OperationResult
            {
                IsSuccess = true,
                Message = "Booking created successfully."
            };
        }


        #endregion

        #region Get Members For Session
        public IEnumerable<MemberForSessionViewModel> GetAllMembersSession(int id)
        {
            var bookingRepo = _unitOfWork.bookingRepository;
            var MemberForSession = bookingRepo.GetSessionsById(id);

            var memberForSessionViewModel =
                _mapper.Map<IEnumerable<MemberForSessionViewModel>>(MemberForSession);

            return memberForSessionViewModel;
        }
        #endregion

        #region Get Sessions With Trainer And Category
        public IEnumerable<SessionViewModel> GetAllSessionsWithTrainerAndCategory()
        {
            var sessionRepo = _unitOfWork.sessionRepository;
            var sessions = sessionRepo.GetAllSessionsWithTrainersAndCateogries();

            var sessionVms = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in sessionVms)
            {
                session.AvailableSlots =
                    session.Capacity - sessionRepo.GetCountOfBookedSlots(session.Id);
            }

            return sessionVms;
        }
        #endregion

        #region Get Members For Dropdown
        public IEnumerable<MemberForSelectListViewModel> GetMemberForDropDown(int id)
        {
            var bookingRepo = _unitOfWork.bookingRepository;

            var bookedMemberIds = bookingRepo
                .GetAll(s => s.Id == id)
                .Select(ms => ms.MemberId)
                .ToList();

            var membersAvailableToBook =
                _unitOfWork.GetRepository<Member>()
                           .GetAll(m => !bookedMemberIds.Contains(m.Id));

            var memberSelectList =
                _mapper.Map<IEnumerable<MemberForSelectListViewModel>>(membersAvailableToBook);

            return memberSelectList;
        }
        #endregion

        public OperationResult ToggleAttendance(int memberId, int sessionId)
        {
            var booking = _unitOfWork.bookingRepository
                .GetAll(b => b.MemberId == memberId && b.SessionId == sessionId)
                .FirstOrDefault();

            if (booking == null)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Booking not found"
                };
            }

         
            _unitOfWork.bookingRepository.Update(booking);

        
            booking.IsAttended = !booking.IsAttended;

            _unitOfWork.SaveChanges();

            return new OperationResult
            {
                IsSuccess = true,
                Message = booking.IsAttended
                    ? "Member marked as attended"
                    : "Attendance removed"
            };
        }


    }
}
