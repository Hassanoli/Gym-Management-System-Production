using GymManagementDAL.Entities.ENUMS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public abstract class GymUser : BaseEntitiy
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly DateOfBirth { get; set;}
        public Gender Gender { get; set;}
        public Address Address { get; set; } = null!;
    }

    #region Address

    //Should download NuGet Package to use this 
    [Owned] 
    public class Address
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
    }
    #endregion





}
