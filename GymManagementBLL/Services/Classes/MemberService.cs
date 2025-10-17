using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementBLL.Services.Classes
{
    internal class MemberService : IMemberServices
    {
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;

        //// ========== Repositories ==========
        //private readonly IGenericRepository<Member> _memberRepository;
        //private readonly IGenericRepository<MemberShip> _memberShipRepository;
        //private readonly IPlanRepository _planRepository;
        //private readonly IGenericRepository<HealthRecord> _healthrecordRepository;
        //private readonly IGenericRepository<MemberSession> _memberSessionrepository;

        // ========== Constructor ==========
        //public MemberService(
        //    IGenericRepository<Member> memberRepository,
        //    IGenericRepository<MemberShip> memberShipRepository,
        //    IPlanRepository planRepository,
        //    IGenericRepository<HealthRecord> healthrecordRepository,
        //    IGenericRepository<MemberSession> memberSessionrepository)
        //{
        //    _memberRepository = memberRepository;
        //    _memberShipRepository = memberShipRepository;
        //    _planRepository = planRepository;
        //    _healthrecordRepository = healthrecordRepository;
        //    _memberSessionrepository = memberSessionrepository;
        //}
        public MemberService(IUintOfWork uintOfWork , IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }

        // ========== Get All Members ==========
        public IEnumerable<MemberViewModel> GetAllMbers()
        {
            var members = _uintOfWork.GetRepository<Member>().GetAll();

            if (members == null || !members.Any())
                return Enumerable.Empty<MemberViewModel>();

            return _mapper.Map<IEnumerable<MemberViewModel>>(members);
        }

        // ========== Create Member ==========
        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone))
                    return false;
                var member = _mapper.Map<Member>(createMember);

                _uintOfWork.GetRepository<Member>().Add(member);
                return _uintOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        // ========== Get Member Details ==========
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
                var plan = _uintOfWork.GetRepository<Plan>().GetById(activeMemberShip.PlanId);
                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }

        // ========== Get Health Record ==========
        public HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid)
        {
            var memberHealthRecord = _uintOfWork.GetRepository<HealthRecord>().GetById(Memberid);
            if (memberHealthRecord == null) return null;

            return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
        }

        // ========== Get Member to Update ==========
        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _uintOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        // ========== Update Member ==========
        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                if (IsEmailExists(memberToUpdate.Email) || IsPhoneExists(memberToUpdate.Phone))
                    return false;

                var MemberRepo = _uintOfWork.GetRepository<Member>();
                var member = MemberRepo.GetById(Id);
                if (member == null) return false;

                _mapper.Map(memberToUpdate, member);
                member.UpdatedAt = DateTime.Now;
                return _uintOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        // ========== Remove Member ==========
        public bool RemoveMember(int MemberId)
        {
            try
            {

                var member = _uintOfWork.GetRepository<Member>().GetById(MemberId);
                if (member == null) return false;

                var HasActiveMemberSessions = _uintOfWork.GetRepository<MemberSession>()
                    .GetAll(X => X.MemberId == MemberId && X.Session.StartDate > DateTime.Now)
                    .Any();

                if (HasActiveMemberSessions) return false;


                var MemberShipRepo = _uintOfWork.GetRepository<MemberShip>();
                var MemberShips = MemberShipRepo.GetAll(X => X.MemberId == MemberId);

                if (MemberShips.Any())
                {
                    foreach (var memberShip in MemberShips)
                        MemberShipRepo.Delete(memberShip.Id); // ✅ استخدم الـ Id
                }

                _uintOfWork.GetRepository<Member>().Delete(member.Id);

                return _uintOfWork.SaveChanges() > 0 ;// ✅ استخدم الـ Id
            }
            catch
            {
                return false;
            }
        }

        // ========== Helper Methods ==========
        #region Helper Methods
        private bool IsEmailExists(string Email)
        {
            return _uintOfWork.GetRepository<Member>().GetAll(X => X.Email == Email).Any();
        }

        private bool IsPhoneExists(string Phone)
        {
            return _uintOfWork.GetRepository<Member>().GetAll(X => X.Phone == Phone).Any();
        }
        #endregion
    }
}
