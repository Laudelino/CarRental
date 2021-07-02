using CarRental.API.Vehicles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Interfaces
{
    public interface IVehicleModelsProvider
    {
        Task<(bool IsSuccess, IEnumerable<VehicleModel> VehicleModels, string ErrorMessage)> GetVehicleModelsAsync();
        Task<(bool IsSuccess, VehicleModel VehicleModel, string ErrorMessage)> GetVehicleModelAsync(int Id);
        Task<(bool IsSuccess, VehicleModel VehicleModel, string ErrorMessage)> PostVehicleModelAsync(VehicleModelRequestNew vehiclemodel);
        Task<(bool IsSuccess, VehicleModel VehicleModel, string ErrorMessage)> PutVehicleModelAsync(VehicleModelRequestUpdate vehiclemodel);
        Task<(bool IsSuccess, string ErrorMessage)> DeleteVehicleModelAsync(int Id);
    }
}
