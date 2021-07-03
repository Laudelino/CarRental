using CarRental.API.Person.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Controllers
{
    [ApiController]
    [Route("api/operator")]
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorsProvider operatorProvider;

        public OperatorController(IOperatorsProvider operatorProvider)
        {
            this.operatorProvider = operatorProvider;
        }

        [HttpPost]
        //[Authorize(Roles = "Operator")]
        public async Task<IActionResult> RegisterOperator(Models.OperatorRegister oper)
        {
            var result = await operatorProvider.RegisterOperator(oper);

            if (result.IsSuccess)
            {
                return Created("", result.Respose);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetOperator(string registrationNumber)
        {
            var result = await operatorProvider.GetOperator(registrationNumber);

            if (result.IsSuccess)
            {
                return Ok(result.Respose);
            }
            return NotFound();
        }
    }
}
