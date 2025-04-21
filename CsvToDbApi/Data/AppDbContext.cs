using CsvToDbApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CsvToDbApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Person> People { get; set; }
    }
}
