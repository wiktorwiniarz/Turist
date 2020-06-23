using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.EquipmentTypeViewModel
{
    public class ManageEquipmentTypeModel
    {
        [HiddenInput]
        public int? Id { get; set; }
        [Display(Name = "Name of type")]
        [StringLength(30, ErrorMessage = "Name is too long")]
        [Required(ErrorMessage = "Name of type is required", AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}
