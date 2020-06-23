using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.AccountViewModels
{
    public class PasswordEditViewModel
    {
        [Display(Name = "Your old password")]
        [StringLength(20, ErrorMessage = "Your password length is not proper", MinimumLength = 8)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to provide your old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Display(Name = "Your new password")]
        [StringLength(20, ErrorMessage = "Your password length is not proper", MinimumLength = 8)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to provide a new password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "Your new password confirmation")]
        [StringLength(20, ErrorMessage = "Your password length is not proper", MinimumLength = 8)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to confirm your new password")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirmation { get; set; }
    }
}
