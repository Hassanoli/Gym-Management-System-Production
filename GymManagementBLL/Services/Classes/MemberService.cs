using AutoMapper;
using GymManagementBLL.Services.Attachment_Service;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberServices
    {
        #region Fields
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachment_Service _attachment_Service;
        #endregion

        #region Constructor
        public MemberService(
            IUintOfWork uintOfWork,
            IMapper mapper,
            IAttachment_Service attachment_Service)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
            _attachment_Service = attachment_Service;
        }
        #endregion

        #region Get All Members
        public IEnumerable<MemberViewModel> GetAllMbers()
        {
            var Members = _uintOfWork.GetRepository<Member>().GetAll();
            if (Members == null || !Members.Any()) return [];

            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(Members);
            return MemberViewModels;
        }
        #endregion

        #region Create Member
        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone))
                    return false;

                var PhotoName = _attachment_Service.Upload("Members", createMember.PhotoFile);

                if (string.IsNullOrEmpty(PhotoName))
                    return false;

                var member = _mapper.Map<Member>(createMember);
                member.Photo = PhotoName;

                _uintOfWork.GetRepository<Member>().Add(member);
                var IsCreated = _uintOfWork.SaveChanges() > 0;

                if (!IsCreated)
                {
                    _attachment_Service.Delete(PhotoName, "Members");
                }

                return IsCreated;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateMember: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Get Member Details
        public MemberViewModel? GetMemberDeails(int MemberId)
        {
            var member = _uintOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            var viewModel = _mapper.Map<MemberViewModel>(member);

            var activeMemberShip = _uintOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.MemberId == MemberId && x.Status == "Active")
                .FirstOrDefault();

            if (activeMemberShip is not null)
            {
                viewModel.MemberShipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = activeMemberShip.EndDate.ToShortDateString();

                var plan = _uintOfWork.GetRepository<Plan>()
                    .GetById(activeMemberShip.PlanId);

                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }
        #endregion

        #region Get Health Record
        public HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid)
        {
            var memberHealthRecord =
                _uintOfWork.GetRepository<HealthRecord>().GetById(Memberid);

            if (memberHealthRecord == null) return null;

            return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
        }
        #endregion

        #region Get Member For Update
        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _uintOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            return _mapper.Map<MemberToUpdateViewModel>(member);
        }
        #endregion

        #region Update Member
        #region Update Member
        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel model)
        {
            try
            {
                var memberRepo = _uintOfWork.GetRepository<Member>();
                var member = memberRepo.GetById(Id);

                if (member == null)
                    return false;

                // Check email & phone uniqueness
                bool emailExists = memberRepo
                    .GetAll(x => x.Email == model.Email && x.Id != Id).Any();

                bool phoneExists = memberRepo
                    .GetAll(x => x.Phone == model.Phone && x.Id != Id).Any();

                if (emailExists || phoneExists)
                    return false;

                // Update basic data
                _mapper.Map(model, member);
                member.UpdatedAt = DateTime.Now;

                #region Photo Update

                if (model.PhotoFile is not null)
                {
                    // Delete old photo
                    if (!string.IsNullOrEmpty(member.Photo))
                    {
                        _attachment_Service.Delete(member.Photo, "Members");
                    }

                    // Upload new photo
                    var newPhoto = _attachment_Service.Upload("Members", model.PhotoFile);

                    if (!string.IsNullOrEmpty(newPhoto))
                    {
                        member.Photo = newPhoto;
                    }
                }

                #endregion

                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UPDATE FAILED: {ex.Message}");
                return false;
            }
        }
        #endregion

        #endregion

        #region Remove Member
        public bool RemoveMember(int MemberId)
        {
            try
            {
                if (HasActiveSessions(MemberId))
                    return false;

                var memberRepo = _uintOfWork.GetRepository<Member>();
                var member = memberRepo.GetById(MemberId);

                if (member == null)
                    return false;

                memberRepo.Delete(MemberId);
                var isDeleted = _uintOfWork.SaveChanges() > 0;

                if (isDeleted)
                {
                    _attachment_Service.Delete(member.Photo, "Members");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- DELETE FAILED: {ex.Message} ---");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"--- INNER EX: {ex.InnerException.Message} ---");
                }
                return false;
            }
        }
        #endregion

        #region Helper Methods
        public bool IsEmailExists(string Email)
        {
            return _uintOfWork.GetRepository<Member>()
                .GetAll(X => X.Email == Email)
                .Any();
        }

        public bool IsPhoneExists(string Phone)
        {
            return _uintOfWork.GetRepository<Member>()
                .GetAll(X => X.Phone == Phone)
                .Any();
        }

        public bool IsEmailExists(string Email, int memberIdToExclude)
        {
            return _uintOfWork.GetRepository<Member>()
                .GetAll(X => X.Email == Email && X.Id != memberIdToExclude)
                .Any();
        }

        public bool IsPhoneExists(string Phone, int memberIdToExclude)
        {
            return _uintOfWork.GetRepository<Member>()
                .GetAll(X => X.Phone == Phone && X.Id != memberIdToExclude)
                .Any();
        }

        public bool HasActiveSessions(int MemberId)
        {
            var memberSessions = _uintOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.MemberId == MemberId)
                .ToList();

            if (!memberSessions.Any()) return false;

            var sessionIds = memberSessions
                .Select(s => s.SessionId)
                .ToList();

            bool hasActiveMemberSessions = _uintOfWork.GetRepository<Session>()
                .GetAll(x => sessionIds.Contains(x.Id) && x.StartDate > DateTime.Now)
                .Any();

            return hasActiveMemberSessions;
        }
        #endregion
    }
}
