using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
        #region Fields
        private readonly IMembershipService _membershipService;
        #endregion

        #region Constructor
        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            var memberships = _membershipService.GetAllMemberships();
            return View(memberships);
        }
        #endregion

        #region Create Actions
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateMembershipViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _membershipService.CreateMembership(model);
                if (result)
                {
                    TempData["SuccessMessage"] = "Membership created successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Membership cannot be created";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["ErrorMessage"] = "Membership cannot be created, check your data";
            LoadDropdowns();
            return View(model);
        }
        #endregion

        #region Delete Actions
        public IActionResult Cancel(int id)
        {
            var result = _membershipService.DeleteMembership(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Membership cannot be deleted";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helper Methods
        private void LoadDropdowns()
        {
            var members = _membershipService.GetMemberForDropDown();
            var plans = _membershipService.GetPLanForDropdown();

            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
        #endregion
    }
}
