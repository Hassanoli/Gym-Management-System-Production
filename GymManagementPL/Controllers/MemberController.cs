using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index(int id)
        {
            return View();
        }
        public ActionResult GetMembers()
        {
            return View();
        }
        public ActionResult CreateMember()
        {
            return View();
        }

    }
}
