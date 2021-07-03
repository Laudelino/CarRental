using CarRental.API.Vehicles.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Controllers
{
    [ApiController]
    [Route("api/vehiclecategories")]
    public class VehicleCategoriesController : ControllerBase
    {
        private readonly IVehicleCategoriesProvider vehicleCategoriesProvider;

        public VehicleCategoriesController(IVehicleCategoriesProvider vehicleCategoriesProvider)
        {
            this.vehicleCategoriesProvider = vehicleCategoriesProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicleCategoriesAsync()
        {
            var result = await vehicleCategoriesProvider.GetVehicleCategoriesAsync();

            if (result.IsSuccess)
            {
                return Ok(result.VehicleCategories);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleCategoryAsync(int id)
        {
            var result = await vehicleCategoriesProvider.GetVehicleCategoryAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.VehicleCategory);
            }
            return NotFound();
        }

        [HttpGet("{id}/models")]
        public async Task<IActionResult> GetVehicleModelsByCategoryAsync(int id)
        {
            var result = await vehicleCategoriesProvider.GetVehicleModelsByCategoryAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.VehicleModels);
            }
            return NotFound();
        }
    }
}
