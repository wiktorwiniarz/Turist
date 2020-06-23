using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.CategorViewModels
{
    public class ListCategorsModel
    {
        public ListCategorsModel()
        {
            EquipmentAttachedList = new List<Equipment>();
        }
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Categor name or number")]
        public string CategorName { get; set; }
        [Display(Name = "Equipments who are inside of this categor")]
        public List<Equipment> EquipmentAttachedList { get; set; }
    }
}
