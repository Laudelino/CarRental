using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Vehicles.Models
{
    /// <summary>
    /// Manufacturer Model for receiving new Manufacturers 
    /// </summary>
    public class ManufacturerRequestNew
    {
        [Required]
        public string Name { get; set; }
    }
}
