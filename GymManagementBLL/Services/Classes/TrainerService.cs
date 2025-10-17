using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementBLL.Services.Classes
{
    internal class TrainerService : ITrainerServices
    {
        private readonly IUintOfWork _uintOfWork;

        public TrainerService(IUintOfWork uintOfWork)
        {
            _uintOfWork = uintOfWork;
        }

        public bool CreateTariner(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var repo = _uintOfWork.GetRepository<Trainer>();

                // Validate unique email and phone
                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone))
                    return false;

                // Create new trainer entity
                var trainer = new Trainer
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    Gender = createTrainer.Gender,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Address = new Address
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        City = createTrainer.City,
                        Street = createTrainer.Street
                    },
                    Specialties = createTrainer.Specialties
                };

                repo.Add(trainer);
                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            var repo = _uintOfWork.GetRepository<Trainer>();
            return repo.GetAll().Any(t => t.Email == email);
        }

        private bool IsPhoneExists(string phone)
        {
            var repo = _uintOfWork.GetRepository<Trainer>();
            return repo.GetAll().Any(t => t.Phone == phone);
        }

        // ✅ New helper to check if trainer has active sessions (بدون ما نعدل Session class)
        private bool HasActiveSessions(int trainerId)
        {
            var sessionRepo = _uintOfWork.GetRepository<Session>();

            // بنستخدم StartDate و EndDate علشان نعتبر الجلسة نشطة
            var activeSessions = sessionRepo.GetAll()
                .Where(s => s.TrainerId == trainerId && s.EndDate > DateTime.Now);

            return activeSessions.Any();
        }
        #endregion

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _uintOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return Array.Empty<TrainerViewModel>();

            return trainers.Select(X => new TrainerViewModel()
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Specialties = X.Specialties.ToString()
            });
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _uintOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            return new TrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialties.ToString()
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _uintOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
                BuildingNumber = trainer.Address.BuildingNumber,
                Specialties = trainer.Specialties
            };
        }

        public bool RemoveTrainer(int trainerId)
        {

            var Repo = _uintOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(trainerId);
            if(TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove.Id);
            return _uintOfWork.SaveChanges() > 0;
        }



        public bool UpdateTrainerDetails(UpdateTrainerViewModel updateTrainer, int trainerId)
        {
            var Repo = _uintOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(trainerId);

            if (TrainerToUpdate is null || IsEmailExists(updateTrainer.Email) || IsPhoneExists(updateTrainer.Phone)) return false;
            TrainerToUpdate.Name = updateTrainer.Name;
            TrainerToUpdate.Email = updateTrainer.Email;
            TrainerToUpdate.Phone = updateTrainer.Phone;
            TrainerToUpdate.Address.City = updateTrainer.City;
            TrainerToUpdate.Address.Street = updateTrainer.Street;
            TrainerToUpdate.Address.BuildingNumber = updateTrainer.BuildingNumber;
            TrainerToUpdate.Specialties = updateTrainer.Specialties;
            Repo.Update(TrainerToUpdate);
            return _uintOfWork.SaveChanges() > 0;
        }
    }
}
