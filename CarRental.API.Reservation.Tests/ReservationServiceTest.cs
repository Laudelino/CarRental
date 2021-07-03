using CarRental.API.Reservation.DB;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarRental.API.Reservation.Tests
{
    public class ReservationServiceTest
    {
        [Fact]
        public async Task GetAReservation()
        {

        }
        [Fact]
        public async Task SimulateAReservation()
        {

        }
        [Fact]
        public async Task CreateAReservation()
        {

        }
        [Fact]
        public async Task UpdateAReservation()
        {

        }
        [Fact]
        public async Task CancelAReservation()
        {

        }
        [Fact]
        public async Task ReturnARent()
        {

        }
        [Fact]
        public async Task CreateAReservationFromASimulation()
        {

        }
        private void CreateReservations(ReservationsDbContext dbContext)
        {
            if (!dbContext.Reservations.Any())
            {
                for (int i = 1; i < 11; i++)
                {
                    dbContext.Reservations.Add(new DB.Reservation()
                    {
                        Id = i,
                        Status = Guid.NewGuid().ToString()
                    });
                }
                dbContext.SaveChanges();
            }
        }
    }
}
