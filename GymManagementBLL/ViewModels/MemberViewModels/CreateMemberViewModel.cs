using GymManagementDAL.Entities.ENUMS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class CreateMemberViewModel
    {
        #region Name
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can contain only letters and spaces.")]
        public string Name { get; set; } = null!;
        #endregion

        #region Email
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public string Email { get; set; } = null!;
        #endregion

        #region Phone
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Enter a valid Egyptian phone number.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 digits.")]
        public string Phone { get; set; } = null!;
        #endregion

        #region Photo
        //[Url(ErrorMessage = "Photo must be a valid URL.")]
        //[StringLength(255, ErrorMessage = "Photo URL must not exceed 255 characters.")]
        //public string? Photo { get; set; }
        [Required(ErrorMessage = "Photo is required.")]
        [Display(Name = "Profile Photo")]
        public IFormFile PhotoFile { get; set; } = null!;
        #endregion

        #region Address

        #endregion

        #region Date Of Birth
        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        #endregion

        #region Gender 
        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }
        #endregion

        #region Detailed Address
        [Required(ErrorMessage = "Building number is required.")]
        [Range(1, 10000, ErrorMessage = "Building number must be between 1 and 10000.")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street can contain only letters and spaces.")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can contain only letters and spaces.")]
        public string City { get; set; } = null!;
        #endregion

        #region HelathRecordViewModel 
        [Required(ErrorMessage = "HealthRecordViewModel Is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
        #endregion
        

    }
}
