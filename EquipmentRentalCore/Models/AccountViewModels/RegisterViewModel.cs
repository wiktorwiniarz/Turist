using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "It is required to provide username")]
        public string Login { get; set; }
        [Display(Name = "Password"), StringLength(20)]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "It is required to provide password")]
        public string Password { get; set; }
        [Display(Name = "Password confirmation"), StringLength(20)]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "It is required to provide password confirmation!")]
        public string ConfirmPassword { get; set; }
    }
}
