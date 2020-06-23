using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models
{
    public class User : IdentityUser<int>
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
