using CarRental.API.Vehicles.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CarRental.API.Vehicles.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehiclesProvider vehiclesProvider;

        public VehiclesController(IVehiclesProvider vehiclesProvider)
        {
            this.vehiclesProvider = vehiclesProvider;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetVehiclesAsync()
        {
            var result = await vehiclesProvider.GetVehiclesAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Vehicles);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleAsync(int id)
        {
            var result = await vehiclesProvider.GetVehicleAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Vehicle);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Operator")]
        public async Task<IActionResult> PostVehicleAsync(Models.VehicleRequestNew vehicle)
        {
            var result = await vehiclesProvider.PostVehicleAsync(vehicle);

            if (result.IsSuccess)
            {
                return Created("", result.Vehicle);
            }
            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Operator")]
        public async Task<IActionResult> PutVehicleAsync(Models.VehicleRequestUpdate vehicle)
        {
            var result = await vehiclesProvider.PutVehicleAsync(vehicle);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Operator")]
        public async Task<IActionResult> DeleteVehicleAsync(int id)
        {
            var result = await vehiclesProvider.DeleteVehicleAsync(id);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }
       
        [HttpPut("/reserve")]
        public async Task<IActionResult> PutChangeReserveVehicleAsync(Models.VehicleReserveRequest reserveRequest)
        {
            var result = await vehiclesProvider.PutChangeReserveVehicleAsync(reserveRequest);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }

    }
}
