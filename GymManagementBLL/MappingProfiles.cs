using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels; // Make sure this namespace is correct
using GymManagementDAL.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Session Mappings
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            #endregion

            #region Member Mappings
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address { Street = src.Street, City = src.City, BuildingNumber = src.BuildingNumber }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => new HealthRecord { Height = src.HealthRecordViewModel.Height, Weight = src.HealthRecordViewModel.Weight, BloodType = src.HealthRecordViewModel.BloodType, Note = src.HealthRecordViewModel.Note }));
            CreateMap<HealthRecord, HealthRecordViewModel>();
            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ReverseMap() // Map back from ViewModel to Entity for Update
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street));
            #endregion

            #region Trainer Mappings (Corrected)
            // CreateTrainerViewModel -> Trainer (Handles Address)
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address { BuildingNumber = src.BuildingNumber, City = src.City, Street = src.Street }));

            // Trainer -> TrainerViewModel (For display)
            CreateMap<Trainer, TrainerViewModel>()
                 .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString())) // Add Gender mapping
                 .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString())) // Add DoB mapping
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}")) // Add Address mapping
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialties.ToString()));

            // Trainer <-> TrainerToUpdateViewModel (Handles Address correctly)
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ReverseMap() // Map back from ViewModel to Entity for Update
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street));

            // Remove or adjust UpdateTrainerViewModel mapping if not needed
            // CreateMap<UpdateTrainerViewModel, Trainer>() ...

            #endregion

            #region Plan Mappings
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PlanName));
            #endregion
        }
    }
}