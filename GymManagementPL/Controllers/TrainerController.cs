using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GymManagementPL.Controllers
{

    [Authorize(Roles ="SuperAdmin")]
    public class TrainerController : Controller
    {
        #region Fields & Constructor
        private readonly ITrainerServices _trainerServices;

        public TrainerController(ITrainerServices trainerServices)
        {
            _trainerServices = trainerServices;
        }
        #endregion

        #region Get All Trainers (Index)
        public IActionResult Index()
        {
            var trainers = _trainerServices.GetAllTrainers();
            return View(trainers);
        }
        #endregion

        #region Create Trainer
        
        public IActionResult Create()
        {
           
            return View();
        }

       
        [HttpPost]
      
        public IActionResult Create(CreateTrainerViewModel viewModel)
        {
           
            if (!ModelState.IsValid)
            {
               
                return View(viewModel);
            }

         
            bool result = _trainerServices.CreateTrainer(viewModel);

            if (result)
            {
             
                TempData["SuccessMessage"] = "Trainer created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
              
                if (_trainerServices.IsEmailExists(viewModel.Email))
                    ModelState.AddModelError("Email", "This email is already registered.");

                if (_trainerServices.IsPhoneExists(viewModel.Phone))
                    ModelState.AddModelError("Phone", "This phone number is already registered.");

            
                if (!ModelState.Any())
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the trainer.");

                return View(viewModel);
            }
        }
        #endregion

     
        #region Get Trainer Details
     
        public IActionResult Details(int id)
        {    
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID.";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerServices.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer); 
        }
        #endregion

      
        #region Update Trainer
       
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID.";
                return RedirectToAction(nameof(Index));
            }

          
            var trainerToUpdate = _trainerServices.GetTrainerToUpdate(id);

            if (trainerToUpdate == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainerToUpdate); 
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel viewModel)
        {
         
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID provided.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
             
                return View(viewModel);
            }

            bool result = _trainerServices.UpdateTrainerDetails(id, viewModel);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (_trainerServices.IsEmailExists(viewModel.Email, id))
                    ModelState.AddModelError("Email", "This email is already registered by another trainer.");

                if (_trainerServices.IsPhoneExists(viewModel.Phone, id))
                    ModelState.AddModelError("Phone", "This phone number is already registered by another trainer.");

                if (!ModelState.ContainsKey("Email") && !ModelState.ContainsKey("Phone"))
                    ModelState.AddModelError(string.Empty, "Failed to update trainer. Please check the data or try again.");

                return View(viewModel);
            }
        }
        #endregion

      
        #region Delete Trainer
      
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID.";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerServices.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

          
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer.Name; 

            return View(); 
        }

      
        [HttpPost]
      
        [ActionName("Delete")]
       
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID provided for deletion.";
                return RedirectToAction(nameof(Index));
            }

           
            if (_trainerServices.HasActiveSessions(id))
            {
                TempData["ErrorMessage"] = "Cannot delete trainer. They have active or future sessions assigned.";
                return RedirectToAction(nameof(Index));
            }

           
            bool result = _trainerServices.RemoveTrainer(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            }
            else
            {
                
                TempData["ErrorMessage"] = "Failed to delete trainer. It might have already been deleted or an error occurred.";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}