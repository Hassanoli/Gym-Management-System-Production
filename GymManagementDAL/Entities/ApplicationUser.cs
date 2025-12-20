using Microsoft.AspNetCore.Identity;

namespace GymManagementDAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        #region Properties

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        #endregion
    }
}
