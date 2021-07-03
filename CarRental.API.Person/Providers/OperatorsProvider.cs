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
    public class OperatorsProvider : IOperatorsProvider
    {
        private readonly PersonDbContext dbContext;
        private readonly ILogger<OperatorsProvider> logger;
        private readonly IMapper mapper;
        private readonly UserManager<DB.Person> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly DB.PersonDBInit personDBInit;
        public OperatorsProvider(PersonDbContext dBContext, ILogger<OperatorsProvider> logger, IMapper mapper, UserManager<DB.Person> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
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
        public async Task<(bool IsSuccess, Models.Operator Respose, string ErrorMessage)> RegisterOperator(OperatorRegister oper)
        {
            try
            {
                var dbperson = new DB.Person() 
                { 
                    Name = oper.Name,
                    RegistrationNumber = oper.RegistrationNumber,
                    UserName = oper.RegistrationNumber
                };

                var result = await userManager.CreateAsync(dbperson, oper.Password);
                if (await roleManager.RoleExistsAsync(UserRoles.Operator))
                {
                    await userManager.AddToRoleAsync(dbperson, UserRoles.Operator);
                }

                if (result.Succeeded)
                {
                    var resultoperator = mapper.Map<DB.Person, Models.Operator>(dbperson);
                    return (true, resultoperator, null);
                }
                return (false, null, "Failed to create the record");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Operator Respose, string ErrorMessage)> GetOperator(string registrationNumber)
        {
            try
            {
                var oper = await userManager.FindByNameAsync(registrationNumber);

                if (oper != null)
                {
                    var result = mapper.Map<DB.Person, Models.Operator>(oper);
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
    }
}
