using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Person.DB
{
    public class PersonDbContext : IdentityDbContext<Person>
    {
        public DbSet<Person> People { get; set; }

        public PersonDbContext(DbContextOptions options) : base(options)
        { }
    }
}
