using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        #region Fields

        private readonly IPlanServices _planServices;

        #endregion

        #region Constructor

        public PlanController(IPlanServices planServices)
        {
            _planServices = planServices;
        }

        #endregion

        #region Index

        public IActionResult Index()
        {
            var plans = _planServices.GetAllPlans();
            return View(plans);
        }

        #endregion

        #region Details

        public ActionResult Details(int id, string viewName = "Details")
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id.";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planServices.GetPlanDetails(id);

            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(viewName, plan);
        }

        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id.";
                return RedirectToAction(nameof(Index));
            }

            var planToUpdate = _planServices.GetPlanToUpdate(id);

            if (planToUpdate == null)
            {
                TempData["ErrorMessage"] = "Plan not found or cannot be updated.";
                return RedirectToAction(nameof(Index));
            }

            return View(planToUpdate);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatePlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(updatePlan);
            }

            var result = _planServices.UpdatePlan(id, updatePlan);

            if (result)
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Plan.";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Activate / Deactivate

        public ActionResult Activate(int id)
        {
            var result = _planServices.ToggleStatus(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Plan Status Updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Plan status.";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
