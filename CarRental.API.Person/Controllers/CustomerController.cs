using CarRental.API.Person.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomersProvider customerProvider;
        public CustomerController(ICustomersProvider customerProvider)
        {
            this.customerProvider = customerProvider;
        }

        [HttpPost]
        //[Authorize(Roles = "Operator")]
        public async Task<IActionResult> RegisterCustomer(Models.CustomerRegister customer)
        {
            var result = await customerProvider.RegisterCustomer(customer);

            if (result.IsSuccess)
            {
                return Created("", result.Customer);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomer(string cpf)
        {
            var result = await customerProvider.GetCustomer(cpf);

            if (result.IsSuccess)
            {
                return Ok(result.Customer);
            }
            return NotFound();
        }
    }
}
