using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Models
{
    /// <summary>
    /// Model the entity for the Manufacturer - Marca
    /// Returned by the provider
    /// </summary>
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
