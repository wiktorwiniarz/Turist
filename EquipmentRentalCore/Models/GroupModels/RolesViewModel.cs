using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.GroupModels
{
    public class RolesViewModel
    {
        public RolesViewModel()
        {
            UsersList = new List<UsersInRoleViewModel>();
            ChooseAdditionalUserList = new List<SelectListItem>();
        }
        public int RoleID { get; set; }
        [Display(Name = "Nazwa grupy")]
        public string RoleName { get; set; }
        [Display(Name = "Lista użytkowników w grupie")]
        public List<UsersInRoleViewModel> UsersList { get; set; }
        public int? ChosenListID { get; set; }
        [Display(Name = "Wybierz użytkownika do dodania")]
        public List<SelectListItem> ChooseAdditionalUserList { get; set; }
    }
}
