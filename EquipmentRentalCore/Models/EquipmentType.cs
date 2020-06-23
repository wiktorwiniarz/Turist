using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models
{
    public class EquipmentType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int TypeID { get; set; }
        public string TypeName { get; set; }

        public ICollection<Equipment> Equipments { get; set; }
    }
}
