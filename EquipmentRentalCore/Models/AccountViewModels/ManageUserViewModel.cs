using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.AccountViewModels
{
    public class ManageUserViewModel
    {
        [Display(Name = "User name")]
        public string Username { get; set; }
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "First name")]
        [StringLength(30, ErrorMessage = "Your first name is too long!")]
        public string FirstName { get; set; }
        [Display(Name = "Surname")]
        [StringLength(50, ErrorMessage = "Your surname is too long!")]
        public string Surname { get; set; }

        [Display(Name = "Groups to which user belongs")]
        public List<GroupViewModel> GroupList { get; set; }

        public PasswordEditViewModel PasswordEditViewModel { get; set; }
    }
}
