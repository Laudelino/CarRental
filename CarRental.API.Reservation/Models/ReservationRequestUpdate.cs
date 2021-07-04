using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Models
{
    public class ReservationRequestUpdate
    {
        [Required]
        public int Id { get; set; }
        [Required] 
        public int VehicleId { get; set; }
        [Required] 
        public string CustomerCPF { get; set; }
        [Required] 
        public DateTime ReservationStart { get; set; }
        [Required] 
        public DateTime ReservationEnd { get; set; }
        [Required] 
        public decimal RentalRate { get; set; }
        [Required] 
        public decimal EstimatedTotal { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        [Required] 
        public decimal ReturnTotal { get; set; }
        [Required] 
        public bool IsClean { get; set; }
        [Required] 
        public bool HasFullTank { get; set; }
        [Required] 
        public bool HasScratches { get; set; }
        [Required] 
        public bool HasDents { get; set; }
        public string Status { get; set; }
    }
}
