using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AcountService : IAcountService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public AcountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region Validate User
        public ApplicationUser? ValidadteUser(AccountViewModel accountViewModel)
        {
            var user = _userManager.FindByEmailAsync(accountViewModel.Email).Result;

            if (user is null)
            {
                return null;
            }

            var IsPasswordValid = _userManager.CheckPasswordAsync(user, accountViewModel.Password).Result;

            return IsPasswordValid ? user : null;
        }
        #endregion
    }
}
