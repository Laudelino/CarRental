using AutoMapper;
using CarRental.API.Reservation.DB;
using CarRental.API.Reservation.Interfaces;
using CarRental.API.Reservation.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Providers
{
    public class ReservationProvider : IReservationProvider
    {
        private readonly ReservationsDbContext dbContext;
        private readonly ILogger<ReservationProvider> logger;
        private readonly IMapper mapper;
        public ReservationProvider(ReservationsDbContext dBContext, ILogger<ReservationProvider> logger, IMapper mapper)
        {
            this.dbContext = dBContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
        private void SeedData()
        {
            if (!dbContext.Reservations.Any())
            {
                dbContext.Reservations.Add(new DB.Reservation() { Id = 1, VehicleId = 1 });
                dbContext.Reservations.Add(new DB.Reservation() { Id = 2, VehicleId = 2 });
                dbContext.Reservations.Add(new DB.Reservation() { Id = 3, VehicleId = 3 });

                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> CancelReservationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> GetReservationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PostReservationReturnAsync(ReservationReturn reserve)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PostReserveAsync(ReserveRequest reserve)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool IsSuccess, ReserveSimulation ReserveSimulation, string ErrorMessage)> PostReserveSimulationAsync(ReserveSimulationRequest reserveSimulation)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PutReservationAsync(ReservationRequestUpdate reserve)
        {
            throw new NotImplementedException();
        }

       
    }
}
