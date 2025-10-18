using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult GetTrainers()
        {
            return View();
        }
        public ActionResult CreateTrainer()
        {
            return View();
        }
    }
}
