using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Vehicles.Models
{
    /// <summary>
    /// Manufacturer Model used when updating an existing Manufacturer is necessary
    /// </summary>
    public class ManufacturerRequestUpdate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
