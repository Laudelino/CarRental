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
    public class VehiclesServiceTest
    {
        [Fact]
        public async Task GetVehiclesReturnsAllVehicles()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehiclesReturnsAllVehicles))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicles(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var vehiclesProvider = new VehiclesProvider(dbContext, null, mapper);

            var vehicles = await vehiclesProvider.GetVehiclesAsync();

            //Checks if call is returning IsSuccess as status
            Assert.True(vehicles.IsSuccess);
            Assert.True(vehicles.Vehicles.Any());
            //Checks that there were no errors
            Assert.Null(vehicles.ErrorMessage);
        }

        [Fact]
        public async Task GetVehicleMReturnsVehicleUsingValidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehicleMReturnsVehicleUsingValidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicles(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var vehiclesProvider = new VehiclesProvider(dbContext, null, mapper);

            var vehicles = await vehiclesProvider.GetVehicleAsync(1);

            //Checks if call is returning IsSuccess as status
            Assert.True(vehicles.IsSuccess);
            Assert.NotNull(vehicles.Vehicle);
            Assert.True(vehicles.Vehicle.Id == 1);
            //Checks that there were no errors
            Assert.Null(vehicles.ErrorMessage);
        }

        [Fact]
        public async Task GetVehicleReturnsVehicleUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehicleReturnsVehicleUsingInvalidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicles(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var vehiclesProvider = new VehiclesProvider(dbContext, null, mapper);

            var vehicles = await vehiclesProvider.GetVehicleAsync(-100);

            //Checks if call is returning IsSuccess as false, due to the ID not existing
            Assert.False(vehicles.IsSuccess);
            //Checks if the object is null as it should
            Assert.Null(vehicles.Vehicle);
            //Checks that we have an error
            Assert.NotNull(vehicles.ErrorMessage);
        }

        [Fact]
        public async Task AddVehicleReturnsVehicle()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(AddVehicleReturnsVehicle))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicles(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var vehiclesProvider = new VehiclesProvider(dbContext, null, mapper);
            Random rand = new Random();

            var newVehicle = new Models.VehicleRequestNew()
            {
                Plate = Guid.NewGuid().ToString(),
                VehicleModelId = rand.Next(1, 4),
                Year = rand.Next(2000, 2022),
            };

            var vehicles = await vehiclesProvider.PostVehicleAsync(newVehicle);


            Assert.True(vehicles.IsSuccess);
            Assert.NotNull(vehicles.Vehicle);
            Assert.True(vehicles.Vehicle.Plate == newVehicle.Plate);
            Assert.Null(vehicles.ErrorMessage);
        }

        [Fact]
        public async Task PutValidIdVehicleReturnsVehicle()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(PutValidIdVehicleReturnsVehicle))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicles(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var vehiclesProvider = new VehiclesProvider(dbContext, null, mapper);
            var allVehicles = await vehiclesProvider.GetVehiclesAsync();
            Random rand = new Random();

            var putVehicle = new Models.VehicleRequestUpdate()
            {
                Id = allVehicles.Vehicles.First().Id,
                Plate = Guid.NewGuid().ToString(),
                VehicleModelId = rand.Next(1, 4),
                Year = rand.Next(2000, 2022),
            };

            var vehicles = await vehiclesProvider.PutVehicleAsync(putVehicle);

            Assert.True(vehicles.IsSuccess);
            Assert.NotNull(vehicles.Vehicle);
            Assert.True(vehicles.Vehicle.Id == putVehicle.Id);
            Assert.True(vehicles.Vehicle.Plate == putVehicle.Plate);
            Assert.Null(vehicles.ErrorMessage);
        }

        [Fact]
        public async Task PutInvalidIdVehiclesReturnsVehicle()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(PutInvalidIdVehiclesReturnsVehicle))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicles(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var vehiclesProvider = new VehiclesProvider(dbContext, null, mapper);


            var putVehicle = new Models.VehicleRequestUpdate() { Id = -100, Plate = Guid.NewGuid().ToString() };

            var vehicles = await vehiclesProvider.PutVehicleAsync(putVehicle);

            Assert.False(vehicles.IsSuccess);
            Assert.Null(vehicles.Vehicle);
            Assert.NotNull(vehicles.ErrorMessage);
        }

        [Fact]
        public async Task DeleteValidIdVehicle()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(DeleteValidIdVehicle))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicles(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var vehiclesProvider = new VehiclesProvider(dbContext, null, mapper);
            var allVehicles = await vehiclesProvider.GetVehiclesAsync();

            var vehicles = await vehiclesProvider.DeleteVehicleAsync(allVehicles.Vehicles.First().Id);

            Assert.True(vehicles.IsSuccess);
            Assert.Null(vehicles.ErrorMessage);
        }

        [Fact]
        public async Task DeleteInvalidIdVehicle()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(DeleteInvalidIdVehicle))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicles(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var vehiclesProvider = new VehiclesProvider(dbContext, null, mapper);

            var vehicles = await vehiclesProvider.DeleteVehicleAsync(-100);

            Assert.False(vehicles.IsSuccess);
            Assert.NotNull(vehicles.ErrorMessage);
        }

        private void CreateVehicles(VehiclesDbContext dbContext)
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

            if (!dbContext.VehicleCategories.Any())
            {
                for (int i = 1; i < 5; i++)
                {
                    dbContext.VehicleCategories.Add(new VehicleCategory()
                    {
                        Id = i,
                        Name = Guid.NewGuid().ToString()
                    });
                }
                dbContext.SaveChanges();
            }

            if (!dbContext.VehicleModels.Any())
            {
                Random rand = new Random();
                for (int i = 1; i < 5; i++)
                {
                    dbContext.VehicleModels.Add(new VehicleModel()
                    {
                        Id = i,
                        Name = Guid.NewGuid().ToString(),
                        FuelTypeId = rand.Next(1, 4),
                        ManufacturerId = rand.Next(1, 4),
                        RentalRate = (decimal)rand.NextDouble(),
                        TrunkSize = rand.Next(1000),
                        VehicleCategoryId = rand.Next(1, 4)
                    });
                }
                dbContext.SaveChanges();
            }

            if (!dbContext.Vehicles.Any())
            {
                Random rand = new Random();
                for (int i = 1; i < 5; i++)
                {
                    dbContext.Vehicles.Add(new DB.Vehicle()
                    {
                        Id = i,
                        Plate = Guid.NewGuid().ToString(),
                        VehicleModelId = rand.Next(1,4),
                        Year = rand.Next(2000, 2022)
                    });
                }
                dbContext.SaveChanges();
            }
        }


    }
}
