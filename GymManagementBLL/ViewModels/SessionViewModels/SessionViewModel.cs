using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string TrainerName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Capacity { get; set; }

        public int AvailableSlots { get; set; }

        #region Computed Properties

        // 1. خاصية لعرض التاريخ فقط
        public string DateDisplay => StartDate.ToString("dd/MM/yyyy");

        // 2. خاصية لعرض الوقت فقط
        public string TimeRangeDisplay => $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";

        public TimeSpan Duration => EndDate - StartDate;

        public string Status
        {
            get
            {
                if (StartDate > DateTime.Now)
                    return "Upcoming"; // <-- تم تصحيح الخطأ الإملائي هنا
                else if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
                    return "Ongoing";
                else
                    return "Completed";
            }
        }

        #endregion
    }
}