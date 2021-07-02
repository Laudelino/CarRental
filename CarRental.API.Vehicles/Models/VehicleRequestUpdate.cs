using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Vehicles.Models
{
    public class VehicleRequestUpdate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Plate { get; set; }
        [Required] 
        public int VehicleModelId { get; set; }
        [Required] 
        public int Year { get; set; }
    }
}
