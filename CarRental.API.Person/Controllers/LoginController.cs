using CarRental.API.Person.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginProvider loginProvider;

        public LoginController(ILoginProvider loginProvider)
        {
            this.loginProvider = loginProvider;
        }
        [HttpPost]
        public async Task<IActionResult> Login(Models.Login login)
        {
            var result = await loginProvider.Login(login);
            
            if(result.IsSuccess)
            {
                if (result.Respose.Operator != null)
                {
                    return Ok(
                        new
                        {
                            result.Respose.token,
                            result.Respose.Operator
                        });
                }
                if (result.Respose.Customer != null)
                {
                    return Ok(
                        new
                        {
                            result.Respose.token,
                            result.Respose.Customer
                        });
                }
                return Ok(
                    new
                    {
                        result.Respose.token
                    });
            }
            return Unauthorized();
        }
    }
}
