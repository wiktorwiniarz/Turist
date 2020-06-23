using EquipmentRentalCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentRentalCore.Extensions
{
    public static class DBGroupsInitializer
    {
        public static void OnInitialize(EquipmentRentalContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Roles.Any())
            {
                context.Roles.Add(new Group
                {
                    Name = "User",
                    NormalizedName = "USER"
                });
                context.Roles.Add(new Group
                {
                    Name = "Service",
                    NormalizedName = "SERVICE"
                });
                context.Roles.Add(new Group
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINSTRATOR"
                });
                context.SaveChanges();
            }
        }
    }
}
