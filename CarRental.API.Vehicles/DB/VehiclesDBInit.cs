using System.Linq;

namespace CarRental.API.Vehicles.DB
{
    public static class VehiclesDBInit
    {
        public static void SeedData(VehiclesDbContext dbContext) 
        {
            if (!dbContext.FuelTypes.Any())
            {
                dbContext.FuelTypes.Add(new DB.FuelType() { Id = 1, Name = "Gasolina" });
                dbContext.FuelTypes.Add(new DB.FuelType() { Id = 2, Name = "Álcool" });
                dbContext.FuelTypes.Add(new DB.FuelType() { Id = 3, Name = "Diesel" });

                dbContext.SaveChanges();
            }
            if (!dbContext.Manufacturers.Any())
            {
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 1, Name = "Volkswagen" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 2, Name = "Fiat" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 3, Name = "Ford" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 4, Name = "GM" });
                dbContext.SaveChanges();
            }
            if (!dbContext.VehicleCategories.Any())
            {
                dbContext.VehicleCategories.Add(new DB.VehicleCategory() { Id = 1, Name = "Básico" });
                dbContext.VehicleCategories.Add(new DB.VehicleCategory() { Id = 2, Name = "Completo" });
                dbContext.VehicleCategories.Add(new DB.VehicleCategory() { Id = 3, Name = "Luxo" });

                dbContext.SaveChanges();
            }
            if (!dbContext.VehicleModels.Any())
            {
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 1, Name = "Uno 1.0", FuelTypeId = 1, ManufacturerId = 2, RentalRate = 9.5M, TrunkSize = 200, VehicleCategoryId = 1 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 2, Name = "Gol 1.0", FuelTypeId = 2, ManufacturerId = 1, RentalRate = 9.5M, TrunkSize = 200, VehicleCategoryId = 1 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 3, Name = "Virtus Comfortline", FuelTypeId = 1, ManufacturerId = 1, RentalRate = 15.5M, TrunkSize = 200, VehicleCategoryId = 2 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 4, Name = "Ecosport 1.5", FuelTypeId = 2, ManufacturerId = 3, RentalRate = 16.5M, TrunkSize = 500, VehicleCategoryId = 2 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 5, Name = "Cruze Sedan FAST", FuelTypeId = 2, ManufacturerId = 4, RentalRate = 18.5M, TrunkSize = 300, VehicleCategoryId = 3 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 6, Name = "S10 2.8", FuelTypeId = 3, ManufacturerId = 4, RentalRate = 17.5M, TrunkSize = 500, VehicleCategoryId = 3 });
                dbContext.SaveChanges();
            }
            if (!dbContext.Vehicles.Any())
            {
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 1, Plate = "ABC-1234", VehicleModelId = 1, Year = 2022, IsReserved = true });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 2, Plate = "DEF-5678", VehicleModelId = 2, Year = 2010, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 3, Plate = "GHI-9012", VehicleModelId = 3, Year = 2020, IsReserved = true });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 4, Plate = "BRA2E19", VehicleModelId = 4, Year = 2021, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 5, Plate = "QTP5F71", VehicleModelId = 5, Year = 2018, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 6, Plate = "MSK9B10", VehicleModelId = 6, Year = 2019, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 7, Plate = "JKL-1023", VehicleModelId = 3, Year = 2020, IsReserved = false });
                dbContext.SaveChanges();
            }
        }
    }
}
