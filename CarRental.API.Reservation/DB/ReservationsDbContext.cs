using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.DB
{
    public class ReservationsDbContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }
        public ReservationsDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
