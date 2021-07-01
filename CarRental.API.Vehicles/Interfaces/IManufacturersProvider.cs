using CarRental.API.Vehicles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Interfaces
{
    public interface IManufacturersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Manufacturer> Manufacturers, string ErrorMessage)> GetManufacturersAsync();
        Task<(bool IsSuccess, Manufacturer Manufacturer, string ErrorMessage)> GetManufacturerAsync(int Id);
    }
}
