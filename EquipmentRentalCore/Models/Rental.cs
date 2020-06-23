using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models
{
    public class Rental
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RentalID { get; set; }
        public DateTime RentalStart { get; set; }

        public DateTime RentalEnd { get; set; }

        public int RentalUserID { get; set; }
        public User RentalUser { get; set; }
        public int RentalEquipmentID { get; set; }
        public Equipment RentalEquipment { get; set; }
    }
}
