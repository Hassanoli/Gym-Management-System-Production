namespace GymManagementDAL.Entities
{
    public class MemberShip : BaseEntitiy
    {
        #region Properties

        // StartDate = CreatedAt
        public DateTime EndDate { get; set; }

        public string Status
        {
            get
            {
                if (EndDate <= DateTime.Now)
                    return "Expired";
                else
                    return "Active";
            }
        }

        #endregion

        #region Relationships

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId
        {
            get; set;
        }
        public Plan Plan { get; set; } = null!;

        #endregion
    }
}
