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
        
        [HttpGet("{id}/models")]
        public async Task<IActionResult> GetVehicleModelsByManufacturerAsync(int id)
        {
            var result = await manufacturersProvider.GetVehicleModelsByManufacturerAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.VehicleModels);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> PostManufacturerAsync(Models.ManufacturerRequestNew manufacturer)
        {
            var result = await manufacturersProvider.PostManufacturerAsync(manufacturer);

            if(result.IsSuccess)
            {
                return Created("", result.Manufacturer);
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> PutManufacturerAsync(Models.ManufacturerRequestUpdate manufacturer)
        {
            var result = await manufacturersProvider.PutManufacturerAsync(manufacturer);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManufacturerAsync(int id)
        {
            var result = await manufacturersProvider.DeleteManufacturerAsync(id);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }

    }
}
