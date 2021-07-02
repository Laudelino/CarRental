using AutoMapper;
using CarRental.API.Vehicles.DB;
using CarRental.API.Vehicles.Interfaces;
using CarRental.API.Vehicles.Models;
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

        /// <summary>
        /// Add sample data as we don't have a database
        /// </summary>
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

        Task<(bool IsSuccess, string ErrorMessage)> IVehiclesProvider.DeleteVehicleAsync(int Id)
        {
            throw new NotImplementedException();
        }

        Task<(bool IsSuccess, Models.Vehicle Vehicle, string ErrorMessage)> IVehiclesProvider.GetVehicleModelAsync(int Id)
        {
            throw new NotImplementedException();
        }

        Task<(bool IsSuccess, IEnumerable<Models.Vehicle> Vehicles, string ErrorMessage)> IVehiclesProvider.GetVehicleModelsAsync()
        {
            throw new NotImplementedException();
        }

        Task<(bool IsSuccess, Models.Vehicle Vehicle, string ErrorMessage)> IVehiclesProvider.PostVehicleModelAsync(VehicleRequestNew vehicle)
        {
            throw new NotImplementedException();
        }

        Task<(bool IsSuccess, Models.Vehicle Vehicle, string ErrorMessage)> IVehiclesProvider.PutVehicleModelAsync(VehicleRequestUpdate vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
