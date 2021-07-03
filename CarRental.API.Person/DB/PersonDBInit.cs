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
                    Name = "LaudelinoC",
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
            }
        }
    }
}
