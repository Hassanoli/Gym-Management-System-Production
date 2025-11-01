using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null! ;
        public string LastName { get; set; } = null! ;
    }
}
