using Microsoft.EntityFrameworkCore;

namespace RestASPNet.Controllers.Model.Context
{
    public class MSSQLContext : DbContext
    {
        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
    }
}
