using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
    public class SessionViewModel
    {
        #region Properties

        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TrainerName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public int AvailableSlots { get; set; }

        #endregion

        #region Computed Properties

        public string DateDisplay => StartDate.ToString("dd/MM/yyyy");

        public string TimeRangeDisplay =>
            $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";

        public TimeSpan Duration => EndDate - StartDate;

        public string Status
        {
            get
            {
                if (StartDate > DateTime.Now)
                    return "Upcoming";
                else if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
                    return "Ongoing";
                else
                    return "Completed";
            }
        }

        #endregion
    }
}
