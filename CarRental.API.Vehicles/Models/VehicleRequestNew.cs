using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Models
{
    public class VehicleRequestNew
    {
        [Required] 
        public string Plate { get; set; }
        [Required] 
        public int VehicleModelId { get; set; }
        [Required] 
        public int Year { get; set; }
    }
}
