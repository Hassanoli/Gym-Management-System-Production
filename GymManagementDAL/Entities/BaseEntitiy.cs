using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    // its abstract because inheited and implement
    public abstract  class BaseEntitiy 
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatededAt { get; set; } = DateTime.Now;
    }
}
