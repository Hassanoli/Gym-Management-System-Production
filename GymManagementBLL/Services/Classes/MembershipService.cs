using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        #region Fields
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public MembershipService(IUintOfWork uintOfWork, IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods

        public IEnumerable<MembershipViewModel> GetAllMemberships()
        {
            var memberships = _uintOfWork.membershibRepository
                .GetAllMembershipWithMemberAndPlans(m => m.Status.ToLower() == "active");

            var membershipViewModels =
                _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);

            return membershipViewModels;
        }

        public bool CreateMembership(CreateMembershipViewModel model)
        {
            if (!IsMemberExits(model.MemberId)
                || !IsPlanExits(model.PlanId)
                || HasAcitveMembership(model.MemberId))
                return false;

            var membershipRepo = _uintOfWork.membershibRepository;
            var membershipToCreate = _mapper.Map<MemberShip>(model);

            var plan = _uintOfWork.GetRepository<Plan>().GetById(model.PlanId);
            membershipToCreate.EndDate = DateTime.UtcNow.AddDays(plan!.DurationDays);

            membershipRepo.Add(membershipToCreate);
            return _uintOfWork.SaveChanges() > 0;
        }

        public IEnumerable<PlanForSelectListViewModel> GetPLanForDropdown()
        {
            var plans = _uintOfWork
                .GetRepository<Plan>()
                .GetAll(p => p.IsActive);

            var planSelectList =
                _mapper.Map<IEnumerable<PlanForSelectListViewModel>>(plans);

            return planSelectList;
        }

        public IEnumerable<MemberForSelectListViewModel> GetMemberForDropDown()
        {
            var members = _uintOfWork.GetRepository<Member>().GetAll();

            var memberSelectList =
                _mapper.Map<IEnumerable<MemberForSelectListViewModel>>(members);

            return memberSelectList;
        }

        public bool DeleteMembership(int memberId)
        {
            var membershipRepo = _uintOfWork.membershibRepository;

            var membershipToDelete = membershipRepo
                .GetFirstOrDefault(m => m.MemberId == memberId && m.Status.ToLower() == "active");

            if (membershipToDelete is null)
                return false;

            membershipRepo.Delete(membershipToDelete);
            return _uintOfWork.SaveChanges() > 0;
        }

        #endregion

        #region Helper Methods

        private bool IsMemberExits(int memberId)
            => _uintOfWork.GetRepository<Member>().GetById(memberId) is not null;

        private bool IsPlanExits(int planId)
            => _uintOfWork.GetRepository<Plan>().GetById(planId) is not null;

        private bool HasAcitveMembership(int memberId)
            => _uintOfWork.membershibRepository
                .GetAllMembershipWithMemberAndPlans(
                    m => m.Status.ToLower() == "active" && m.MemberId == memberId)
                .Any();

        #endregion
    }
}
