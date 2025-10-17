using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface ITrainerServices
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTariner(CreateTrainerViewModel createTrainer);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);
        bool UpdateTrainerDetails(UpdateTrainerViewModel updateTrainer, int trainerId);
        bool RemoveTrainer(int trainerId);
        TrainerViewModel? GetTrainerDetails(int trainerId);
    }
}
