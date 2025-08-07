using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace At.luki0606.DartZone.API.Data
{
    public class DartZoneDbContextFactory : IDesignTimeDbContextFactory<DartZoneDbContext>
    {
        public DartZoneDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            DbContextOptionsBuilder<DartZoneDbContext> optionsBuilder = new();
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);
            return new DartZoneDbContext(optionsBuilder.Options);
        }
    }
}
