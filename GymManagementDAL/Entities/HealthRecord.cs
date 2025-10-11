using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    // 1 to 1 Relationship with Member [Shared Pk] 
    public class HealthRecord : BaseEntitiy
    {
        // Id => Fk == Pk [Id]
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Note { get; set;}
    }
}
