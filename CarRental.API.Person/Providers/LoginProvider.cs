using AutoMapper;
using CarRental.API.Person.DB;
using CarRental.API.Person.Interfaces;
using CarRental.API.Person.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.API.Person.Providers
{
    public class LoginProvider : ILoginProvider
    {
        private readonly PersonDbContext dbContext;
        private readonly ILogger<LoginProvider> logger;
        private readonly IMapper mapper;
        private readonly UserManager<DB.Person> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly DB.PersonDBInit personDBInit;

        public LoginProvider(PersonDbContext dBContext, ILogger<LoginProvider> logger, IMapper mapper, UserManager<DB.Person> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
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

        public async Task<(bool IsSuccess, LoginResponse Respose, string ErrorMessage)> Login(Models.Login login)
        {
            try
            {
                var user = await userManager.FindByNameAsync(login.Username);

                if (user != null)
                {
                    if (await userManager.CheckPasswordAsync(user, login.Password))
                    {
                        var userRoles = await userManager.GetRolesAsync(user);

                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }                        

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                        var token = new JwtSecurityToken(
                            expires: DateTime.Now.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );

                        var response = new Models.LoginResponse
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token)
                        };
                        if (userRoles.Count == 1)
                        {
                            if (userRoles[0] == "Operator")
                            {
                                response.Operator = mapper.Map<DB.Person, Models.Operator>(user);
                            }
                            else
                            {
                                response.Customer = mapper.Map<DB.Person, Models.Customer>(user);
                            }
                        }
                        return (true, response, null);
                    }
                }
                return (false, null, "Invalid login");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private async void SeedData(PersonDbContext dbContext)
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
                    RegistrationNumber = "12345678999",
                    UserName = "12345678999"
                };
                result = await userManager.CreateAsync(oper, "Customer01!");
                if (await roleManager.RoleExistsAsync(UserRoles.Customer))
                {
                    await userManager.AddToRoleAsync(oper, UserRoles.Customer);
                }
            }
        }
    }
}
