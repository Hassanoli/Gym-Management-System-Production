using GymManagementDAL.Entities.ENUMS;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Entities
{
    public abstract class GymUser : BaseEntitiy
    {
        #region Properties

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        #endregion

        #region Owned Types

        public Address Address { get; set; } = null!;

        #endregion
    }

    #region Address

    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
    }

    #endregion
}
