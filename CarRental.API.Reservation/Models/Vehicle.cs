using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public virtual VehicleModel VehicleModel { get; set; }
        public int Year { get; set; }
    }
}
