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
    public class ManufacturersServiceTest
    {
        [Fact]
        public async Task GetManufacturersReturnsAllManufacturers()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetManufacturersReturnsAllManufacturers))
                .Options;
            var dbContext = new VehiclesDbContext(options);
            
            CreateManufacurers(dbContext);

            var manufacturerProfile = new ManufacturerProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);

            var manufacuters = await manufacturersProvider.GetManufacturersAsync();
            
            //Checks if call is returning IsSuccess as status
            Assert.True(manufacuters.IsSuccess);
            //Checks if we have any manufacturer
            Assert.True(manufacuters.Manufacturers.Any());
            //Checks that there were no errors
            Assert.Null(manufacuters.ErrorMessage);
        }

        [Fact]
        public async Task GetManufacturersReturnsManufacturerUsingValidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetManufacturersReturnsManufacturerUsingValidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateManufacurers(dbContext);

            var manufacturerProfile = new ManufacturerProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);

            var manufacuter = await manufacturersProvider.GetManufacturerAsync(1);

            //Checks if call is returning IsSuccess as status
            Assert.True(manufacuter.IsSuccess);
            //Checks if we got the right Manufacturer
            Assert.NotNull(manufacuter.Manufacturer);
            //Checks if we got the right Manufacturer
            Assert.True(manufacuter.Manufacturer.Id==1);
            //Checks that there were no errors
            Assert.Null(manufacuter.ErrorMessage);
        }

        [Fact]
        public async Task GetManufacturersReturnsManufacturerUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetManufacturersReturnsManufacturerUsingValidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateManufacurers(dbContext);

            var manufacturerProfile = new ManufacturerProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);

            var manufacuter = await manufacturersProvider.GetManufacturerAsync(-100);

            //Checks if call is returning IsSuccess as false, due to the ID not existing
            Assert.False(manufacuter.IsSuccess);
            //Checks if the object is null as it should
            Assert.Null(manufacuter.Manufacturer);
            //Checks that we have an error
            Assert.NotNull(manufacuter.ErrorMessage);
        }

        private void CreateManufacurers(VehiclesDbContext dbContext)
        {
            for (int i = 1; i < 5; i++)
            {
                dbContext.Manufacturers.Add(new Manufacturer()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString()
                });
            }
            dbContext.SaveChanges();
        }
    }
}
