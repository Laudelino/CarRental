using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public decimal RentalRate { get; set; }
        public virtual FuelType FuelType { get; set; } 
        public int TrunkSize { get; set; }
        public virtual VehicleCategory VehicleCategory { get; set; }
    }
}
