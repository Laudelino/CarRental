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
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 5, Name = "Renault" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 6, Name = "Hyundai" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 7, Name = "Citroen" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 8, Name = "Peugeot" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 9, Name = "Jeep" });
                dbContext.Manufacturers.Add(new DB.Manufacturer() { Id = 10, Name = "Toyota" });
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
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 7, Name = "Mobi 1.0", FuelTypeId = 2, ManufacturerId = 1, RentalRate = 8.5M, TrunkSize = 120, VehicleCategoryId = 1 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 8, Name = "Kwid 1.0", FuelTypeId = 1, ManufacturerId = 5, RentalRate = 10M, TrunkSize = 210, VehicleCategoryId = 1 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 9, Name = "HB20 1.6", FuelTypeId = 1, ManufacturerId = 6, RentalRate = 13.5M, TrunkSize = 200, VehicleCategoryId = 2 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 10, Name = "C4 Cactus 1.6", FuelTypeId = 1, ManufacturerId = 7, RentalRate = 16.5M, TrunkSize = 250, VehicleCategoryId = 2 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 11, Name = "Renegade Sport 1.8", FuelTypeId = 3, ManufacturerId = 9, RentalRate = 19.5M, TrunkSize = 350, VehicleCategoryId = 3 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 12, Name = "Corolla GLI", FuelTypeId = 1, ManufacturerId = 10, RentalRate = 17.0M, TrunkSize = 250, VehicleCategoryId = 3 });
                dbContext.VehicleModels.Add(new DB.VehicleModel() { Id = 13, Name = "2008 1.6", FuelTypeId = 1, ManufacturerId = 8, RentalRate = 14.5M, TrunkSize = 250, VehicleCategoryId = 2 });
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
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 7, Plate = "JKL-1023", VehicleModelId = 3, Year = 2020, IsReserved = true });

                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 8, Plate = "MNO-3456", VehicleModelId = 7, Year = 2018, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 9, Plate = "PQR-7890", VehicleModelId = 8, Year = 2019, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 10, Plate = "STU-0123", VehicleModelId = 9, Year = 2020, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 11, Plate = "ARG2E19", VehicleModelId = 10, Year = 2021, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 12, Plate = "EQU5F71", VehicleModelId = 11, Year = 2022, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 13, Plate = "VWX9B10", VehicleModelId = 12, Year = 2021, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 14, Plate = "YAB-4567", VehicleModelId = 13, Year = 2020, IsReserved = false });

                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 15, Plate = "CDE-8909", VehicleModelId = 7, Year = 2019, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 16, Plate = "FGH-8765", VehicleModelId = 8, Year = 2020, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 17, Plate = "IJK-4321", VehicleModelId = 9, Year = 2021, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 18, Plate = "COL2E19", VehicleModelId = 10, Year = 2022, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 19, Plate = "PAR5F71", VehicleModelId = 11, Year = 2018, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 20, Plate = "VEN9B10", VehicleModelId = 12, Year = 2019, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 21, Plate = "OPQ-0123", VehicleModelId = 13, Year = 2020, IsReserved = false });

                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 22, Plate = "RST-1234", VehicleModelId = 7, Year = 2017, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 23, Plate = "UVX-5678", VehicleModelId = 8, Year = 2018, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 24, Plate = "WYZ-9012", VehicleModelId = 9, Year = 2019, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 25, Plate = "CHI2E19", VehicleModelId = 10, Year = 2020, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 26, Plate = "BOL5F71", VehicleModelId = 11, Year = 2021, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 27, Plate = "URU9B10", VehicleModelId = 12, Year = 2020, IsReserved = false });
                dbContext.Vehicles.Add(new DB.Vehicle() { Id = 28, Plate = "DDD-1023", VehicleModelId = 13, Year = 2022, IsReserved = false });
                dbContext.SaveChanges();
            }
        }
    }
}
