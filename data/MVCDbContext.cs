using Microsoft.EntityFrameworkCore;
using test_application_dotnet.Models.Domain;

namespace test_application_dotnet.data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employees> Employees { get; set; }
    }
}
