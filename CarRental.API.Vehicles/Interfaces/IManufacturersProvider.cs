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
        Task<(bool IsSuccess, Models.Manufacturer Manufacturer, string ErrorMessage)> GetManufacturerAsync(int Id);
        Task<(bool IsSuccess, IEnumerable<VehicleModel> VehicleModels, string ErrorMessage)> GetVehicleModelsByManufacturerAsync(int id);
        Task<(bool IsSuccess, Models.Manufacturer Manufacturer, string ErrorMessage)> PostManufacturerAsync(ManufacturerRequestNew manufacturer);
        Task<(bool IsSuccess, Models.Manufacturer Manufacturer, string ErrorMessage)> PutManufacturerAsync(ManufacturerRequestUpdate manufacturer);
        Task<(bool IsSuccess, string ErrorMessage)> DeleteManufacturerAsync(int Id);

    }
}
