using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.DB
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public int VehicleModelId { get; set; }
        public virtual VehicleModel VehicleModel { get; set; }
        public int Year { get; set; }
    }
}
