using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.DB
{
    public static class ReservationsDBInit
    {
        public static void SeedData(ReservationsDbContext dbContext)
        {
            if (!dbContext.Reservations.Any())
            {
                dbContext.Reservations.Add(new DB.Reservation() 
                { 
                    Id = 1,  
                    Status = ReservationStatus.Active,
                    CustomerCPF = "12345678999",
                    EstimatedTotal = 192,
                    RentalRate = 2,
                    ReservationStart = DateTime.Now,
                    ReservationEnd = DateTime.Now.AddDays(4),
                    VehicleId = 1
                });
                dbContext.Reservations.Add(new DB.Reservation()
                {
                    Id = 4,
                    Status = ReservationStatus.Active,
                    CustomerCPF = "12345678999",
                    EstimatedTotal = 3.5M * 5 * 24,
                    RentalRate = 3.5M,
                    ReservationStart = DateTime.Now.AddDays(-1),
                    ReservationEnd = DateTime.Now.AddDays(4),
                    VehicleId = 3
                });

                dbContext.Reservations.Add(new DB.Reservation()
                {
                    Id = 2,
                    Status = ReservationStatus.Cancelled,
                    CustomerCPF = "12345678999",
                    EstimatedTotal = 2*3*24,
                    RentalRate = 2,
                    ReservationStart = DateTime.Now.AddDays(1),
                    ReservationEnd = DateTime.Now.AddDays(4),
                    VehicleId = 2
                });

                dbContext.Reservations.Add(new DB.Reservation()
                {
                    Id = 3,
                    Status = ReservationStatus.Completed,
                    CustomerCPF = "12345678999",
                    EstimatedTotal = 3*2*24,
                    RentalRate = 3,
                    ReservationStart = DateTime.Now.AddDays(-10),
                    ReservationEnd = DateTime.Now.AddDays(-8),
                    VehicleId = 1
                });


                dbContext.Reservations.Add(new DB.Reservation()
                {
                    Id = 5,
                    Status = ReservationStatus.Completed,
                    CustomerCPF = "00000000000",
                    EstimatedTotal = 3 * 3 * 24,
                    RentalRate = 3,
                    ReservationStart = DateTime.Now.AddDays(-15),
                    ReservationEnd = DateTime.Now.AddDays(-12),
                    VehicleId = 5
                });

                dbContext.Reservations.Add(new DB.Reservation()
                {
                    Id = 6,
                    Status = ReservationStatus.Completed,
                    CustomerCPF = "98765432100",
                    EstimatedTotal = 3 * 4 * 24,
                    RentalRate = 3,
                    ReservationStart = DateTime.Now.AddDays(-20),
                    ReservationEnd = DateTime.Now.AddDays(-16),
                    VehicleId = 6
                });

                dbContext.Reservations.Add(new DB.Reservation()
                {
                    Id = 6,
                    Status = ReservationStatus.Active,
                    CustomerCPF = "98765432100",
                    EstimatedTotal = 3 * 30 * 24,
                    RentalRate = 3,
                    ReservationStart = DateTime.Now.AddDays(-15),
                    ReservationEnd = DateTime.Now.AddDays(15),
                    VehicleId = 7
                });

                dbContext.SaveChanges();
            }
        }
    }
}
