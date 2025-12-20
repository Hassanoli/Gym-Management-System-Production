using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Fields

        private readonly IAnalyticsService _analyticsService;

        #endregion

        #region Constructor

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        #endregion

        #region Index

        public ActionResult Index()
        {
            var data = _analyticsService.GetAnalyticsData();
            return View(data);
        }

        #endregion
    }
}
