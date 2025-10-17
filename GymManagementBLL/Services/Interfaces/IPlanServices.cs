using GymManagementBLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface IPlanServices
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanDetails(int PlanId);
        UpdatePlanViewModel? GetPlanToUpdate(int PlanId);
        bool UpdatePlan(int PlanId, UpdatePlanViewModel UpdatedPlan);
        bool ToggleStatus(int PlanId);
    }
}
