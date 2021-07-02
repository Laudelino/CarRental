using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Profiles
{
    public class VehicleProfile : AutoMapper.Profile
    {
        public VehicleProfile()
        {
            CreateMap<DB.Manufacturer, Models.Manufacturer>();
            CreateMap<DB.VehicleCategory, Models.VehicleCategory>();
            CreateMap<DB.FuelType, Models.FuelType>();
        }
    }
}
