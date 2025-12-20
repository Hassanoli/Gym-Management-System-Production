using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        #region Fields

        private readonly IMemberServices _memberServices;

        #endregion

        #region Constructor

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }

        #endregion

        #region Index

        public ActionResult Index()
        {
            return View(_memberServices.GetAllMbers());
        }

        #endregion

        #region Details

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));

            var member = _memberServices.GetMemberDeails(id);
            return member is null ? RedirectToAction(nameof(Index)) : View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            var record = _memberServices.GetMemberHealthRecordDetails(id);
            return record is null ? RedirectToAction(nameof(Index)) : View(record);
        }

        #endregion

        #region Create

        public ActionResult Create() => View();

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

            if (_memberServices.CreateMember(model))
                return RedirectToAction(nameof(Index));

            return View(nameof(Create), model);
        }

        #endregion

        #region Edit

        public ActionResult MemberEdit(int id)
        {
            var member = _memberServices.GetMemberToUpdate(id);
            return member is null ? RedirectToAction(nameof(Index)) : View(member);
        }

        [HttpPost]
        public ActionResult MemberEdit(int id, MemberToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _memberServices.UpdateMemberDetails(id, model);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            ViewBag.MemberId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            _memberServices.RemoveMember(id);
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
