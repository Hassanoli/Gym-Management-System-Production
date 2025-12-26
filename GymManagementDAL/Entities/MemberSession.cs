    namespace GymManagementDAL.Entities
    {
        public class MemberSession : BaseEntitiy
        {
            #region Properties

            // BookingDate = CreatedAt
            public bool IsAttended { get; set; }

            #endregion

            #region Relationships

            public int MemberId { get; set; }
            public Member Member { get; set; } = null!;

            public int SessionId { get; set; }
            public Session Session { get; set; } = null!;

            #endregion
        }
    }
