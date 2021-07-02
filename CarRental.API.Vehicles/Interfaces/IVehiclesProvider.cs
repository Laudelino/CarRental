using CarRental.API.Vehicles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Interfaces
{
    public interface IVehiclesProvider
    {
        Task<(bool IsSuccess, IEnumerable<Vehicle> Vehicles, string ErrorMessage)> GetVehicleModelsAsync();
        Task<(bool IsSuccess, Vehicle Vehicle, string ErrorMessage)> GetVehicleModelAsync(int Id);
        Task<(bool IsSuccess, Vehicle Vehicle, string ErrorMessage)> PostVehicleModelAsync(VehicleRequestNew vehicle);
        Task<(bool IsSuccess, Vehicle Vehicle, string ErrorMessage)> PutVehicleModelAsync(VehicleRequestUpdate vehicle);
        Task<(bool IsSuccess, string ErrorMessage)> DeleteVehicleAsync(int Id);
    }
}
