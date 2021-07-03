using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Reservation.Profiles
{
    public class ReservationProfile : AutoMapper.Profile
    {
        public ReservationProfile()
        {
            CreateMap<DB.Reservation, Models.Reservation>();
        }
    }
}
