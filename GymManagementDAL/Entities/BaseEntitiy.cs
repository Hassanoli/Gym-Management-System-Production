namespace GymManagementDAL.Entities
{
    // Abstract base class for all entities
    public abstract class BaseEntitiy
    {
        #region Properties

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        #endregion
    }
}
