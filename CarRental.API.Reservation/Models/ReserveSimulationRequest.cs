using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Models
{
    public class ReserveSimulationRequest
    {
        [Required] 
        public int ModelId { get; set; }
        [Required] 
        public DateTime ReservationStart { get; set; }
        [Required] 
        public DateTime ReservationEnd { get; set; }
        [Required] 
        public decimal RentalRate { get; set; }
        [Required] 
        public decimal EstimatedTotal { get; set; }
    }
}
