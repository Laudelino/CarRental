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
    public class VehiclesProvider : IVehiclesProvider
    {
        private readonly VehiclesDbContext dbContext;
        private readonly ILogger<VehiclesProvider> logger;
        private readonly IMapper mapper;

        public VehiclesProvider(VehiclesDbContext dBContext, ILogger<VehiclesProvider> logger, IMapper mapper)
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
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 1, Plate = "ABC-1234", VehicleModelId = 1, Year = 2022 });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 2, Plate = "DEF-5678", VehicleModelId = 2, Year = 2010 });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 3, Plate = "GHI-9012", VehicleModelId = 3, Year = 2020 });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 4, Plate = "BRA2E19", VehicleModelId = 4, Year = 2021 });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 5, Plate = "QTP5F71", VehicleModelId = 5, Year = 2018 });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 6, Plate = "MSK9B10", VehicleModelId = 6, Year = 2019 });
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.Vehicle> Vehicles, string ErrorMessage)> GetVehiclesAsync()
        {
            try
            {
                var vehicles = await dbContext.Vehicles.Include(m => m.VehicleModel).Include(f => f.VehicleModel.FuelType).Include(m => m.VehicleModel.Manufacturer).Include(c => c.VehicleModel.VehicleCategory).ToListAsync();
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
        public async Task<(bool IsSuccess, IEnumerable<Models.Vehicle> Vehicles, string ErrorMessage)> GetVehiclesNotReservedAsync()
        {
            try
            {
                var vehicles = await dbContext.Vehicles.Include(m => m.VehicleModel).Include(f => f.VehicleModel.FuelType).Include(m => m.VehicleModel.Manufacturer).Include(c => c.VehicleModel.VehicleCategory).Where(r => r.IsReserved == false).ToListAsync();
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
        public async Task<(bool IsSuccess, Models.Vehicle Vehicle, string ErrorMessage)> GetVehicleAsync(int Id)
        {
            try
            {
                var vehicle = await dbContext.Vehicles.Include(m => m.VehicleModel).Include(f => f.VehicleModel.FuelType).Include(m => m.VehicleModel.Manufacturer).Include(c => c.VehicleModel.VehicleCategory).FirstOrDefaultAsync(m => m.Id == Id);

                if (vehicle != null)
                {
                    var result = mapper.Map<DB.Vehicle, Models.Vehicle>(vehicle);
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
        public async Task<(bool IsSuccess, Models.Vehicle Vehicle, string ErrorMessage)> PostVehicleAsync(VehicleRequestNew vehicle)
        {
            try
            {
                var dbvehicle = new DB.Vehicle() 
                { 
                    Plate = vehicle.Plate,
                    VehicleModelId = vehicle.VehicleModelId,
                    Year = vehicle.Year
                };
                dbContext.Add(dbvehicle);

                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    var resultVehicle = await GetVehicleAsync(dbvehicle.Id);

                    if (resultVehicle.IsSuccess)
                    {
                        return (true, resultVehicle.Vehicle, null);
                    }
                }
                return (false, null, "Failed to create the record");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, Models.Vehicle Vehicle, string ErrorMessage)> PutVehicleAsync(VehicleRequestUpdate vehicle)
        {
            try
            {
                var find = await this.GetVehicleAsync(vehicle.Id);

                if (find.IsSuccess)
                {
                    var dbvehicle = await dbContext.Vehicles.FirstOrDefaultAsync(c => c.Id == vehicle.Id);
                    
                    dbvehicle.Plate = vehicle.Plate;
                    dbvehicle.VehicleModelId = vehicle.VehicleModelId;
                    dbvehicle.Year = vehicle.Year;

                    dbContext.Update(dbvehicle);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var resultVehicle = await GetVehicleAsync(dbvehicle.Id);

                        if (resultVehicle.IsSuccess)
                        {
                            return (true, resultVehicle.Vehicle, null);
                        }
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
        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteVehicleAsync(int Id)
        {
            try
            {
                var find = await this.GetVehicleAsync(Id);

                if (find.IsSuccess)
                {
                    var dbvehicle = await dbContext.Vehicles.FirstOrDefaultAsync(c => c.Id == Id);

                    dbContext.Remove(dbvehicle);

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
        public async Task<(bool IsSuccess, Models.Vehicle Vehicle, string ErrorMessage)> PutChangeReserveVehicleAsync(VehicleReserveRequest reserveRequest)
        {
            try
            {
                var find = await this.GetVehicleAsync(reserveRequest.Id);

                if(!find.IsSuccess)
                    return (false, null, "Failed to find record");

                if (find.IsSuccess)
                {
                    var dbvehicle = await dbContext.Vehicles.FirstOrDefaultAsync(c => c.Id == reserveRequest.Id);

                    dbvehicle.IsReserved = reserveRequest.IsReserved;

                    dbContext.Update(dbvehicle);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var resultVehicle = await GetVehicleAsync(dbvehicle.Id);

                        if (resultVehicle.IsSuccess)
                        {
                            return (true, resultVehicle.Vehicle, null);
                        }
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
        public async Task<(bool IsSuccess, Models.Vehicle Vehicle, string ErrorMessage)> PutReserveVehicleByModelAsync(int id)
        {
            try
            {
                var vehicle = await dbContext.Vehicles.Include(m => m.VehicleModel).Include(f => f.VehicleModel.FuelType).Include(m => m.VehicleModel.Manufacturer).Include(c => c.VehicleModel.VehicleCategory).Where(m => m.VehicleModel.Id == id).Where(v => v.IsReserved == false).FirstOrDefaultAsync();

                if (vehicle != null)
                {
                    var dbvehicle = await dbContext.Vehicles.FirstOrDefaultAsync(c => c.Id == vehicle.Id);

                    dbvehicle.IsReserved = true;

                    dbContext.Update(dbvehicle);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var resultVehicle = await GetVehicleAsync(dbvehicle.Id);

                        if (resultVehicle.IsSuccess)
                        {
                            return (true, resultVehicle.Vehicle, null);
                        }
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
    }
}
