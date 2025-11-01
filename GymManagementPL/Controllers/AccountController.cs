using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAcountService _acountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAcountService acountService, SignInManager<ApplicationUser> signInManager)
        {
            _acountService = acountService;
            _signInManager = signInManager;
        }

        #region Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel accountView)
        {
            if (!ModelState.IsValid) return View(accountView);

            var user = _acountService.ValidadteUser(accountView);

            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(accountView);
            }
            var Result = _signInManager.PasswordSignInAsync(user, accountView.Password, accountView.RememberMe, false).Result;

            if (Result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");

            if (Result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account Locked Out");

            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(accountView);



        }
        #endregion

        #region LogOut
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
