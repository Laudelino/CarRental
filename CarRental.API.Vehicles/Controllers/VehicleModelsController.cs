using CarRental.API.Vehicles.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Controllers
{
    [ApiController]
    [Route("api/models")]
    public class VehicleModelsController : ControllerBase
    {
        private readonly IVehicleModelsProvider vehicleModelsProvider;

        public VehicleModelsController(IVehicleModelsProvider vehicleModelsProvider)
        {
            this.vehicleModelsProvider = vehicleModelsProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicleModelsAsync()
        {
            var result = await vehicleModelsProvider.GetVehicleModelsAsync();

            if (result.IsSuccess)
            {
                return Ok(result.VehicleModels);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleModelAsync(int id)
        {
            var result = await vehicleModelsProvider.GetVehicleModelAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.VehicleModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostVehicleModelAsync(Models.VehicleModelRequestNew vehicleModel)
        {
            var result = await vehicleModelsProvider.PostVehicleModelAsync(vehicleModel);

            if (result.IsSuccess)
            {
                return Created("", result.VehicleModel);
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> PutVehicleModelAsync(Models.VehicleModelRequestUpdate vehicleModel)
        {
            var result = await vehicleModelsProvider.PutVehicleModelAsync(vehicleModel);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleModelAsync(int id)
        {
            var result = await vehicleModelsProvider.DeleteVehicleModelAsync(id);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
