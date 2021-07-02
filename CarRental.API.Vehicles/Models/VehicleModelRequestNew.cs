using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Vehicles.Models
{
    public class VehicleModelRequestNew
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int ManufacturerId { get; set; }
        [Required]
        public decimal RentalRate { get; set; }
        [Required] 
        public int FuelTypeId { get; set; }
        [Required] 
        public int TrunkSize { get; set; }
        [Required] 
        public int VehicleCategoryId { get; set; }
    }
}
