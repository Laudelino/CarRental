using CarRental.API.Vehicles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Interfaces
{
    public interface IVehicleCategoriesProvider
    {
        Task<(bool IsSuccess, IEnumerable<VehicleCategory> VehicleCategories, string ErrorMessage)> GetVehicleCategoriesAsync();
        Task<(bool IsSuccess, Models.VehicleCategory VehicleCategory, string ErrorMessage)> GetVehicleCategoryAsync(int id);
        Task<(bool IsSuccess, IEnumerable<VehicleModel> VehicleModels, string ErrorMessage)> GetVehicleModelsByCategoryAsync(int id);
    }
}
