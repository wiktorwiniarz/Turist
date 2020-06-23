using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Models
{
    public class Group : IdentityRole<int>
    {
        public int RoleId { get; set; }
    }
}
