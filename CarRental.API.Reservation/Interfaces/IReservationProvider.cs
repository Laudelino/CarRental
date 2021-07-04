using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.API.Reservation.Models;

namespace CarRental.API.Reservation.Interfaces
{
    public interface IReservationProvider
    {
        Task<(bool IsSuccess, ReserveSimulation ReserveSimulation, string ErrorMessage)> PostReserveSimulationAsync(ReserveSimulationRequest simulation);
        Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PostReserveAsync(ReserveRequest reserve);
        Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PutReservationReturnAsync(ReservationReturn reserve);
        Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PutReservationAsync(ReservationRequestUpdate reserve);
        Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> CancelReservationAsync(int id);
        Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> GetReservationAsync(int id);
        Task<(bool IsSuccess, IEnumerable<Models.Reservation> Reservations, string ErrorMessage)> GetReservationByCustomerAsync(string CustomerCPF);
        Task<(bool IsSuccess, VehicleAvailability VehicleAvailability, string ErrorMessage)> GetVehicleAvailabilityAsync(int vehicleId);
        Task<(bool IsSuccess, string ReservationContract, string ErrorMessage)> GetReservationContractAsync(int id);
    }
}
