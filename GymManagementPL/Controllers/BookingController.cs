using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class BookingController : Controller
    {
        #region Fields

        private readonly IBookingService _bookingService;
        private readonly IUintOfWork _uintOfWork;

        #endregion

        #region Constructor

        public BookingController(IBookingService bookingService , IUintOfWork uintOfWork)
        {
            _bookingService = bookingService;
            _uintOfWork = uintOfWork;
        }

        #endregion

        #region Index

        public IActionResult Index()
        {
            var sessions = _bookingService.GetAllSessionsWithTrainerAndCategory();
            return View(sessions);
        }

        #endregion

        #region Get Members

        public IActionResult GetMembersForUpcomingSession(int id)
        {
            var members = _bookingService.GetAllMembersSession(id);
            return View(members);
        }

        public IActionResult GetMembersForOngoingSession(int id)
        {
            var members = _bookingService.GetAllMembersSession(id);
            return View(members);
        }

        #endregion

        #region Create Booking

        public IActionResult Create(int id)
        {
            var members = _bookingService.GetMemberForDropDown(id);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Create(CreateBookingViewModel model)
        {
            var result = _bookingService.CreateBooking(model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(
                nameof(GetMembersForUpcomingSession),
                new { id = model.SessionId }
            );
        }


        #endregion

        #region Cancel
        [HttpPost]
        public IActionResult Cancel(int MemberId, int SessionId)
        {
            var bookingRepo = _uintOfWork.bookingRepository;

            var booking = bookingRepo
                .GetAll(b => b.MemberId == MemberId && b.SessionId == SessionId)
                .FirstOrDefault();

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found";
                return RedirectToAction("Index");
            }

            bookingRepo.Delete(booking);
            _uintOfWork.SaveChanges();

            TempData["SuccessMessage"] = "Booking cancelled successfully";

            return RedirectToAction(
                nameof(GetMembersForUpcomingSession),
                new { id = SessionId }
            );
        }


        #endregion

        #region Mark As Attend
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleAttendance(int memberId, int sessionId)
        {
            var result = _bookingService.ToggleAttendance(memberId, sessionId);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = result.Message;
            else
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction(
                nameof(GetMembersForOngoingSession),
                new { id = sessionId }
            );
        } 
        #endregion

    }
}
