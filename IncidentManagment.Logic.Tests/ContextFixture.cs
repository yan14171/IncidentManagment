
using IncidentManagment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IncidentManagment.Logic.Tests;
public class ContextFixture : IDisposable
{

    public DbContextOptionsBuilder<IncidentContext> SetupInMemoryDatabase()
        => new DbContextOptionsBuilder<IncidentContext>().UseInMemoryDatabase("IncidentTestingDatabase", new InMemoryDatabaseRoot());

    public void Dispose()
    {
    }
}