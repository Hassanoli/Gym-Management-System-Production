using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanServices
    {
        #region Fields
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public PlanService(IUintOfWork uintOfWork, IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Get All Plans
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _uintOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any())
                return Enumerable.Empty<PlanViewModel>();

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }
        #endregion

        #region Get Plan Details
        public PlanViewModel? GetPlanDetails(int PlanId)
        {
            var plan = _uintOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null) return null;

            return _mapper.Map<PlanViewModel>(plan);
        }
        #endregion

        #region Get Plan For Update
        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var plan = _uintOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false || HasActiveMemberShip(PlanId))
                return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
        }
        #endregion

        #region Update Plan
        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
            try
            {
                var planRepo = _uintOfWork.GetRepository<Plan>();
                var plan = planRepo.GetById(PlanId);

                if (plan is null || HasActiveMemberShip(PlanId))
                    return false;

                _mapper.Map(updatedPlan, plan);
                plan.UpdatedAt = DateTime.Now;

                return _uintOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Toggle Plan Status
        public bool ToggleStatus(int PlanId)
        {
            var planRepo = _uintOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(PlanId);

            if (plan is null || HasActiveMemberShip(PlanId))
                return false;

            plan.IsActive = !plan.IsActive;

            try
            {
                return _uintOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Helper Methods
        private bool HasActiveMemberShip(int PlanId)
        {
            return _uintOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.PlanId == PlanId && x.Status == "Active")
                .Any();
        }
        #endregion
    }
}
