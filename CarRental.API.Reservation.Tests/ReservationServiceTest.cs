using AutoMapper;
using CarRental.API.Reservation.DB;
using CarRental.API.Reservation.Profiles;
using CarRental.API.Reservation.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarRental.API.Reservation.Tests
{
    public class ReservationServiceTest
    {
        [Fact]
        public async Task GetAReservationUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                .UseInMemoryDatabase(nameof(GetAReservationUsingValidId))
                .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);

            var reservation = await modelsProvider.GetReservationAsync(1);

            //Checks if call is returning IsSuccess as status
            Assert.True(reservation.IsSuccess);
            Assert.NotNull(reservation.Reservation);
            Assert.True(reservation.Reservation.Id == 1);
            //Checks that there were no errors
            Assert.Null(reservation.ErrorMessage);
        }
        [Fact]
        public async Task GetAReservationUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                .UseInMemoryDatabase(nameof(GetAReservationUsingInvalidId))
                .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);

            var reservation = await modelsProvider.GetReservationAsync(-100);

            //Checks if call is returning IsSuccess as status
            Assert.False(reservation.IsSuccess);
            Assert.Null(reservation.Reservation);
            //Checks that there were no errors
            Assert.NotNull(reservation.ErrorMessage);
        }
        [Fact]
        public async Task GetAReservationsByCustomerId()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                .UseInMemoryDatabase(nameof(GetAReservationsByCustomerId))
                .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);

            var reservations = await modelsProvider.GetReservationByCustomerAsync("12345678999");

            int dbreservationsCount = await dbContext.Reservations.Where(c => c.CustomerCPF == "12345678999").CountAsync();

            //Checks if call is returning IsSuccess as status
            Assert.True(reservations.IsSuccess);
            Assert.NotNull(reservations.Reservations);
            Assert.True(dbreservationsCount == reservations.Reservations.Count());
            //Checks that there were no errors
            Assert.Null(reservations.ErrorMessage);
        }
        [Fact]
        public async Task CheckVehicleAvailability()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                .UseInMemoryDatabase(nameof(CheckVehicleAvailability))
                .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);

            var reservations = await modelsProvider.GetVehicleAvailabilityAsync(1);

            int dbreservationsCount = await dbContext.Reservations.Where(v => v.VehicleId == 1).Where(s => s.Status == ReservationStatus.Active).CountAsync();
            bool dbvehicleAvailable = (dbreservationsCount == 0);
            //Checks if call is returning IsSuccess as status
            Assert.True(reservations.IsSuccess);
            Assert.NotNull(reservations.VehicleAvailability);
            Assert.True(reservations.VehicleAvailability.Id == 1);
            Assert.True(dbvehicleAvailable == reservations.VehicleAvailability.IsAvailable);
            //Checks that there were no errors
            Assert.Null(reservations.ErrorMessage);
        }
        [Fact]
        public async Task SimulateAReservation()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                .UseInMemoryDatabase(nameof(SimulateAReservation))
                .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);

            Random rand = new Random();
            var simulationRequest = new Models.ReserveSimulationRequest()
            {
                ModelId = rand.Next(1, 4),
                ReservationStart = DateTime.Now.AddDays((double)rand.Next(1, 3)),
                ReservationEnd = DateTime.Now.AddDays((double)rand.Next(4, 7))
            };

            var reservationSimulation = await modelsProvider.PostReserveSimulationAsync(simulationRequest);

            //Checks if call is returning IsSuccess as status
            Assert.True(reservationSimulation.IsSuccess);
            Assert.NotNull(reservationSimulation.ReserveSimulation);
            //Checks that there were no errors
            Assert.Null(reservationSimulation.ErrorMessage);
        }
        [Fact]
        public async Task CreateAReservation()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                            .UseInMemoryDatabase(nameof(CreateAReservation))
                            .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);
            Random rand = new Random();

            var newReserve = new Models.ReserveRequest()
            {
                CustomerCPF = "12345678999",
                ModelId = rand.Next(1, 4),
                ReservationStart = DateTime.Now.AddDays((double)rand.Next(1, 3)),
                ReservationEnd = DateTime.Now.AddDays((double)rand.Next(4, 7)),
                EstimatedTotal = (decimal)rand.NextDouble() * 100
            };

            var reserve = await modelsProvider.PostReserveAsync(newReserve);

            Assert.True(reserve.IsSuccess);
            Assert.NotNull(reserve.Reservation);
            Assert.True(reserve.Reservation.CustomerCPF == newReserve.CustomerCPF);
            Assert.True(reserve.Reservation.ReservationStart == newReserve.ReservationStart);
            Assert.True(reserve.Reservation.ReservationEnd == newReserve.ReservationEnd);
            Assert.True(reserve.Reservation.EstimatedTotal == newReserve.EstimatedTotal);
            Assert.Null(reserve.ErrorMessage);
        }
        [Fact]
        public async Task UpdateAReservation()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                                                    .UseInMemoryDatabase(nameof(UpdateAReservation))
                                                    .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);

            var dbreservation = await dbContext.Reservations.FirstOrDefaultAsync();

            Random rand = new Random();

            var newReserve = new Models.ReservationRequestUpdate()
            {
                Id = dbreservation.Id,
                CustomerCPF = "12345678999",
                VehicleId = rand.Next(1, 4),
                ReservationStart = DateTime.Now.AddDays((double)rand.Next(1, 3)),
                ReservationEnd = DateTime.Now.AddDays((double)rand.Next(4, 7)),
                EstimatedTotal = (decimal)rand.NextDouble() * 100,
                ReturnTotal = (decimal)rand.NextDouble() * 100,
                RentalRate = (decimal)rand.NextDouble() * 10,
                HasDents = (bool)(rand.NextDouble() >= 0.5),
                HasFullTank = (bool)(rand.NextDouble() >= 0.5),
                HasScratches = (bool)(rand.NextDouble() >= 0.5),
                IsClean = (bool)(rand.NextDouble() >= 0.5),
                ReturnDate = dbreservation.ReservationEnd
            };

            var reserve = await modelsProvider.PutReservationAsync(newReserve);

            Assert.True(reserve.IsSuccess);
            Assert.NotNull(reserve.Reservation);
            Assert.True(reserve.Reservation.Id == newReserve.Id);
            Assert.True(reserve.Reservation.HasDents == newReserve.HasDents);
            Assert.True(reserve.Reservation.HasFullTank == newReserve.HasFullTank);
            Assert.True(reserve.Reservation.HasScratches == newReserve.HasScratches);
            Assert.True(reserve.Reservation.IsClean == newReserve.IsClean);
            Assert.Null(reserve.ErrorMessage);
        }
        [Fact]
        public async Task CancelAReservation()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                        .UseInMemoryDatabase(nameof(CancelAReservation))
                        .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);

            var dbreservation = await dbContext.Reservations.Where(r => r.Status != ReservationStatus.Cancelled).FirstOrDefaultAsync();

            var reserve = await modelsProvider.CancelReservationAsync(dbreservation.Id);

            Assert.True(reserve.IsSuccess);
            Assert.NotNull(reserve.Reservation);
            Assert.True(reserve.Reservation.Id == dbreservation.Id);
            Assert.True(reserve.Reservation.Status == ReservationStatus.Cancelled);
            Assert.Null(reserve.ErrorMessage);
        }
        [Fact]
        public async Task ReturnARent()
        {
            var options = new DbContextOptionsBuilder<ReservationsDbContext>()
                                        .UseInMemoryDatabase(nameof(ReturnARent))
                                        .Options;
            var dbContext = new ReservationsDbContext(options);

            CreateReservations(dbContext);

            var modelProfile = new ReservationProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(modelProfile));
            var mapper = new Mapper(config);
            var modelsProvider = new ReservationProvider(dbContext, null, mapper, null);

            var dbreservation = await dbContext.Reservations.Where(s => s.Status == ReservationStatus.Active).FirstOrDefaultAsync();

            Random rand = new Random();

            var newReserve = new Models.ReservationReturn()
            {
                Id = dbreservation.Id,
                HasDents = (bool)(rand.NextDouble() >= 0.5),
                HasFullTank = (bool)(rand.NextDouble() >= 0.5),
                HasScratches = (bool)(rand.NextDouble() >= 0.5),
                IsClean = (bool)(rand.NextDouble() >= 0.5),
                ReturnDate = dbreservation.ReservationEnd
            };

            var reserve = await modelsProvider.PutReservationReturnAsync(newReserve);

            Assert.True(reserve.IsSuccess);
            Assert.NotNull(reserve.Reservation);
            Assert.True(reserve.Reservation.Id == newReserve.Id);
            Assert.True(reserve.Reservation.HasDents == newReserve.HasDents);
            Assert.True(reserve.Reservation.HasFullTank == newReserve.HasFullTank);
            Assert.True(reserve.Reservation.HasScratches == newReserve.HasScratches);
            Assert.True(reserve.Reservation.IsClean == newReserve.IsClean);
            Assert.True(reserve.Reservation.Status == ReservationStatus.Completed);
            Assert.Null(reserve.ErrorMessage);
        }
        private void CreateReservations(ReservationsDbContext dbContext)
        {
            if (!dbContext.Reservations.Any())
            {
                Random rand = new Random();
                string status = ReservationStatus.Active;
                int rdStatus = 0;
                DateTime reservationStart = DateTime.Now;
                DateTime reservationEnd = DateTime.Now;
                DateTime returnDate = DateTime.Now;
                for (int i = 1; i < 11; i++)
                {
                    rdStatus = rand.Next(0, 3);
                    
                    switch (rdStatus)
                    {
                        case 0:
                            status = ReservationStatus.Active;
                            reservationStart = DateTime.Now.AddDays((double)rand.Next(1, 3));
                            reservationEnd = DateTime.Now.AddDays((double)rand.Next(4, 7));
                            break;
                        case 1:
                            status = ReservationStatus.Active; 
                            reservationStart = DateTime.Now.AddDays((double)rand.Next(1, 3)*-1);
                            reservationEnd = DateTime.Now.AddDays((double)rand.Next(1, 3));
                            break;
                        case 2:
                            status = ReservationStatus.Completed;
                            reservationStart = DateTime.Now.AddDays((double)rand.Next(4, 7) * -1);
                            reservationEnd = DateTime.Now.AddDays((double)rand.Next(1, 3)*-1);
                            returnDate = reservationEnd;
                            break;
                        case 3:
                            status = ReservationStatus.Cancelled;
                            reservationStart = DateTime.Now.AddDays((double)rand.Next(4, 7) * -1);
                            reservationEnd = DateTime.Now.AddDays((double)rand.Next(4, 7));
                            break;
                        default:
                            status = ReservationStatus.Active;  break;
                    }
                    var dbReservation = new DB.Reservation()
                    {
                        Id = i,
                        Status = status,
                        CustomerCPF = "12345678999",
                        VehicleId = rand.Next(1, 4),
                        ReservationStart = reservationStart,
                        ReservationEnd = reservationEnd,
                        EstimatedTotal = (decimal) rand.NextDouble()*100
                    };
                    if(dbReservation.Status== ReservationStatus.Completed)
                    {
                        dbReservation.HasDents = (bool)(rand.NextDouble() >= 0.5);
                        dbReservation.HasFullTank = (bool)(rand.NextDouble() >= 0.5);
                        dbReservation.HasScratches = (bool)(rand.NextDouble() >= 0.5);
                        dbReservation.IsClean = (bool)(rand.NextDouble() >= 0.5);
                        dbReservation.ReturnDate = returnDate;
                        dbReservation.ReturnTotal = dbReservation.EstimatedTotal;
                    }
                    dbContext.Reservations.Add(dbReservation);
                }
                dbContext.SaveChanges();
            }
        }
    }
}
