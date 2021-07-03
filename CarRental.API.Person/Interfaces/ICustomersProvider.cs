using CarRental.API.Person.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> RegisterCustomer(CustomerRegister oper);
        Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> GetCustomer(string cpf);
    }
}
