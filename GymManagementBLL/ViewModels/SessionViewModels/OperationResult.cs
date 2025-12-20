using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
    public class OperationResult
    {
        #region Properties

        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        #endregion
    }
}
