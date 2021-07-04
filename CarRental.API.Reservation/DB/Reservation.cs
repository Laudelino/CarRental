using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.DB
{
    public class Reservation
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string CustomerCPF { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        public decimal RentalRate { get; set; }
        public decimal EstimatedTotal { get; set; }
        
        public DateTime ReturnDate { get; set; }
        public decimal ReturnTotal { get; set; }
        public bool IsClean { get; set; }
        public bool HasFullTank { get; set; }
        public bool HasScratches { get; set; }
        public bool HasDents { get; set; }

        public string Status { get; set; }
    }
}
