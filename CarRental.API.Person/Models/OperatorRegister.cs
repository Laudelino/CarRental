using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Person.Models
{
    public class OperatorRegister
    {
        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
