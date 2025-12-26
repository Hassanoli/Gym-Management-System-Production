using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.BookingViewModels
{
    public class MemberForSessionViewModel
    {
        #region Properties

        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int SessionId { get; set; }
        public string BookingDate { get; set; }
        public bool IsAttended { get; set; }

        public DateTime? AttendanceDate { get; set; }
        #endregion
    }
}
