using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        #region Fields

        private readonly ISessionService _sessionService;

        #endregion

        #region Constructor

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #endregion

        #region Actions

        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public ActionResult Create()
        {
            LoadTrainerDropDown();
            LoadCatigoryDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainerDropDown();
                LoadCatigoryDropDown();
                return View(viewModel);
            }

          
            var Result = _sessionService.CreateSession(viewModel);

            if (Result.IsSuccess)
            {
             
                TempData["SuccessMessage"] = Result.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
             
                TempData["ErrorMessage"] = Result.Message;
                LoadTrainerDropDown();
                LoadCatigoryDropDown();
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }
            var sessionToUpdate = _sessionService.GetSessionToUpdate(id);
            if (sessionToUpdate == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainerDropDown();
            return View(sessionToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainerDropDown();
                return View(updateSession);
            }
            var Result = _sessionService.UpdateSession(updateSession, id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Session Updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Updated unsuccessfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }
            var sessionToDelete = _sessionService.GetSessionById(id);
            if (sessionToDelete == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View(sessionToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Now expecting OperationResult
            var Result = _sessionService.RemoveSession(id);

            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = Result.Message;
            }
            else
            {
                // Display the specific failure message from the service
                TempData["ErrorMessage"] = Result.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Helper Methods

        private void LoadTrainerDropDown()
        {
            var Trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadCatigoryDropDown()
        {
            var Categories = _sessionService.GetAllTCategoriesForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        #endregion
    }
}