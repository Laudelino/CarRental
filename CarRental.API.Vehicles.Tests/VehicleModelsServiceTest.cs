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
    public class VehicleModelsServiceTest
    {
        [Fact]
        public async Task GetVehicleModelsReturnsAllVehicleModels()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehicleModelsReturnsAllVehicleModels))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleModels(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new VehicleModelsProvider(dbContext, null, mapper);

            var vehicleModels = await modelsProvider.GetVehicleModelsAsync();

            //Checks if call is returning IsSuccess as status
            Assert.True(vehicleModels.IsSuccess);
            //Checks if we have any manufacturer
            Assert.True(vehicleModels.VehicleModels.Any());
            //Checks that there were no errors
            Assert.Null(vehicleModels.ErrorMessage);
        }

        [Fact]
        public async Task GetVehicleModelsReturnsVehicleModelUsingValidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehicleModelsReturnsVehicleModelUsingValidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleModels(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new VehicleModelsProvider(dbContext, null, mapper);

            var vehicleModels = await modelsProvider.GetVehicleModelAsync(1);

            //Checks if call is returning IsSuccess as status
            Assert.True(vehicleModels.IsSuccess);
            //Checks if we got the right Manufacturer
            Assert.NotNull(vehicleModels.VehicleModel);
            //Checks if we got the right Manufacturer
            Assert.True(vehicleModels.VehicleModel.Id == 1);
            //Checks that there were no errors
            Assert.Null(vehicleModels.ErrorMessage);
        }

        [Fact]
        public async Task GetVehicleModelsReturnsVehicleModelUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehicleModelsReturnsVehicleModelUsingInvalidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleModels(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new VehicleModelsProvider(dbContext, null, mapper);

            var vehicleModels = await modelsProvider.GetVehicleModelAsync(-100);

            //Checks if call is returning IsSuccess as false, due to the ID not existing
            Assert.False(vehicleModels.IsSuccess);
            //Checks if the object is null as it should
            Assert.Null(vehicleModels.VehicleModel);
            //Checks that we have an error
            Assert.NotNull(vehicleModels.ErrorMessage);
        }

        [Fact]
        public async Task AddVehicleModelsReturnsVehicleModel()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(AddVehicleModelsReturnsVehicleModel))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleModels(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new VehicleModelsProvider(dbContext, null, mapper);
            Random rand = new Random();

            var newVehicleModel = new Models.VehicleModelRequestNew() { 
                Name = Guid.NewGuid().ToString(), 
                FuelTypeId = rand.Next(1, 4),
                ManufacturerId = rand.Next(1, 4),
                RentalRate = (decimal)rand.NextDouble(),
                TrunkSize = rand.Next(1000),
                VehicleCategoryId = rand.Next(1, 4)
            };

            var vehicleModels = await modelsProvider.PostVehicleModelAsync(newVehicleModel);


            Assert.True(vehicleModels.IsSuccess);
            Assert.NotNull(vehicleModels.VehicleModel);
            Assert.True(vehicleModels.VehicleModel.Name == newVehicleModel.Name);
            Assert.Null(vehicleModels.ErrorMessage);
        }

        [Fact]
        public async Task PutValidIdVehicleModelsReturnsVehicleModel()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(PutValidIdVehicleModelsReturnsVehicleModel))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleModels(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new VehicleModelsProvider(dbContext, null, mapper);
            var allVehicleModels = await modelsProvider.GetVehicleModelsAsync();
            Random rand = new Random();

            var putVehicleModel = new Models.VehicleModelRequestUpdate() 
            { 
                Id = allVehicleModels.VehicleModels.First().Id, 
                Name = Guid.NewGuid().ToString(),
                FuelTypeId = rand.Next(1, 4),
                ManufacturerId = rand.Next(1, 4),
                RentalRate = (decimal)rand.NextDouble(),
                TrunkSize = rand.Next(1000),
                VehicleCategoryId = rand.Next(1, 4)
            };

            var vehicleModels = await modelsProvider.PutVehicleModelAsync(putVehicleModel);

            Assert.True(vehicleModels.IsSuccess);
            Assert.NotNull(vehicleModels.VehicleModel);
            Assert.True(vehicleModels.VehicleModel.Id == putVehicleModel.Id);
            Assert.True(vehicleModels.VehicleModel.Name == putVehicleModel.Name);
            Assert.Null(vehicleModels.ErrorMessage);
        }

        [Fact]
        public async Task PutInvalidIdVehicleModelsReturnsVehicleModel()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(PutInvalidIdVehicleModelsReturnsVehicleModel))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleModels(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new VehicleModelsProvider(dbContext, null, mapper);


            var putVehicleModel = new Models.VehicleModelRequestUpdate() { Id = -100, Name = Guid.NewGuid().ToString() };

            var vehicleModels = await modelsProvider.PutVehicleModelAsync(putVehicleModel);

            Assert.False(vehicleModels.IsSuccess);
            Assert.Null(vehicleModels.VehicleModel);
            Assert.NotNull(vehicleModels.ErrorMessage);
        }

        [Fact]
        public async Task DeleteValidIdVehicleModel()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(DeleteValidIdVehicleModel))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleModels(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new VehicleModelsProvider(dbContext, null, mapper);
            var allVehicleModels = await modelsProvider.GetVehicleModelsAsync();

            var vehicleModels = await modelsProvider.DeleteVehicleModelAsync(allVehicleModels.VehicleModels.First().Id);

            Assert.True(vehicleModels.IsSuccess);
            Assert.Null(vehicleModels.ErrorMessage);
        }

        [Fact]
        public async Task DeleteInvalidIdVehicleModel()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(DeleteInvalidIdVehicleModel))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleModels(dbContext);

            var modelProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new VehicleModelsProvider(dbContext, null, mapper);

            var vehicleModels = await modelsProvider.DeleteVehicleModelAsync(-100);

            Assert.False(vehicleModels.IsSuccess);
            Assert.NotNull(vehicleModels.ErrorMessage);
        }

        private void CreateVehicleModels(VehiclesDbContext dbContext)
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
        }
    }
}
