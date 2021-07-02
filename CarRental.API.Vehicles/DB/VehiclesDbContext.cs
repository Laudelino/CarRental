using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.DB
{
    /// <summary>
    /// Responsible for everything related to data
    /// </summary>
    public class VehiclesDbContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<VehicleCategory> VehicleCategories { get; set; }

        public DbSet<FuelType> FuelTypes { get; set; }

        public VehiclesDbContext(DbContextOptions options) : base(options)
        { 
            
        }
    }
}
