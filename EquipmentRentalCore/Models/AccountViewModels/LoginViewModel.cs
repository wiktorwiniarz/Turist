using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "It is required to provide your username")]
        [StringLength(20, ErrorMessage = "The provided username is too long")]
        public string Login { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "It is required to provide your password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The password you have provided is too long or too short, it needs to be between 8 and 20 signs")]
        public string Password { get; set; }

        [Display(Name = "Remember login")]
        public bool RememberLogin { get; set; }
    }
}
