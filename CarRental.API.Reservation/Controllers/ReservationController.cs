﻿using CarRental.API.Reservation.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationProvider reservationProvider;
        public ReservationController(IReservationProvider reservationProvider)
        {
            this.reservationProvider = reservationProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationAsync(int id)
        {
            var result = await reservationProvider.GetReservationAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Reservation);
            }
            return NotFound();
        }
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelReservationAsync(int id)
        {
            var result = await reservationProvider.CancelReservationAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Reservation);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> PostReserveAsync(Models.ReserveRequest reserveRequest)
        {
            var result = await reservationProvider.PostReserveAsync(reserveRequest);

            if (result.IsSuccess)
            {
                return Created("", result.Reservation);
            }
            return BadRequest();
        }
        [HttpPut("return")]
        public async Task<IActionResult> PutReservationReturnAsync(Models.ReservationReturn reservationReturn)
        {
            var result = await reservationProvider.PutReservationReturnAsync(reservationReturn);

            if (result.IsSuccess)
            {
                return Ok(result.Reservation);
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> PutManufacturerAsync(Models.ReservationRequestUpdate reservation)
        {
            var result = await reservationProvider.PutReservationAsync(reservation);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }
        [HttpPost("simulation")]
        public async Task<IActionResult> PostReserveSimulationAsync(Models.ReserveSimulationRequest simulationRequest)
        {
            var result = await reservationProvider.PostReserveSimulationAsync(simulationRequest);

            if (result.IsSuccess)
            {
                return Ok(result.ReserveSimulation);
            }
            return BadRequest();
        }
        [HttpGet("VehicleAvailablity/{VehicleId}")]
        public async Task<IActionResult> GetVehicleAvailabilityAsync(int VehicleId)
        {
            var result = await reservationProvider.GetVehicleAvailabilityAsync(VehicleId);

            if (result.IsSuccess)
            {
                return Ok(result.VehicleAvailability);
            }
            return NotFound();
        }
    }
}
