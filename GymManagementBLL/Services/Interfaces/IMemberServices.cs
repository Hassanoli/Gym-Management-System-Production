using GymManagementBLL.ViewModels.MemberViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface IMemberServices
    {
        IEnumerable<MemberViewModel> GetAllMbers();
        bool CreateMember(CreateMemberViewModel createMember);
        MemberViewModel? GetMemberDeails(int id);
        HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid);
        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
        bool UpdateMemberDetails(int Id , MemberToUpdateViewModel memberToUpdate);
        bool RemoveMember(int MemberId);
    }
}
