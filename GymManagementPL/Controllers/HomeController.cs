using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO; // Required for Path and Directory

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;
        #region View Results

        // Returns an HTML view
        // GET: /Home/Index

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public ActionResult Index()
        {
            var Data = _analyticsService.GetAnalyticsData();
            return View(Data);
        }
        #endregion

        //#region Data Results

        //// Returns data in JSON format
        //// GET: /Home/Trainers
        //public ActionResult Trainers()
        //{
        //    var trainers = new List<Trainer>()
        //    {
        //        new Trainer() { Name = "John Doe", Phone = "01550122173" },
        //        new Trainer() { Name = "Jane Smith", Phone = "01550122174" },
        //        new Trainer() { Name = "Mike Johnson", Phone = "01550122175" }
        //    };
        //    return Json(trainers);
        //}

        //// Returns a plain text string
        //// GET: /Home/Content
        //public ActionResult Content()
        //{
        //    return Content("Welcome to Gym Management System");
        //}

        //#endregion

        //#region File & Redirection Results

        //// Serves a file for download
        //// GET: /Home/Downloadfile
        //public ActionResult Downloadfile()
        //{
        //    // A more robust way to get the file path
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "site.css");
        //    var fileBytes = System.IO.File.ReadAllBytes(filePath);
        //    return File(fileBytes, "text/css", "DownloadedSite.css");
        //}

        //// Redirects to an external URL
        //// GET: /Home/Redirect
        //public ActionResult Redirect()
        //{
        //    return Redirect("https://play.anghami.com/");
        //}

        //#endregion

        //#region Special Action Results

        //// Returns an empty response
        //// GET: /Home/EmptyAction
        //public ActionResult EmptyAction()
        //{
        //    return new EmptyResult();
        //}

        //#endregion
    }
}
