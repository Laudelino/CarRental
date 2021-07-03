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
    public class VehicleModelsProvider : IVehicleModelsProvider
    {
        private readonly VehiclesDbContext dbContext;
        private readonly ILogger<VehicleModelsProvider> logger;
        private readonly IMapper mapper;

        public VehicleModelsProvider(VehiclesDbContext dBContext, ILogger<VehicleModelsProvider> logger, IMapper mapper)
        {
            this.dbContext = dBContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
        private void SeedData()
        {
            if (!dbContext.VehicleModels.Any())
            {
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 1, Name = "Uno 1.0",               FuelTypeId = 1, ManufacturerId = 2, RentalRate = 9.5M,  TrunkSize = 200, VehicleCategoryId = 1 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 2, Name = "Gol 1.0",               FuelTypeId = 2, ManufacturerId = 1, RentalRate = 9.5M,  TrunkSize = 200, VehicleCategoryId = 1 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 3, Name = "Virtus Comfortline",    FuelTypeId = 1, ManufacturerId = 1, RentalRate = 15.5M, TrunkSize = 200, VehicleCategoryId = 2 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 4, Name = "Ecosport 1.5",          FuelTypeId = 2, ManufacturerId = 3, RentalRate = 16.5M, TrunkSize = 500, VehicleCategoryId = 2 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 5, Name = "Cruze Sedan FAST",      FuelTypeId = 2, ManufacturerId = 4, RentalRate = 18.5M, TrunkSize = 300, VehicleCategoryId = 3 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 6, Name = "S10 2.8",               FuelTypeId = 3, ManufacturerId = 4, RentalRate = 17.5M, TrunkSize = 500, VehicleCategoryId = 3 });
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.VehicleModel> VehicleModels, string ErrorMessage)> GetVehicleModelsAsync()
        {
            try
            {
                var vehicleModels = await dbContext.VehicleModels.Include(f => f.FuelType).Include(m => m.Manufacturer).Include(c => c.VehicleCategory).ToListAsync();
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

        public async Task<(bool IsSuccess, Models.VehicleModel VehicleModel, string ErrorMessage)> GetVehicleModelAsync(int Id)
        {
            try
            {
                var vehicleModel = await dbContext.VehicleModels.Include(f => f.FuelType).Include(m => m.Manufacturer).Include(c => c.VehicleCategory).FirstOrDefaultAsync(m => m.Id == Id);

                if (vehicleModel != null)
                {
                    var result = mapper.Map<DB.VehicleModel, Models.VehicleModel>(vehicleModel);
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

        public async Task<(bool IsSuccess, IEnumerable<Models.Vehicle> Vehicles, string ErrorMessage)> GetVehiclesByModelAsync(int id)
        {
            try
            {
                var vehicles = await dbContext.Vehicles.Include(m => m.VehicleModel).Include(f => f.VehicleModel.FuelType).Include(m => m.VehicleModel.Manufacturer).Include(c => c.VehicleModel.VehicleCategory).Where(m => m.VehicleModel.Id == id).ToListAsync();
                if (vehicles != null && vehicles.Any())
                {
                    var result = mapper.Map<IEnumerable<DB.Vehicle>, IEnumerable<Models.Vehicle>>(vehicles);
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

        public async Task<(bool IsSuccess, Models.VehicleModel VehicleModel, string ErrorMessage)> PostVehicleModelAsync(VehicleModelRequestNew vehiclemodel)
        {
            try
            {
                var dbvehicleModel = new DB.VehicleModel() { Name = vehiclemodel.Name };
                dbContext.Add(dbvehicleModel);

                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    var resultVehicleModel = mapper.Map<DB.VehicleModel, Models.VehicleModel>(dbvehicleModel);
                    return (true, resultVehicleModel, null);
                }
                return (false, null, "Failed to create the record");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.VehicleModel VehicleModel, string ErrorMessage)> PutVehicleModelAsync(VehicleModelRequestUpdate vehiclemodel)
        {
            try
            {
                var find = await this.GetVehicleModelAsync(vehiclemodel.Id);

                if (find.IsSuccess)
                {
                    var dbvehicleModel = await dbContext.VehicleModels.FirstOrDefaultAsync(c => c.Id == vehiclemodel.Id);
                    dbvehicleModel.Name = vehiclemodel.Name;

                    dbContext.Update(dbvehicleModel);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var resultvehiclemodel = mapper.Map<DB.VehicleModel, Models.VehicleModel>(dbvehicleModel);
                        return (true, resultvehiclemodel, null);
                    }
                    return (false, null, "Failed to update the record");
                }
                return (false, null, "Failed to find record");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteVehicleModelAsync(int Id)
        {
            try
            {
                var find = await this.GetVehicleModelAsync(Id);

                if (find.IsSuccess)
                {
                    var dbvehicleModel = await dbContext.VehicleModels.FirstOrDefaultAsync(c => c.Id == Id);

                    dbContext.Remove(dbvehicleModel);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        return (true, null);
                    }
                    return (false, "Failed to delete the record");
                }
                return (false, "Failed to find record");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, ex.Message);
            }
        }
    }
}
