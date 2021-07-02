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
    public class VehicleCategoriesProvider : IVehicleCategoriesProvider
    {
        private readonly VehiclesDbContext dbContext;
        private readonly ILogger<VehicleCategoriesProvider> logger;
        private readonly IMapper mapper;

        public VehicleCategoriesProvider(VehiclesDbContext dBContext, ILogger<VehicleCategoriesProvider> logger, IMapper mapper)
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
            if (!dbContext.VehicleCategories.Any())
            {
                dbContext.VehicleCategories.Add(new DB.VehicleCategory() { Id = 1, Name = "Básico" });
                dbContext.VehicleCategories.Add(new DB.VehicleCategory() { Id = 2, Name = "Completo" });
                dbContext.VehicleCategories.Add(new DB.VehicleCategory() { Id = 3, Name = "Luxo" });
                
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.VehicleCategory> VehicleCategories, string ErrorMessage)> GetVehicleCategoriesAsync()
        {
            try
            {
                var categories = await dbContext.VehicleCategories.ToListAsync();
                if (categories != null && categories.Any())
                {
                    var result = mapper.Map<IEnumerable<DB.VehicleCategory>, IEnumerable<Models.VehicleCategory>>(categories);
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

        public async Task<(bool IsSuccess, Models.VehicleCategory VehicleCategory, string ErrorMessage)> GetVehicleCategoryAsync(int Id)
        {
            try
            {
                var categories = await dbContext.VehicleCategories.FirstOrDefaultAsync(c => c.Id == Id);

                if (categories != null)
                {
                    var result = mapper.Map<DB.VehicleCategory, Models.VehicleCategory>(categories);
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
