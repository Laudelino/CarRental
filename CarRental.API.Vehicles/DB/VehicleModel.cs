using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.DB
{
    /// <summary>
    /// Model the entity for the VehicleModel - Modelo
    /// Used for database access
    /// </summary>
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int TrunkSize { get; set; }
        public decimal RentalRate { get; set; }
    }
}
