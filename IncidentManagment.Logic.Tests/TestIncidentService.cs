using IncidentManagment.Data;
using IncidentManagment.Data.Exceptions;
using IncidentManagment.Data.Models;
using IncidentManagment.Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace IncidentManagment.Logic.Tests;

public class TestIncidentService : IClassFixture<ContextFixture>
{
    private DbContextOptionsBuilder<IncidentContext> _optionsBuilder;
    public TestIncidentService(ContextFixture contextCreationFixture)
    {
        _optionsBuilder = contextCreationFixture.SetupInMemoryDatabase();
    }

    [Fact]
    public async Task CreatingAnIncidentBoundToNonExistentAccountThrowsValueNotFoundException()
    {
        using (var context = new IncidentContext(_optionsBuilder.Options))
        {
            context.Contacts.Add(new Contact { Email = "a", FirstName = "b", LastName = "c" });
            context.Accounts.Add(new Account { Name = "account", ContactId = "a" });
            context.Accounts.Add(new Account { Name = "account1", ContactId = "b" });
            await context.SaveChangesAsync();
        }
        
        using (var context = new IncidentContext(_optionsBuilder.Options))
        {
            var incidentService = new IncidentService(context);

            await Assert.ThrowsAsync<ValueNotFoundException>(async () => await incidentService.CreateIncidentAsync(new Incident(), "account2"));
        }
    }

    [Fact]
    public async Task CreatingAnIncidentBoundToExistingtAccountAddsSingleIncident()
    {
        using (var context = new IncidentContext(_optionsBuilder.Options))
        {
            context.Contacts.Add(new Contact { Email = "a", FirstName = "b", LastName = "c" });
            context.Accounts.Add(new Account { Name = "account", ContactId = "a" });
            context.Accounts.Add(new Account { Name = "account1", ContactId = "a" });
            await context.SaveChangesAsync();
        }

        using (var context = new IncidentContext(_optionsBuilder.Options))
        {
            var incidentService = new IncidentService(context);

            var createdIncident = await incidentService.CreateIncidentAsync(new Incident { Description = "test" }, "account1");

            Assert.Equal("account1", createdIncident.Account.Name);

            Assert.Contains(createdIncident, context.Incidents);

            var incs = await incidentService.GetIncidentsAsync();

            Assert.Single(incs);
        }
    }

}