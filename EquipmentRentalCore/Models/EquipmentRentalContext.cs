using EquipmentRentalCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentRentalCore.Models.CategorViewModels;

namespace EquipmentRentalCore.Models
{
    public class EquipmentRentalContext : IdentityDbContext<User, Group, int>
    {
        public EquipmentRentalContext(DbContextOptions<EquipmentRentalContext> options) : base(options)
        {

        }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Categor> Categors { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Group>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Rental>()
                .HasOne(x => x.RentalUser)
                .WithMany(u => u.Rentals)
                .HasForeignKey(x => x.RentalUserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(x => x.RentalEquipment)
                .WithOne(e => e.Rental);

            modelBuilder.Entity<Equipment>()
                .HasOne(x => x.EquipmentType)
                .WithMany(t => t.Equipments)
                .HasForeignKey(x => x.EquipmentTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Equipment>()
                .HasOne(x => x.Categor)
                .WithMany(r => r.Equipments)
                .HasForeignKey(x => x.CategorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
