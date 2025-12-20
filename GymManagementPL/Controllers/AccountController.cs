using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        #region Fields

        private readonly IAcountService _acountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        #endregion

        #region Constructor

        public AccountController(
            IAcountService acountService,
            SignInManager<ApplicationUser> signInManager)
        {
            _acountService = acountService;
            _signInManager = signInManager;
        }

        #endregion

        #region Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel accountView)
        {
            if (!ModelState.IsValid)
                return View(accountView);

            var user = _acountService.ValidadteUser(accountView);

            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(accountView);
            }

            var result = _signInManager.PasswordSignInAsync(
                user,
                accountView.Password,
                accountView.RememberMe,
                false).Result;

            if (result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");

            if (result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account Locked Out");

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(accountView);
        }

        #endregion

        #region Logout

        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region Access Denied

        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}
