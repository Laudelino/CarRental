using CarRental.API.Vehicles.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Interfaces
{
    public interface IFuelTypesProvider
    {
        Task<(bool IsSuccess, IEnumerable<FuelType> FuelTypes, string ErrorMessage)> GetFuelTypesAsync();
        Task<(bool IsSuccess, FuelType FuelType, string ErrorMessage)> GetFuelTypeAsync(int Id);
        Task<(bool IsSuccess, IEnumerable<VehicleModel> VehicleModels, string ErrorMessage)> GetVehicleModelsByFuelTypeAsync(int id);
    }
}
