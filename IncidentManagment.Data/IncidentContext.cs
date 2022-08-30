using IncidentManagment.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Reflection;

namespace IncidentManagment.Data;
public class IncidentContext : DbContext
{
    public IncidentContext(DbContextOptions<IncidentContext> opts) : base(opts){ }

    public DbSet<Incident> Incidents { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating (ModelBuilder builder)
    { 
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

public class IncidentContextFactory : IDesignTimeDbContextFactory<IncidentContext>
{
    public IncidentContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<IncidentContext>();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(n => n.AddConsole()));
        return new IncidentContext(optionsBuilder.Options);
    }
}