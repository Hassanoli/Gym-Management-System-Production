using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class MemberToUpdateViewModel
    {
        #region Name
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Name { get; set; } = null!;
        #endregion

        #region Email
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        #endregion

        #region Phone
        [Required]
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$")]
        public string Phone { get; set; } = null!;
        #endregion

        #region Address
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        #endregion

        #region Photo
        public string? Photo { get; set; }      // الصورة القديمة
        public IFormFile? PhotoFile { get; set; } // الصورة الجديدة
        #endregion
    }
}
