using CarRental.API.Reservation.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet("{id}/contract")]
        public async Task<IActionResult> GetReservationContractAsync(int id)
        {
            try
            {
                var result = await reservationProvider.GetReservationContractAsync(id);

                if (result.IsSuccess)
                {
                    
                    var workStream = new MemoryStream();
                    using (var pdfWriter = new PdfWriter(workStream))
                    {
                        pdfWriter.SetCloseStream(false);
                        using (var pdfdocument = new PdfDocument(pdfWriter))
                        {
                            var document = new Document(pdfdocument);
                            document.Add(new Paragraph(result.ReservationContract));
                        }
                    }

                    workStream.Position = 0;
                    return new FileStreamResult(workStream, "application/pdf");
                }
                return NotFound(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }
        [HttpPut("{id}/cancel")]
        [Authorize]
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
        [Authorize]
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
        [Authorize(Roles = "Operator")]
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
        [Authorize]
        public async Task<IActionResult> PutReservationAsync(Models.ReservationRequestUpdate reservation)
        {
            var result = await reservationProvider.PutReservationAsync(reservation);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }
        [HttpPost("simulation")]
        [Authorize]
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
