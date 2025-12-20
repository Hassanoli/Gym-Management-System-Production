using GymManagementDAL.Entities.ENUMS;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        #region Properties

        // HireDate = CreatedAt
        public Specialties Specialties { get; set; }

        #endregion

        #region Relationships

        public ICollection<Session> TrainerSession { get; set; } = null!;

        #endregion
    }
}
