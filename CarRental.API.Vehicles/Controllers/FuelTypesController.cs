using CarRental.API.Vehicles.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Controllers
{
    [ApiController]
    [Route("api/fueltypes")]
    public class FuelTypesController : ControllerBase
    {
        private readonly IFuelTypesProvider fueltypesProvider;

        public FuelTypesController(IFuelTypesProvider fueltypesProvider)
        {
            this.fueltypesProvider = fueltypesProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetFuelTypesAsync()
        {
            var result = await fueltypesProvider.GetFuelTypesAsync();

            if (result.IsSuccess)
            {
                return Ok(result.FuelTypes);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFuelTypeAsync(int id)
        {
            var result = await fueltypesProvider.GetFuelTypeAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.FuelType);
            }
            return NotFound();
        }

        [HttpGet("{id}/models")]
        public async Task<IActionResult> GetVehicleModelsByFuelTypeAsync(int id)
        {
            var result = await fueltypesProvider.GetVehicleModelsByFuelTypeAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.VehicleModels);
            }
            return NotFound();
        }
    }
}
