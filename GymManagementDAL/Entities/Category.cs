namespace GymManagementDAL.Entities
{
    public class Category : BaseEntitiy
    {
        #region Properties

        public string CategoryName { get; set; } = null!;

        #endregion

        #region Relationships

        public ICollection<Session> Sessions { get; set; } = null!;

        #endregion
    }
}
