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

            var manufacturerProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);

            var manufacturers = await manufacturersProvider.GetManufacturersAsync();
            
            //Checks if call is returning IsSuccess as status
            Assert.True(manufacturers.IsSuccess);
            //Checks if we have any manufacturer
            Assert.True(manufacturers.Manufacturers.Any());
            //Checks that there were no errors
            Assert.Null(manufacturers.ErrorMessage);
        }

        [Fact]
        public async Task GetManufacturersReturnsManufacturerUsingValidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetManufacturersReturnsManufacturerUsingValidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateManufacurers(dbContext);

            var manufacturerProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);

            var manufacturer = await manufacturersProvider.GetManufacturerAsync(1);

            //Checks if call is returning IsSuccess as status
            Assert.True(manufacturer.IsSuccess);
            //Checks if we got the right Manufacturer
            Assert.NotNull(manufacturer.Manufacturer);
            //Checks if we got the right Manufacturer
            Assert.True(manufacturer.Manufacturer.Id==1);
            //Checks that there were no errors
            Assert.Null(manufacturer.ErrorMessage);
        }

        [Fact]
        public async Task GetManufacturersReturnsManufacturerUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetManufacturersReturnsManufacturerUsingInvalidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateManufacurers(dbContext);

            var manufacturerProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);

            var manufacturer = await manufacturersProvider.GetManufacturerAsync(-100);

            //Checks if call is returning IsSuccess as false, due to the ID not existing
            Assert.False(manufacturer.IsSuccess);
            //Checks if the object is null as it should
            Assert.Null(manufacturer.Manufacturer);
            //Checks that we have an error
            Assert.NotNull(manufacturer.ErrorMessage);
        }

        [Fact]
        public async Task AddManufacturersReturnsManufacturer()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(AddManufacturersReturnsManufacturer))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateManufacurers(dbContext);

            var manufacturerProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);

            var newManufacturer = new Models.ManufacturerRequestNew() { Name = Guid.NewGuid().ToString() };

            var manufacturer = await manufacturersProvider.PostManufacturerAsync(newManufacturer);

            
            Assert.True(manufacturer.IsSuccess);
            Assert.NotNull(manufacturer.Manufacturer);
            Assert.True(manufacturer.Manufacturer.Name == newManufacturer.Name);
            Assert.Null(manufacturer.ErrorMessage);
        }

        [Fact]
        public async Task PutValidIdManufacturersReturnsManufacturer()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(PutValidIdManufacturersReturnsManufacturer))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            //CreateManufacurers(dbContext);

            var manufacturerProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);
            var allManufacturers = await manufacturersProvider.GetManufacturersAsync();

            var putManufacturer = new Models.ManufacturerRequestUpdate() { Id = allManufacturers.Manufacturers.First().Id, Name = Guid.NewGuid().ToString() };

            var manufacturer = await manufacturersProvider.PutManufacturerAsync(putManufacturer);

            Assert.True(manufacturer.IsSuccess);
            Assert.NotNull(manufacturer.Manufacturer);
            Assert.True(manufacturer.Manufacturer.Id == putManufacturer.Id);
            Assert.True(manufacturer.Manufacturer.Name == putManufacturer.Name);
            Assert.Null(manufacturer.ErrorMessage);
        }

        [Fact]
        public async Task PutInvalidIdManufacturersReturnsManufacturer()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(PutInvalidIdManufacturersReturnsManufacturer))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateManufacurers(dbContext);

            var manufacturerProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);
            
            
            var putManufacturer = new Models.ManufacturerRequestUpdate() { Id = -100, Name = Guid.NewGuid().ToString() };

            var manufacturer = await manufacturersProvider.PutManufacturerAsync(putManufacturer);

            Assert.False(manufacturer.IsSuccess);
            Assert.Null(manufacturer.Manufacturer);
            Assert.NotNull(manufacturer.ErrorMessage);
        }

        [Fact]
        public async Task DeleteValidIdManufacturer()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(DeleteValidIdManufacturer))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateManufacurers(dbContext);

            var manufacturerProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);
            var allManufacturers = await manufacturersProvider.GetManufacturersAsync();

            var manufacturer = await manufacturersProvider.DeleteManufacturerAsync(allManufacturers.Manufacturers.First().Id);

            Assert.True(manufacturer.IsSuccess);
            Assert.Null(manufacturer.ErrorMessage);
        }

        [Fact]
        public async Task DeleteInvalidIdManufacturer()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(DeleteInvalidIdManufacturer))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateManufacurers(dbContext);

            var manufacturerProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(manufacturerProfile));
            var mapper = new Mapper(config);
            var manufacturersProvider = new ManufacturersProvider(dbContext, null, mapper);

            var manufacturer = await manufacturersProvider.DeleteManufacturerAsync(-100);

            Assert.False(manufacturer.IsSuccess);
            Assert.NotNull(manufacturer.ErrorMessage);
        }

        private void CreateManufacurers(VehiclesDbContext dbContext)
        {
            if (!dbContext.Manufacturers.Any())
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
}
