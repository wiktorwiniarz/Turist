using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.CategorViewModels
{
    public class ManageCategorsModel
    {
        [HiddenInput]
        public int? Id { get; set; }
        [Display(Name = "Categor name")]
        [StringLength(30, ErrorMessage = "Name is too long")]
        [Required(ErrorMessage = "Name of categor is required", AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}
