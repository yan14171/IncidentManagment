using IncidentManagment.Data;
using IncidentManagment.Data.Exceptions;
using IncidentManagment.Data.Models;
using IncidentManagment.Logic.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagment.Logic.Tests;
public class TestAccountService : IClassFixture<ContextFixture>
{
    private DbContextOptionsBuilder<IncidentContext> _optionsBuilder;
    public TestAccountService(ContextFixture contextCreationFixture)
    {
        _optionsBuilder = contextCreationFixture.SetupInMemoryDatabase();
    }


    [Fact]
    public async Task AddingAccountWithInvalidContactIdThrowsValueNotFoundException()
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
            var accountService = new AccountService(context);

            await Assert.ThrowsAsync<ValueNotFoundException>(async () => await accountService.AddAccountAsync(new Account { ContactId = "b" }));
        }
    }

    [Fact]
    public async Task AddingAccountWithValidContactIdAddsAnAccountWithGivenProps()
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
            var accountService = new AccountService(context);

            var addedIncidents = new List<Incident> { new Incident { Description = "Included incident" } };
            var addedAccount = new Account { Name = "b", ContactId = "a", Incidents = addedIncidents };

            await accountService.AddAccountAsync(addedAccount);

            var accs = await accountService.GetAccountsAsync();

            Assert.Equal(3, accs.Count());

            addedAccount = accs.FirstOrDefault(n => n.Name == "b");

            Assert.NotNull(addedAccount);

            Assert.Single(addedAccount.Incidents);
        }
    }
}
