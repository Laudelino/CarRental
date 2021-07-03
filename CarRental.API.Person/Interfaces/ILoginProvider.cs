using CarRental.API.Person.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Interfaces
{
    public interface ILoginProvider
    {
        Task<(bool IsSuccess, LoginResponse Respose, string ErrorMessage)> Login(Login login);
    }
}
