using CarRental.API.Person.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Interfaces
{
    public interface IOperatorsProvider
    {
        Task<(bool IsSuccess, Operator Respose, string ErrorMessage)> RegisterOperator(OperatorRegister oper);
        Task<(bool IsSuccess, Operator Respose, string ErrorMessage)> GetOperator(string registrationNumber);
    }
}
