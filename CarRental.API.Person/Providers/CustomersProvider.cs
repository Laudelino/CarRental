using AutoMapper;
using CarRental.API.Person.DB;
using CarRental.API.Person.Interfaces;
using CarRental.API.Person.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly PersonDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;
        private readonly UserManager<DB.Person> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly DB.PersonDBInit personDBInit;
        public CustomersProvider(PersonDbContext dBContext, ILogger<CustomersProvider> logger, IMapper mapper, UserManager<DB.Person> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.dbContext = dBContext;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;

            this.personDBInit = new PersonDBInit(userManager, roleManager, configuration);
            personDBInit.SeedData(dbContext);
        }
        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomer(string cpf)
        {
            try
            {
                var customer = await userManager.FindByNameAsync(cpf);

                if (customer != null)
                {
                    var result = mapper.Map<DB.Person, Models.Customer>(customer);
                    return (true, result, null);
                }
                return (false, null, "Not Found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> RegisterCustomer(CustomerRegister customer)
        {
            try
            {
                var dbperson = new DB.Person()
                {
                    Name = customer.Name,
                    CPF = customer.CPF,
                    UserName = customer.CPF,
                    Birthdate = customer.Birthdate,
                    CEP = customer.CEP,
                    City = customer.City,
                    Number = customer.Number,
                    State = customer.State,
                    Street = customer.Street,
                    Complement = customer.Complement
                };

                var result = await userManager.CreateAsync(dbperson, customer.Password);
                if (await roleManager.RoleExistsAsync(UserRoles.Customer))
                {
                    await userManager.AddToRoleAsync(dbperson, UserRoles.Customer);
                }

                if (result.Succeeded)
                {
                    var resultcustomer = mapper.Map<DB.Person, Models.Customer>(dbperson);
                    return (true, resultcustomer, null);
                }
                return (false, null, "Failed to create the record");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
