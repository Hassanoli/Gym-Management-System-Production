using GymManagementDAL.Entities.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        // HireDate == CreatedAt Of BaseEntity

        public Specialties Specialties { get; set; }

        public ICollection<Session> TrainerSession { get; set; } = null!;
        
    }
}
