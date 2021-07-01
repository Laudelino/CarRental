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
    public class ManufacturersProvider : IManufacturersProvider
    {
        private readonly VehiclesDbContext dbContext;
        private readonly ILogger<ManufacturersProvider> logger;
        private readonly IMapper mapper;

        public ManufacturersProvider(VehiclesDbContext dBContext, ILogger<ManufacturersProvider> logger, IMapper mapper)
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
            if (!dbContext.Manufacturers.Any())
            {
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 1, Name = "Volkswagen" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 2, Name = "Fiat" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 3, Name = "Ford" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 4, Name = "Kia" });
                dbContext.SaveChanges();
            }
        }

        public Task<(bool IsSuccess, IEnumerable<Models.Manufacturer> Manufacturers, string ErrorMessage)> GetManufacturersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(bool IsSuccess, Models.Manufacturer Manufacturer, string ErrorMessage)> GetManufacturerAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
