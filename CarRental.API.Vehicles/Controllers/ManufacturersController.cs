using CarRental.API.Vehicles.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Controllers
{
    [ApiController]
    [Route("api/manufacturers")]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturersProvider manufacturersProvider;

        public ManufacturersController(IManufacturersProvider manufacturersProvider)
        {
            this.manufacturersProvider = manufacturersProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetManufacturersAsync()
        {
            var result = await manufacturersProvider.GetManufacturersAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Manufacturers);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufacturerAsync(int id)
        {
            var result = await manufacturersProvider.GetManufacturerAsync(id);

            if(result.IsSuccess)
            {
                return Ok(result.Manufacturer);
            }
            return NotFound();
        }

    }
}
