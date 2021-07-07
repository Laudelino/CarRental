using AutoMapper;
using CarRental.API.Reservation.DB;
using CarRental.API.Reservation.Interfaces;
using CarRental.API.Reservation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Providers
{
    public class ReservationProvider : IReservationProvider
    {
        private readonly ReservationsDbContext dbContext;
        private readonly ILogger<ReservationProvider> logger;
        private readonly IMapper mapper;
        private readonly IHttpClientFactory _clientFactory;

        public ReservationProvider(ReservationsDbContext dBContext, ILogger<ReservationProvider> logger, IMapper mapper, IHttpClientFactory clientFactory)
        {
            this.dbContext = dBContext;
            this.logger = logger;
            this.mapper = mapper;
            _clientFactory = clientFactory;

            SeedData();
        }
        private void SeedData()
        {
            if (!dbContext.Reservations.Any())
            {
                dbContext.Reservations.Add(new DB.Reservation() { Id = 1, VehicleId = 1 });
                dbContext.Reservations.Add(new DB.Reservation() { Id = 2, VehicleId = 2 });
                dbContext.Reservations.Add(new DB.Reservation() { Id = 3, VehicleId = 3 });

                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> CancelReservationAsync(int id)
        {
            try
            {
                var find = await this.GetReservationAsync(id);

                if (find.IsSuccess)
                {
                    var reserveRequest = new VehicleReserveRequest
                    {
                        Id = find.Reservation.VehicleId,
                        IsReserved = false
                    };

                    var changeVehicleReserve = await this.ChangeReserveVehicleAsync(reserveRequest);
                    
                    if(!changeVehicleReserve.IsSuccess)
                        return (false, null, "Failed to cancel the reservation");

                    var dbreservation = await dbContext.Reservations.FirstOrDefaultAsync(c => c.Id == find.Reservation.Id);

                    dbreservation.Status = ReservationStatus.Cancelled;

                    dbContext.Update(dbreservation);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var resultreservation = mapper.Map<DB.Reservation, Models.Reservation>(dbreservation);
                        return (true, resultreservation, null);
                    }
                    return (false, null, "Failed to update the record");
                }
                return (false, null, "Failed to find record");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> GetReservationAsync(int id)
        {
            try
            {
                var fuelTypes = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id);

                if (fuelTypes != null)
                {
                    var result = mapper.Map<DB.Reservation, Models.Reservation>(fuelTypes);
                    return (true, result, null);
                }
                return (false, null, "Not Found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string ReservationContract, string ErrorMessage)> GetReservationContractAsync(int id)
        {
            try
            {
                var reservation = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id);

                if (reservation != null)
                {
                    var vehicle = await this.GetVehicleAsync(reservation.VehicleId);
                    if(!vehicle.IsSuccess)
                        return (false, null, "Not able to generate the contract");

                    var customer = await this.ValidateCustomer(reservation.CustomerCPF);
                    if (!customer.IsSuccess)
                        return (false, null, "Not able to generate the contract");

                    var result = mapper.Map<DB.Reservation, Models.Reservation>(reservation);

                    string reservationContract =
                        $"Contrato de Aluguel de Veiculo\n\n" +
                        $"Locatario: {customer.Customer.Name}, portador do CPF {customer.Customer.CPF}\n\n" +
                        $"Veículo alugado de modelo {vehicle.Vehicle.VehicleModel.Name}, ano {vehicle.Vehicle.Year} e placa {vehicle.Vehicle.Plate}\n\n" +
                        $"Reserva do veiculo com inicio em {reservation.ReservationStart} e fim em {reservation.ReservationEnd}\n\n" +
                        $"Reserva tem um valor total estimado de {reservation.EstimatedTotal}\n\n" +
                        $"Um adicional de 30% ao valor será adicionado a cada ocorrência de carro não limpo, tanque de combustivél não cheio, amassados e arranhões a ser avaliado no check-list de devolução do veículo";


                    return (true, reservationContract, null);
                }
                return (false, null, "Not Found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.Reservation> Reservations, string ErrorMessage)> GetReservationByCustomerAsync(string CustomerCPF)
        {
            try
            {
                var reservations = await dbContext.Reservations.Where(c => c.CustomerCPF == CustomerCPF).ToListAsync();

                if (reservations != null)
                {
                    var result = mapper.Map< IEnumerable<DB.Reservation>, IEnumerable< Models.Reservation>>(reservations);
                    foreach(Models.Reservation r in result)
                    {
                        var v = await this.GetVehicleAsync(r.VehicleId);
                        if (v.IsSuccess)
                            r.Vehicle = v.Vehicle;
                    }
                    return (true, result, null);
                }
                return (false, null, "Not Found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, VehicleAvailability VehicleAvailability, string ErrorMessage)> GetVehicleAvailabilityAsync(int vehicleId)
        {
            try
            {
                var reservations = await dbContext.Reservations.Where(v => v.VehicleId == vehicleId).Where(s => s.Status == ReservationStatus.Active).ToListAsync();

                if (reservations != null)
                {
                    if(reservations.Count() != 0)
                        return (true, new VehicleAvailability { Id = vehicleId, IsAvailable = false }, null);
                    if (reservations.Count() == 0)
                        return (true, new VehicleAvailability { Id = vehicleId, IsAvailable = true }, null);
                }
                return (true, new VehicleAvailability { Id = vehicleId, IsAvailable = true }, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PutReservationReturnAsync(ReservationReturn reserve)
        {
            decimal additionalCostPercentage = 0.3M;
            decimal additionalCostCalc = 0.0M;
            try
            {
                var find = await this.GetReservationAsync(reserve.Id);

                if (!find.IsSuccess)
                    return (false, null, "Invalid Reservation");

                if (find.IsSuccess)
                {
                    if (reserve.ReturnDate < find.Reservation.ReservationStart)
                        return (false, null, "Invalid return date");

                    var reserveRequest = new VehicleReserveRequest
                    {
                        Id = find.Reservation.VehicleId,
                        IsReserved = false
                    };

                    var changeVehicleReserve = await this.ChangeReserveVehicleAsync(reserveRequest);

                    if (!changeVehicleReserve.IsSuccess)
                        return (false, null, "Failed to return the vehicle");

                    var dbreservation = await dbContext.Reservations.FirstOrDefaultAsync(c => c.Id == reserve.Id);
                    
                    dbreservation.IsClean = reserve.IsClean;
                    dbreservation.HasFullTank = reserve.HasFullTank;
                    dbreservation.HasScratches = reserve.HasScratches;
                    dbreservation.HasDents = reserve.HasDents;
                    dbreservation.ReturnDate = reserve.ReturnDate;
                    dbreservation.Status = ReservationStatus.Completed;


                    if (!reserve.IsClean) additionalCostCalc += additionalCostPercentage;
                    if (!reserve.HasFullTank) additionalCostCalc += additionalCostPercentage;
                    if (!reserve.HasScratches) additionalCostCalc += additionalCostPercentage;
                    if (!reserve.HasDents) additionalCostCalc += additionalCostPercentage;

                    dbreservation.ReturnTotal = dbreservation.EstimatedTotal + (additionalCostCalc * dbreservation.EstimatedTotal);

                    dbContext.Update(dbreservation);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var resultreservation = mapper.Map<DB.Reservation, Models.Reservation>(dbreservation);
                        return (true, resultreservation, null);
                    }
                    return (false, null, "Failed to update the record");
                }
                return (false, null, "Failed to find record");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PostReserveAsync(ReserveRequest reserve)
        {
            try
            {
                if (!string.IsNullOrEmpty(reserve.CustomerCPF))
                {
                    var customer = await this.ValidateCustomer(reserve.CustomerCPF);

                    if(!customer.IsSuccess)
                        return (false, null, "Invalid Customer");
                    if (reserve.ReservationEnd < reserve.ReservationStart)
                        return (false, null, "Reservation could not be created, verify your information");

                    if (customer.IsSuccess)
                    {
                        var vehicle = await this.ReserveVehicleByModel(reserve.ModelId);
                        
                        if(!vehicle.IsSuccess)
                            return (false, null, "Failed to reserve a vehicle");

                        if (vehicle.IsSuccess)
                        {
                            var dbreservation = new DB.Reservation()
                            {
                                CustomerCPF = customer.Customer.CPF,
                                VehicleId = vehicle.Vehicle.Id,
                                ReservationStart = reserve.ReservationStart,
                                ReservationEnd = reserve.ReservationEnd,
                                RentalRate = reserve.RentalRate,
                                EstimatedTotal = reserve.EstimatedTotal,
                                Status = ReservationStatus.Active
                            };
                            dbContext.Add(dbreservation);

                            var result = await dbContext.SaveChangesAsync();

                            if (result > 0)
                            {
                                var resultReservation = mapper.Map<DB.Reservation, Models.Reservation>(dbreservation);
                                return (true, resultReservation, null);
                            }
                        }
                    }
                    return (false, null, "Failed to create the record");
                }
                return (false, null, "Failed to create the record");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, ReserveSimulation ReserveSimulation, string ErrorMessage)> PostReserveSimulationAsync(ReserveSimulationRequest simulation)
        {
            try
            {
                
                int totalHours = (int)Math.Round((simulation.ReservationEnd.Subtract(simulation.ReservationStart).TotalSeconds)/3600);
                
                if(totalHours < 1)
                    return (false, null, "Invalid Reservation dates");

                var result = await GetModelRentalRate(simulation.ModelId); 
                if (result.IsSuccess)
                {
                    decimal rentalRate = result.ModelRentalRate;
                    var reserveSimulation = new Models.ReserveSimulation
                    {
                        ModelId = simulation.ModelId,
                        RentalRate = rentalRate,
                        EstimatedTotal = rentalRate * totalHours,
                        ReservationStart = simulation.ReservationStart,
                        ReservationEnd = simulation.ReservationEnd
                    };

                    return (true, reserveSimulation, null);
                }
                return (false, null, "Internal Error");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, Models.Reservation Reservation, string ErrorMessage)> PutReservationAsync(ReservationRequestUpdate reserve)
        {
            try
            {
                var find = await this.GetReservationAsync(reserve.Id);

                if(reserve.ReservationEnd < reserve.ReservationStart)
                    return (false, null, "Reservation could not be created, verify your information");
                if (!find.IsSuccess)
                    return (false, null, "Reservation was not found");

                if (find.IsSuccess)
                {
                    var dbreservation = await dbContext.Reservations.FirstOrDefaultAsync(c => c.Id == reserve.Id);

                    if (!string.IsNullOrEmpty(reserve.CustomerCPF))
                    {
                        var customer = await this.ValidateCustomer(reserve.CustomerCPF);
                        if(customer.IsSuccess)
                            dbreservation.CustomerCPF = customer.Customer.CPF;
                    }
                    //dbreservation.VehicleId = reserve.VehicleId;                    
                    dbreservation.ReservationStart = reserve.ReservationStart;
                    dbreservation.ReservationEnd = reserve.ReservationEnd;
                    dbreservation.RentalRate = reserve.RentalRate;
                    dbreservation.EstimatedTotal = reserve.EstimatedTotal;
                    dbreservation.IsClean = reserve.IsClean;
                    dbreservation.HasFullTank = reserve.HasFullTank;
                    dbreservation.HasScratches = reserve.HasScratches;
                    dbreservation.HasDents = reserve.HasDents;
                    dbreservation.ReturnDate = reserve.ReturnDate;
                    dbreservation.Status = string.IsNullOrEmpty(reserve.Status) ? dbreservation.Status : reserve.Status;
                    dbreservation.ReturnTotal = reserve.ReturnTotal;
                    
                    dbContext.Update(dbreservation);

                    var result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var resultreservation = mapper.Map<DB.Reservation, Models.Reservation>(dbreservation);
                        return (true, resultreservation, null);
                    }
                    return (false, null, "Failed to update the record");
                }
                return (false, null, "Failed to find record");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private async Task<(bool IsSuccess, decimal ModelRentalRate, string ErrorMessage)> GetModelRentalRate(int ModelId)
        {
            try
            {
                var client = _clientFactory.CreateClient("CarRentalAPI");
                var response = await client.GetAsync($"models/{ModelId}/");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var vehicleModel = JsonSerializer.Deserialize<VehicleModel>(content, options);
                    return (true, vehicleModel.RentalRate, null);
                }
                else
                {
                    return (false, -1, "Internal error");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, -1, ex.Message);
            }
        }
        private async Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> ValidateCustomer(string CustomerCPF)
        {
            try
            {
                if (!string.IsNullOrEmpty(CustomerCPF))
                {
                    var client = _clientFactory.CreateClient("CarRentalAPI");
                    var response = await client.GetAsync($"customer/{CustomerCPF}/");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();
                        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        var customer = JsonSerializer.Deserialize<Customer>(content, options);
                        return (true, customer, null);
                    }
                    return (false, null, "Invalid CPF");
                }
                else
                {
                    return (false, null, "Internal error");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        private async Task<(bool IsSuccess, Vehicle Vehicle, string ErrorMessage)> ReserveVehicleByModel(int ModelId)
        {
            try
            {
                var client = _clientFactory.CreateClient("CarRentalAPI");
                var response = await client.PutAsync($"models/{ModelId}/reserve", null);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var vehicle = JsonSerializer.Deserialize<Vehicle>(content, options);
                    return (true, vehicle, null);
                }
                return (false, null, "Internal error");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        private async Task<(bool IsSuccess, Vehicle Vehicle, string ErrorMessage)> ChangeReserveVehicleAsync(VehicleReserveRequest reserveRequest)
        {
            try
            {
                var client = _clientFactory.CreateClient("CarRentalAPI");

                var requestContent = JsonSerializer.Serialize(reserveRequest);
                var buffer = System.Text.Encoding.UTF8.GetBytes(requestContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                
                var response = await client.PutAsync($"vehicles/reserve", byteContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var vehicle = JsonSerializer.Deserialize<Vehicle>(content, options);
                    return (true, vehicle, null);
                }
                return (false, null, "Internal error");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        private async Task<(bool IsSuccess, Vehicle Vehicle, string ErrorMessage)> GetVehicleAsync(int VehicleId)
        {
            try
            {
                var client = _clientFactory.CreateClient("CarRentalAPI");
                var response = await client.GetAsync($"vehicles/{VehicleId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var vehicle = JsonSerializer.Deserialize<Vehicle>(content, options);
                    return (true, vehicle, null);
                }
                return (false, null, "Internal error");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
