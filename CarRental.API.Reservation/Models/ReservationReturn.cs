using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Models
{
    public class ReservationReturn
    {
        [Required]
        public int Id { get; set; }
        [Required] 
        public DateTime ReturnDate { get; set; }
        [Required] 
        public bool IsClean { get; set; }
        [Required] 
        public bool HasFullTank { get; set; }
        [Required] 
        public bool HasScratches { get; set; }
        [Required] 
        public bool HasDents { get; set; }
    }
}
