using AutoMapper;
using CarRental.API.Vehicles.DB;
using CarRental.API.Vehicles.Profiles;
using CarRental.API.Vehicles.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace CarRental.API.Vehicles.Tests
{
    public class FuelTypesServiceTest
    {
        [Fact]
        public async Task GetFuelTypesReturnsAllFuelTypes()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetFuelTypesReturnsAllFuelTypes))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateFuelTypes(dbContext);

            var fueltypeProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(fueltypeProfile));
            var mapper = new Mapper(config);
            var fueltypesProvider = new FuelTypesProvider(dbContext, null, mapper);

            var fuelTypes = await fueltypesProvider.GetFuelTypesAsync();

            //Checks if call is returning IsSuccess as status
            Assert.True(fuelTypes.IsSuccess);
            Assert.True(fuelTypes.FuelTypes.Any());
            //Checks that there were no errors
            Assert.Null(fuelTypes.ErrorMessage);
        }

        [Fact]
        public async Task GetFuelTypeReturnsFuelTypeUsingValidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetFuelTypeReturnsFuelTypeUsingValidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateFuelTypes(dbContext);

            var fueltypeProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(fueltypeProfile));
            var mapper = new Mapper(config);
            var fueltypesProvider = new FuelTypesProvider(dbContext, null, mapper);

            var fuelType = await fueltypesProvider.GetFuelTypeAsync(1);

            //Checks if call is returning IsSuccess as status
            Assert.True(fuelType.IsSuccess);
            Assert.NotNull(fuelType.FuelType);
            Assert.True(fuelType.FuelType.Id == 1);
            //Checks that there were no errors
            Assert.Null(fuelType.ErrorMessage);
        }

        [Fact]
        public async Task GetFuelTypeReturnsFuelTypeUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetFuelTypeReturnsFuelTypeUsingInvalidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateFuelTypes(dbContext);

            var fueltypeProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(fueltypeProfile));
            var mapper = new Mapper(config);
            var fueltypesProvider = new FuelTypesProvider(dbContext, null, mapper);

            var fuelType = await fueltypesProvider.GetFuelTypeAsync(-100);

            //Checks if call is returning IsSuccess as false, due to the ID not existing
            Assert.False(fuelType.IsSuccess);
            //Checks if the object is null as it should
            Assert.Null(fuelType.FuelType);
            //Checks that we have an error
            Assert.NotNull(fuelType.ErrorMessage);
        }
        private void CreateFuelTypes(VehiclesDbContext dbContext)
        {
            if (!dbContext.FuelTypes.Any())
            {
                for (int i = 1; i < 5; i++)
                {
                    dbContext.FuelTypes.Add(new FuelType()
                    {
                        Id = i,
                        Name = Guid.NewGuid().ToString()
                    });
                }
                dbContext.SaveChanges();
            }
        }
    }
}
