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
        private readonly IVehiclesProvider vehiclesProvider;

        public VehicleModelsController(IVehicleModelsProvider vehicleModelsProvider, IVehiclesProvider vehiclesProvider)
        {
            this.vehicleModelsProvider = vehicleModelsProvider;
            this.vehiclesProvider = vehiclesProvider;
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
        [HttpGet("/available")]
        public async Task<IActionResult> GetModelWithAvailableVehiclesAsync()
        {
            var result = await vehicleModelsProvider.GetModelWithAvailableVehiclesAsync();

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

        [HttpGet("{id}/vehicles")]
        public async Task<IActionResult> GetVehiclesByModelAsync(int id)
        {
            var result = await vehicleModelsProvider.GetVehiclesByModelAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Vehicles);
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
        
        [HttpPut("{id}/reserve")]
        public async Task<IActionResult> PutReserveVehicleByModelAsync(int id)
        {
            var result = await vehiclesProvider.PutReserveVehicleByModelAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Vehicle);
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
