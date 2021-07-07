using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace CarRental.API.Person.DB
{
    public class PersonDBInit
    {
        private readonly UserManager<DB.Person> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public PersonDBInit(UserManager<DB.Person> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        public async void SeedData(PersonDbContext dbContext)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Operator))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Operator));
            if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));

            if (!dbContext.People.Any())
            {
                var oper = new DB.Person()
                {
                    Name = "Laudelino",
                    RegistrationNumber = "12345678900",
                    UserName = "12345678900"
                };
                var result = await userManager.CreateAsync(oper, "Oper01!");
                if (await roleManager.RoleExistsAsync(UserRoles.Operator))
                {
                    await userManager.AddToRoleAsync(oper, UserRoles.Operator);
                }

                var customer = new DB.Person()
                {
                    Name = "Laudelino Cliente",
                    CPF = "12345678999",
                    UserName = "12345678999",
                    Birthdate = new System.DateTime(1990, 1, 1),
                    CEP = "00000000",
                    City = "Curitiba",
                    Complement = "SB04",
                    Number = "104",
                    State = "PR",
                    Street = "Franscisco Der"
                };
                result = await userManager.CreateAsync(customer, "Customer01!");
                if (await roleManager.RoleExistsAsync(UserRoles.Customer))
                {
                    await userManager.AddToRoleAsync(customer, UserRoles.Customer);
                }
                customer = new DB.Person()
                {
                    Name = "Cliente Alugador",
                    CPF = "00000000000",
                    UserName = "00000000000",
                    Birthdate = new System.DateTime(1980, 1, 1),
                    CEP = "00000000",
                    City = "Recife",
                    Complement = "SB04",
                    Number = "104",
                    State = "PR",
                    Street = "Franscisco Der"
                };
                result = await userManager.CreateAsync(customer, "Cliente123!");
                if (await roleManager.RoleExistsAsync(UserRoles.Customer))
                {
                    await userManager.AddToRoleAsync(customer, UserRoles.Customer);
                }
                customer = new DB.Person()
                {
                    Name = "Uber Alugador",
                    CPF = "98765432100",
                    UserName = "98765432100",
                    Birthdate = new System.DateTime(1970, 1, 1),
                    CEP = "00000000",
                    City = "Fortaleza",
                    Complement = "SB04",
                    Number = "104",
                    State = "PR",
                    Street = "Franscisco Der"
                };
                result = await userManager.CreateAsync(customer, "Uber123!");
                if (await roleManager.RoleExistsAsync(UserRoles.Customer))
                {
                    await userManager.AddToRoleAsync(customer, UserRoles.Customer);
                }
            }
        }
    }
}
