using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models.EquipmentTypeViewModel
{
    public class ListEquipmentTypeViewModel
    {
        public ListEquipmentTypeViewModel()
        {

        }
        public ListEquipmentTypeViewModel(int id, string typeName, List<Equipment> attached)
        {
            Id = id;
            Name = typeName;
            EquipmentsAttachedList = attached;
        }
        public int Id { get; set; }
        [Display(Name = "Equipment type name")]
        public string Name { get; set; }
        [Display(Name = "Equipments attached")]
        public List<Equipment> EquipmentsAttachedList { get; set; }
    }
}
