using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Person.Models
{
    public class CustomerRegister
    {
        [Required(ErrorMessage = "CPF is required")]
        public string CPF { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }
        public string CEP { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
