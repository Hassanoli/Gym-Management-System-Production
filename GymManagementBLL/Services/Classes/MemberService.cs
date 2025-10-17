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
        public MemberService(IUintOfWork uintOfWork)
        {
            _uintOfWork = uintOfWork;
        }

        // ========== Get All Members ==========
        public IEnumerable<MemberViewModel> GetAllMbers()
        {
            var Members = _uintOfWork.GetRepository<Member>().GetAll();

            if (Members == null || !Members.Any())
                return Enumerable.Empty<MemberViewModel>();

            var MembersViewModels = Members.Select(X => new MemberViewModel()
            {
                Id = X.Id,
                Name = X.Name,
                Phone = X.Phone,
                Email = X.Email,
                Photo = X.Photo,
                Gender = X.Gender.ToString()
            });

            return MembersViewModels;
        }

        // ========== Create Member ==========
        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                // Check for duplicates
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone))
                    return false;

                // Manual Mapping (ViewModel → Entity)
                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        Street = createMember.Street,
                        City = createMember.City,
                        BuildingNumber = createMember.BuildingNumber,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note,
                    },
                };

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

            var viewModel = new MemberViewModel()
            {
                Name = member.Name,
                Phone = member.Phone,
                Email = member.Email,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
            };

            var ActivememberShip = _uintOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.MemberId == MemberId && x.Status == "Active")
                .FirstOrDefault();

            if (ActivememberShip is not null)
            {
                viewModel.MemberShipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = ActivememberShip.EndDate.ToShortDateString();

                var plan = _uintOfWork.GetRepository<Plan>().GetById(ActivememberShip.PlanId);
                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }

        // ========== Get Health Record ==========
        public HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid)
        {
            var MemberHealthRecord = _uintOfWork.GetRepository<HealthRecord>().GetById(Memberid);
            if (MemberHealthRecord == null) return null;

            return new HealthRecordViewModel()
            {
                Height = MemberHealthRecord.Height,
                Weight = MemberHealthRecord.Weight,
                BloodType = MemberHealthRecord.BloodType,
                Note = MemberHealthRecord.Note,
            };
        }

        // ========== Get Member to Update ==========
        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _uintOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            return new MemberToUpdateViewModel()
            {
                Photo = member.Photo,
                Name = member.Name,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street,
            };
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

                member.Email = memberToUpdate.Email;
                member.Phone = memberToUpdate.Phone;
                member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
                member.Address.City = memberToUpdate.City;
                member.Address.Street = memberToUpdate.Street;
                member.UpdatededAt = DateTime.Now;

                MemberRepo.Update(member);

                return _uintOfWork.SaveChanges()>0;
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
