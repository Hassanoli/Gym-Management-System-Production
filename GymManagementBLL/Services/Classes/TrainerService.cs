using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerServices
    {
        #region Fields
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public TrainerService(IUintOfWork uintOfWork, IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _uintOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any())
                return Enumerable.Empty<TrainerViewModel>();

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

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

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _uintOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            return _mapper.Map<TrainerViewModel>(trainer);
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _uintOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }

        public bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel updateTrainer)
        {
            try
            {
                var repo = _uintOfWork.GetRepository<Trainer>();
                var trainerToUpdate = repo.GetById(trainerId);
                if (trainerToUpdate is null) return false;

                if (IsEmailExists(updateTrainer.Email, trainerId) ||
                    IsPhoneExists(updateTrainer.Phone, trainerId))
                    return false;

                _mapper.Map(updateTrainer, trainerToUpdate);
                trainerToUpdate.UpdatedAt = DateTime.Now;

                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- UPDATE TRAINER FAILED: {ex.Message} ---");
                return false;
            }
        }

        public bool RemoveTrainer(int trainerId)
        {
            try
            {
                if (HasActiveSessions(trainerId))
                    return false;

                var repo = _uintOfWork.GetRepository<Trainer>();
                repo.Delete(trainerId);

                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
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

        #region Helper Methods

        public bool IsEmailExists(string email)
        {
            return _uintOfWork.GetRepository<Trainer>()
                .GetAll(t => t.Email == email)
                .Any();
        }

        public bool IsPhoneExists(string phone)
        {
            return _uintOfWork.GetRepository<Trainer>()
                .GetAll(t => t.Phone == phone)
                .Any();
        }

        public bool IsEmailExists(string email, int trainerIdToExclude)
        {
            return _uintOfWork.GetRepository<Trainer>()
                .GetAll(t => t.Email == email && t.Id != trainerIdToExclude)
                .Any();
        }

        public bool IsPhoneExists(string phone, int trainerIdToExclude)
        {
            return _uintOfWork.GetRepository<Trainer>()
                .GetAll(t => t.Phone == phone && t.Id != trainerIdToExclude)
                .Any();
        }

        public bool HasActiveSessions(int trainerId)
        {
            return _uintOfWork.GetRepository<Session>()
                .GetAll(s => s.TrainerId == trainerId && s.EndDate > DateTime.Now)
                .Any();
        }

        #endregion
    }
}
