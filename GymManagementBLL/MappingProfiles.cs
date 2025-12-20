using AutoMapper;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Session Mappings

            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

            CreateMap<CreateSessionViewModel, Session>()
                .ForMember(dest => dest.Capcity, opt => opt.MapFrom(src => src.Capacity));

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

            #endregion

            #region Member Mappings

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    new Address
                    {
                        Street = src.Street,
                        City = src.City,
                        BuildingNumber = src.BuildingNumber
                    }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src =>
                    new HealthRecord
                    {
                        Height = src.HealthRecordViewModel.Height,
                        Weight = src.HealthRecordViewModel.Weight,
                        BloodType = src.HealthRecordViewModel.BloodType,
                        Note = src.HealthRecordViewModel.Note
                    }));

            CreateMap<HealthRecord, HealthRecordViewModel>();

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ReverseMap()
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street));

            #endregion

            #region Trainer Mappings

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    new Address
                    {
                        BuildingNumber = src.BuildingNumber,
                        City = src.City,
                        Street = src.Street
                    }));

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialties.ToString()));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ReverseMap()
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street));

            #endregion

            #region Plan Mappings

            CreateMap<Plan, PlanViewModel>();

            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PlanName));

            #endregion

            #region Membership Mappings

            MapMembership();

            #endregion

            #region Booking Mappings

            MapBooking();

            #endregion
        }

        #region Membership Methods

        private void MapMembership()
        {
            CreateMap<MemberShip, MembershipViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<MembershipViewModel, MemberShip>();
            CreateMap<CreateMembershipViewModel, MemberShip>();

            CreateMap<Plan, PlanForSelectListViewModel>();
            CreateMap<Member, MemberForSelectListViewModel>();
        }

        #endregion

        #region Booking Methods

        private void MapBooking()
        {
            CreateMap<MemberSession, MemberForSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt.ToShortDateString()));

            CreateMap<CreateBookingViewModel, MemberSession>();
        }

        #endregion
    }
}
