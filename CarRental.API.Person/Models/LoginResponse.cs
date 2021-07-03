using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Models
{
    public class LoginResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string token { get; set; }
        public virtual Models.Customer Customer { get; set; }
        public virtual Models.Operator Operator { get; set; }
    }
}
