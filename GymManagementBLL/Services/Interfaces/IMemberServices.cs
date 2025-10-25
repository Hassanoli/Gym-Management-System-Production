using GymManagementBLL.ViewModels.MemberViewModel;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberServices
    {
        #region Main CRUD Methods
        IEnumerable<MemberViewModel> GetAllMbers();
        bool CreateMember(CreateMemberViewModel createMember);
        MemberViewModel? GetMemberDeails(int MemberId);
        HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid);
        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
        bool UpdateMemberDetails(int Id, MemberToUpdateViewModel memberToUpdate);
        bool RemoveMember(int MemberId);
        #endregion

        #region Validation & Helper Methods
        bool IsEmailExists(string Email);
        bool IsPhoneExists(string Phone);
        bool IsEmailExists(string Email, int memberIdToExclude);
        bool IsPhoneExists(string Phone, int memberIdToExclude);
        bool HasActiveSessions(int MemberId);
        #endregion
    }
}