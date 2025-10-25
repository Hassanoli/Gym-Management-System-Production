using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels; // Make sure this namespace is correct
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementBLL.Services.Classes
{
    // Make class public
    public class TrainerService : ITrainerServices
    {
        #region Fields & Constructor
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUintOfWork uintOfWork, IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Get All Trainers
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _uintOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any())
                return Enumerable.Empty<TrainerViewModel>();

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }
        #endregion

        #region Create Trainer
        // Fix typo: CreateTariner -> CreateTrainer
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone))
                    return false;

                var trainer = _mapper.Map<Trainer>(createTrainer);
                _uintOfWork.GetRepository<Trainer>().Add(trainer);
                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- CREATE TRAINER FAILED: {ex.Message} ---");
                return false;
            }
        }
        #endregion

        #region Get Trainer Details
        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _uintOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;
            return _mapper.Map<TrainerViewModel>(trainer);
        }
        #endregion

        #region Get Trainer To Update
        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _uintOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;
            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }
        #endregion

        #region Update Trainer Details
        // Use TrainerToUpdateViewModel
        public bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel updateTrainer)
        {
            try
            {
                var repo = _uintOfWork.GetRepository<Trainer>();
                var trainerToUpdate = repo.GetById(trainerId);
                if (trainerToUpdate is null) return false;

                // Check for duplicates using helper methods
                if (IsEmailExists(updateTrainer.Email, trainerId) || IsPhoneExists(updateTrainer.Phone, trainerId))
                {
                    return false; // Email or phone taken by another trainer
                }

                _mapper.Map(updateTrainer, trainerToUpdate); // Map changes
                trainerToUpdate.UpdatedAt = DateTime.Now; // Update timestamp

                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- UPDATE TRAINER FAILED: {ex.Message} ---");
                return false;
            }
        }
        #endregion

        #region Remove Trainer
        public bool RemoveTrainer(int trainerId)
        {
            try
            {
                // Check business rule first (doesn't require tracking)
                if (HasActiveSessions(trainerId))
                    return false;

                var repo = _uintOfWork.GetRepository<Trainer>();

                // Delete directly by ID (avoids tracking conflict)
                // Assumes your GenericRepository.Delete(int id) works correctly
                // by creating a stub entity and setting its state to Deleted.
                repo.Delete(trainerId);

                // Cascade delete in DB handles removing related Sessions
                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) // Log errors (like concurrency if ID doesn't exist)
            {
                Console.WriteLine($"--- DELETE TRAINER FAILED: {ex.Message} ---");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"--- INNER EX: {ex.InnerException.Message} ---");
                }
                return false;
            }
        }
        #endregion

        #region Helper Methods (Make public)
        public bool IsEmailExists(string email)
        {
            return _uintOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        }

        public bool IsPhoneExists(string phone)
        {
            return _uintOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone).Any();
        }

        public bool IsEmailExists(string email, int trainerIdToExclude)
        {
            return _uintOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email && t.Id != trainerIdToExclude).Any();
        }

        public bool IsPhoneExists(string phone, int trainerIdToExclude)
        {
            return _uintOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone && t.Id != trainerIdToExclude).Any();
        }

        public bool HasActiveSessions(int trainerId)
        {
            // Check for sessions linked to this trainer ENDING in the future
            return _uintOfWork.GetRepository<Session>()
                .GetAll(s => s.TrainerId == trainerId && s.EndDate > DateTime.Now)
                .Any();
        }
        #endregion
    }
}