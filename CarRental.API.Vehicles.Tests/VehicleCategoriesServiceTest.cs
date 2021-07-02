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
    public class VehicleCategoriesServiceTest
    {
        [Fact]
        public async Task GetVehicleCategoriesReturnsAllVehicleCategories()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehicleCategoriesReturnsAllVehicleCategories))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleCategories(dbContext);

            var categoryProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(categoryProfile));
            var mapper = new Mapper(config);
            var categoriesProvider = new VehicleCategoriesProvider(dbContext, null, mapper);

            var categories = await categoriesProvider.GetVehicleCategoriesAsync();

            //Checks if call is returning IsSuccess as status
            Assert.True(categories.IsSuccess);
            Assert.True(categories.VehicleCategories.Any());
            //Checks that there were no errors
            Assert.Null(categories.ErrorMessage);
        }

        [Fact]
        public async Task GetVehicleCategoryReturnsVehicleCategoryUsingValidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehicleCategoryReturnsVehicleCategoryUsingValidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleCategories(dbContext);

            var categoryProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(categoryProfile));
            var mapper = new Mapper(config);
            var categoriesProvider = new VehicleCategoriesProvider(dbContext, null, mapper);

            var category = await categoriesProvider.GetVehicleCategoryAsync(1);

            //Checks if call is returning IsSuccess as status
            Assert.True(category.IsSuccess);
            Assert.NotNull(category.VehicleCategory);
            Assert.True(category.VehicleCategory.Id == 1);
            //Checks that there were no errors
            Assert.Null(category.ErrorMessage);
        }

        [Fact]
        public async Task GetVehicleCategoryReturnsVehicleCategoryUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<VehiclesDbContext>()
                .UseInMemoryDatabase(nameof(GetVehicleCategoryReturnsVehicleCategoryUsingInvalidId))
                .Options;
            var dbContext = new VehiclesDbContext(options);

            CreateVehicleCategories(dbContext);

            var categoryProfile = new VehicleProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(categoryProfile));
            var mapper = new Mapper(config);
            var categoriesProvider = new VehicleCategoriesProvider(dbContext, null, mapper);

            var category = await categoriesProvider.GetVehicleCategoryAsync(-100);

            //Checks if call is returning IsSuccess as false, due to the ID not existing
            Assert.False(category.IsSuccess);
            //Checks if the object is null as it should
            Assert.Null(category.VehicleCategory);
            //Checks that we have an error
            Assert.NotNull(category.ErrorMessage);
        }
        private void CreateVehicleCategories(VehiclesDbContext dbContext)
        {
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
        }
    }
}
