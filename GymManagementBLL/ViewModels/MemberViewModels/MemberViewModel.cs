using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class MemberViewModel
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;

        public string? PlanName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? MemberShipStartDate { get; set; }
        public string? MemberShipEndDate { get; set; }
        public string? Address { get; set; }

        #endregion
    }
}
