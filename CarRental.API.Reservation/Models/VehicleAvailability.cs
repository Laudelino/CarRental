﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Models
{
    public class VehicleAvailability
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
    }
}
