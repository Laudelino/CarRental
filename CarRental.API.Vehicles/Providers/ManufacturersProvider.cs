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

        public async Task<(bool IsSuccess, IEnumerable<Models.Manufacturer> Manufacturers, string ErrorMessage)> GetManufacturersAsync()
        {
            try
            {
                var manufacturers = await dbContext.Manufacturers.ToListAsync();
                if(manufacturers != null && manufacturers.Any())
                {
                    var result = mapper.Map<IEnumerable<DB.Manufacturer>, IEnumerable<Models.Manufacturer>>(manufacturers);
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

        public async Task<(bool IsSuccess, Models.Manufacturer Manufacturer, string ErrorMessage)> GetManufacturerAsync(int Id)
        {
            try
            {
                var manufacturer = await dbContext.Manufacturers.FirstOrDefaultAsync(m => m.Id == Id);
                
                if(manufacturer != null)
                {
                    var result = mapper.Map<DB.Manufacturer, Models.Manufacturer>(manufacturer);
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

        public async Task<(bool IsSuccess, Models.Manufacturer Manufacturer, string ErrorMessage)> PostManufacturerAsync(ManufacturerRequestNew manufacturer)
        {
            try
            {
                var dbmanufacturer = new DB.Manufacturer() { Name = manufacturer.Name };
                dbContext.Add(dbmanufacturer);

                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    var resultmanufacturer = mapper.Map<DB.Manufacturer, Models.Manufacturer>(dbmanufacturer);
                    return (true, resultmanufacturer, null);
                }
                return (false, null, "Failed to create the record");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Manufacturer Manufacturer, string ErrorMessage)> PutManufacturerAsync(ManufacturerRequestUpdate manufacturer)
        {
            try
            {
                var find = await this.GetManufacturerAsync(manufacturer.Id);

                if(find.IsSuccess)
                {
                    var dbmanufacturer = await dbContext.Manufacturers.FirstOrDefaultAsync(c => c.Id == manufacturer.Id);
                    dbmanufacturer.Name = manufacturer.Name;    
                    
                    dbContext.Update(dbmanufacturer);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var resultmanufacturer = mapper.Map<DB.Manufacturer, Models.Manufacturer>(dbmanufacturer);
                        return (true, resultmanufacturer, null);
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

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteManufacturerAsync(int Id)
        {
            try
            {
                var find = await this.GetManufacturerAsync(Id);

                if (find.IsSuccess)
                {
                    var dbmanufacturer = await dbContext.Manufacturers.FirstOrDefaultAsync(c => c.Id == Id);
   
                    dbContext.Remove(dbmanufacturer);

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
