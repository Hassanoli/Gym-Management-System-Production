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
    internal class TrainerService : ITrainerServices
    {
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUintOfWork uintOfWork, IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }

        public bool CreateTariner(CreateTrainerViewModel createTrainer)
        {
            try
            {
                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone))
                    return false;

                var trainer = _mapper.Map<Trainer>(createTrainer);

                _uintOfWork.GetRepository<Trainer>().Add(trainer);
                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _uintOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any())
                return Enumerable.Empty<TrainerViewModel>();

        
            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
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

        public bool UpdateTrainerDetails(UpdateTrainerViewModel updateTrainer, int trainerId)
        {
            try
            {
                var repo = _uintOfWork.GetRepository<Trainer>();
                var trainerToUpdate = repo.GetById(trainerId);
                if (trainerToUpdate is null) return false;

              
                if (repo.GetAll(t => t.Id != trainerId && (t.Email == updateTrainer.Email || t.Phone == updateTrainer.Phone)).Any())
                {
                    return false;
                }

             
                _mapper.Map(updateTrainer, trainerToUpdate);

                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveTrainer(int trainerId)
        {
            var repo = _uintOfWork.GetRepository<Trainer>();
            var trainerToRemove = repo.GetById(trainerId);

            
            if (trainerToRemove is null || HasActiveSessions(trainerId))
                return false;

            repo.Delete(trainerToRemove.Id);
            return _uintOfWork.SaveChanges() > 0;
        }

        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            return _uintOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        }

        private bool IsPhoneExists(string phone)
        {
            return _uintOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone).Any();
        }

        private bool HasActiveSessions(int trainerId)
        {
         
            return _uintOfWork.GetRepository<Session>()
                .GetAll(s => s.TrainerId == trainerId && s.EndDate > DateTime.Now)
                .Any();
        }
        #endregion
    }
}