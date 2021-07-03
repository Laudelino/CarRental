using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.API.Person.Profiles
{
    public class PeopleProfile : AutoMapper.Profile
    {
        public PeopleProfile()
        {
            CreateMap<DB.Person, Models.Customer>();
            CreateMap<DB.Person, Models.Operator>();
        }
    }
}
