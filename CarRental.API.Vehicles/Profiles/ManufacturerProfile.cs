using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Vehicles.Profiles
{
    public class ManufacturerProfile : AutoMapper.Profile
    {
        public ManufacturerProfile()
        {
            CreateMap<DB.Manufacturer, Models.Manufacturer>();
        }
    }
}
