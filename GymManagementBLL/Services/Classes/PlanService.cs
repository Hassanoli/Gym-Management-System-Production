using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class PlanService : IPlanServices
    {
        private readonly IUintOfWork _uintOfWork;
        public PlanService(IUintOfWork uintOfWork)
        {
            _uintOfWork = uintOfWork;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _uintOfWork.GetRepository<Plan>().GetAll();
            if (Plans == null || !Plans.Any()) return []; 

            return Plans.Select(X => new PlanViewModel
            {
                Description = X.Description,
                DurationDays = X.DurationDays,
                Id = X.Id,
                IsActive = X.IsActive,
                Name = X.Name,
                Price = X.Price
            }); 
        }

        public PlanViewModel? GetPlanDetails(int PlanId)
        {
           var plan = _uintOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null) return null;
            return new PlanViewModel
            {
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Id = plan.Id,
                IsActive = plan.IsActive,
                Name = plan.Name,
                Price = plan.Price
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
           var plan = _uintOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false || HasActiveMemberShip(PlanId)) return null;
            return new UpdatePlanViewModel
            {
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                PlanName = plan.Name,
                Price = plan.Price
            };
        }


        public bool UpdatePlan(int PlanId, UpdatePlanViewModel UpdatedPlan)
        {
           try
            {
                var PlanRepo = _uintOfWork.GetRepository<Plan>();
                var Plan = PlanRepo.GetById(PlanId);

                if (Plan is null || HasActiveMemberShip(PlanId)) return false;
                (Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatededAt) =
                    (UpdatedPlan.Description, UpdatedPlan.Price, UpdatedPlan.DurationDays, DateTime.Now);

                PlanRepo.Update(Plan);
                return _uintOfWork.SaveChanges() > 0;
            }
            catch
            {
                 return false;
            }

        }
        public bool ToggleStatus(int PlanId)
        {
            var PlanRepo = _uintOfWork.GetRepository<Plan>();
            var Plan = PlanRepo.GetById(PlanId);

            if (Plan is null || HasActiveMemberShip(PlanId)) return false;

            Plan.IsActive = Plan.IsActive == true ? false : true;
            try
            {
                PlanRepo.Update(Plan);
                return _uintOfWork.SaveChanges() > 0; 
            }
            catch
            {
                return false; 
            }
        }

        #region Helper Method 
        private bool HasActiveMemberShip(int PlanId)
        {
            return _uintOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.PlanId == PlanId && x.Status == "Active")
                .Any(); 
        }
        #endregion
    }
}
