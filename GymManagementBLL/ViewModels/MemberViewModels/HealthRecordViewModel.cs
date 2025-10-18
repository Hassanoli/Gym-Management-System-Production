using System;
using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class HealthRecordViewModel
    {
        #region Height
        [Required(ErrorMessage = "Height is required.")]
        [Range(0.1, 300, ErrorMessage = "Height must be between 0.1 cm and 300 cm.")]
        public decimal Height { get; set; }
        #endregion

        #region Weight
        [Required(ErrorMessage = "Weight is required.")]
        [Range(30, 500, ErrorMessage = "Weight must be between 30 kg and 500 kg.")]
        public decimal Weight { get; set; }
        #endregion

        #region Blood Type
        [Required(ErrorMessage = "Blood type is required.")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "Blood type must be 1 to 3 characters (e.g., A+, B-, O+).")]
        [RegularExpression(@"^(A|B|AB|O)[+-]?$", ErrorMessage = "Blood type must be valid (e.g., A+, O-, AB).")]
        public string BloodType { get; set; } = null!;
        #endregion

        #region Note
        [StringLength(250, ErrorMessage = "Note cannot exceed 250 characters.")]
        public string? Note { get; set; } 
        #endregion
    }
}
