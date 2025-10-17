using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModel
{
    internal class UpdatePlanViewModel
    {
        #region Plan Properties

        [Required(ErrorMessage = "PlanName is required.")]
        [StringLength(50, ErrorMessage = "PlanName length must be less than 51 characters.")]
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Description length must be between 2 and 50 characters.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "DurationDays is required.")]
        [Range(1, 365, ErrorMessage = "Duration Days must be between 1 and 365.")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.1, 10000, ErrorMessage = "Price must be between 0.1 and 10000.")]
        public decimal Price { get; set; }

        #endregion
 

    }
}
