using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanServices _planServices;

        public PlanController(IPlanServices planServices)
        {
            _planServices = planServices;
        }
        public IActionResult Index()
        {
            var Plans = _planServices.GetAllPlans();
            return View(Plans);
        }
        public ActionResult Details(int id , string ViewName = "Details")
        {
            if(id <= 0)
            {
               TempData["ErrorMessage"] = "Invalid Plan Id.";
                return RedirectToAction(nameof(Index));
            }
             var Plan = _planServices.GetPlanDetails(id);

            if(Plan == null)
                {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
                }
            return View(ViewName,Plan);
        }
        public ActionResult Edit (int id)
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
        public ActionResult Edit([FromRoute] int id , UpdatePlanViewModel updatePlan)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(updatePlan);
            }
            var Result = _planServices.UpdatePlan(id, updatePlan);  
            if(Result)
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Plan.";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Activate(int id)
        {
            var Result = _planServices.ToggleStatus(id); 
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Status Updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Plan status.";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
