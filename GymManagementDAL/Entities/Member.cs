namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        #region Properties

        public string Photo { get; set; } = null!;

        #endregion

        #region Relationships

        public HealthRecord HealthRecord { get; set; } = null!;
        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        public ICollection<MemberSession> MemberSession { get; set; } = null!;

        #endregion
    }
}
