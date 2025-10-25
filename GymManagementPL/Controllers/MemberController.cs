using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        #region Fields & Constructor
        private readonly IMemberServices _memberServices;

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }
        #endregion

        #region Get All Members
        public ActionResult Index()
        {
            var members = _memberServices.GetAllMbers();
            return View(members);
        }
        #endregion

        #region Get Member Data
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id.";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMemberDeails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "HealthRecord Of Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            var HelathRecord = _memberServices.GetMemberHealthRecordDetails(id);
            if (HelathRecord is null)
            {
                TempData["ErrorMessage"] = "HealthRecord Of Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(HelathRecord);
        }
        #endregion

        #region Add Member
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Invalid data. Please correct the errors and try again.");
                return View(nameof(Create), createMember);
            }
            bool result = _memberServices.CreateMember(createMember);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (_memberServices.IsEmailExists(createMember.Email))
                    ModelState.AddModelError("DuplicateEmail", "This email is already registered.");
                if (_memberServices.IsPhoneExists(createMember.Phone))
                    ModelState.AddModelError("DuplicatePhone", "This phone number is already registered.");
                return View(nameof(Create), createMember);
            }
        }
        #endregion

        #region Update Member
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Numbers";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberServices.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            bool emailTaken = _memberServices.IsEmailExists(viewModel.Email, id);
            bool phoneTaken = _memberServices.IsPhoneExists(viewModel.Phone, id);
            if (emailTaken)
                ModelState.AddModelError("Email", "This email is already registered by another member.");
            if (phoneTaken)
                ModelState.AddModelError("Phone", "This phone number is already registered by another member.");
            if (emailTaken || phoneTaken)
                return View(viewModel);
            var result = _memberServices.UpdateMemberDetails(id, viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Member failed to update. An unexpected error occurred.";
                return View(viewModel);
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member

        
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberServices.GetMemberDeails(id);

            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
        }

       
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            // --- FIX #2: Added pre-check for better error message ---
            if (id == 0)
            {
                // This catches if the fix wasn't applied correctly
                TempData["ErrorMessage"] = "Delete Failed: ID was 0. Binding error.";
                return RedirectToAction(nameof(Index));
            }

            if (_memberServices.HasActiveSessions(id))
            {
                TempData["ErrorMessage"] = "Cannot delete member. They have active or future sessions.";
                return RedirectToAction(nameof(Index));
            }

            var Result = _memberServices.RemoveMember(id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                // This message will now appear if the database constraint fails
                TempData["ErrorMessage"] = "Member Failed To Delete. Check console log for database errors.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


    }
}