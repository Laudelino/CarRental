using AutoMapper;
using CarRental.API.Vehicles.DB;
using CarRental.API.Vehicles.Interfaces;
using CarRental.API.Vehicles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Providers
{
    public class FuelTypesProvider : IFuelTypesProvider
    {
        private readonly VehiclesDbContext dbContext;
        private readonly ILogger<FuelTypesProvider> logger;
        private readonly IMapper mapper;

        public FuelTypesProvider(VehiclesDbContext dBContext, ILogger<FuelTypesProvider> logger, IMapper mapper)
        {
            this.dbContext = dBContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.FuelTypes.Any())
            {
                dbContext.FuelTypes.Add(new DB.FuelType() { Id = 1, Name = "Gasolina" });
                dbContext.FuelTypes.Add(new DB.FuelType() { Id = 2, Name = "Álcool" });
                dbContext.FuelTypes.Add(new DB.FuelType() { Id = 3, Name = "Diesel" });

                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.FuelType> FuelTypes, string ErrorMessage)> GetFuelTypesAsync()
        {
            try
            {
                var fuelTypes = await dbContext.FuelTypes.ToListAsync();
                if (fuelTypes != null && fuelTypes.Any())
                {
                    var result = mapper.Map<IEnumerable<DB.FuelType>, IEnumerable<Models.FuelType>>(fuelTypes);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.FuelType FuelType, string ErrorMessage)> GetFuelTypeAsync(int id)
        {
            try
            {
                var fuelTypes = await dbContext.FuelTypes.FirstOrDefaultAsync(f => f.Id == id);

                if (fuelTypes != null)
                {
                    var result = mapper.Map<DB.FuelType, Models.FuelType>(fuelTypes);
                    return (true, result, null);
                }
                return (false, null, "Not Found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.VehicleModel> VehicleModels, string ErrorMessage)> GetVehicleModelsByFuelTypeAsync(int id)
        {
            try
            {
                var vehicleModels = await dbContext.VehicleModels.Include(f => f.FuelType).Include(m => m.Manufacturer).Include(c => c.VehicleCategory).Where(f => f.FuelType.Id==id).ToListAsync();
                if (vehicleModels != null && vehicleModels.Any())
                {
                    var result = mapper.Map<IEnumerable<DB.VehicleModel>, IEnumerable<Models.VehicleModel>>(vehicleModels);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
