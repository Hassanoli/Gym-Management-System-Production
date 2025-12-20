namespace GymManagementDAL.Entities
{
    public class Session : BaseEntitiy
    {
        #region Properties

        public string Description { get; set; } = null!;
        public int Capcity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #endregion

        #region Relationships

        #region Category

        public int CategoryId { get; set; }
        public Category SessionCategory { get; set; } = null!;

        #endregion

        #region Trainer

        public int TrainerId { get; set; }
        public Trainer SessionTrainer { get; set; } = null!;

        #endregion

        #region Members

        public ICollection<MemberSession> SessionMembers { get; set; } = null!;

        #endregion

        #endregion
    }
}
