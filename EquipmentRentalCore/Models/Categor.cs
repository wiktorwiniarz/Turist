using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models
{
    public class Categor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
    }
}
