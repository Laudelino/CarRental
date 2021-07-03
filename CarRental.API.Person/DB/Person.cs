using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.DB
{
    public class Person : IdentityUser
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public string CEP { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
