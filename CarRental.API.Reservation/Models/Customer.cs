using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public DateTime Birthdate { get; set; }
        public string CEP { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
