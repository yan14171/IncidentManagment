using IncidentManagment.Data.Exceptions;
using IncidentManagment.Data.Models;
using IncidentManagment.Data;
using IncidentManagment.Logic.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncidentManagment.Logic.Interfaces;

namespace IncidentManagment.Logic.Tests;
public class TestContactService : IClassFixture<ContextFixture>
{
    private DbContextOptionsBuilder<IncidentContext> _optionsBuilder;
    public TestContactService(ContextFixture contextCreationFixture)
    {
        _optionsBuilder = contextCreationFixture.SetupInMemoryDatabase();
    }
    
    [Fact]
    public async Task LinkingContactToNonExistentAccountThrowsValueNotFoundException()
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
            var contactService = new ContactService(context);

            await Assert.ThrowsAsync<ValueNotFoundException>(async () => await contactService.LinkContactToAccountAsync(new Contact(), "account2"));
        }
    }

    [Fact]
    public async Task LinkingNonExistingContactToExistingAccountCreatesOneNewContactAndLinksItToAccount()
    {
        using (var context = new IncidentContext(_optionsBuilder.Options))
        {
            context.Contacts.Add(new Contact { Email = "a", FirstName = "b", LastName = "c" });
            context.Accounts.Add(new Account { Name = "account", ContactId = "a" });
            context.Accounts.Add(new Account { Name = "account1", ContactId = "b" });
            await context.SaveChangesAsync();
        }

        using(var context = new IncidentContext(_optionsBuilder.Options))
        {
            var contactService = new ContactService(context);

            await contactService.LinkContactToAccountAsync(new Contact { Email = "b", FirstName = "John", LastName = "Sam" }, "account");

            var cnts = await contactService.GetContactsAsync();

            Assert.Equal(2, cnts.Count());

            var addedContact = cnts.FirstOrDefault(n => n.Email == "b");

            Assert.NotNull(addedContact);

            Assert.Equal("John", addedContact.FirstName);
        }
    }

    [Fact]
    public async Task LinkingExistingContactToExistingAccountChangesOldContactAndLinksItToAccount()
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
            var contactService = new ContactService(context);

            await contactService.LinkContactToAccountAsync(new Contact { Email = "a", FirstName = "John", LastName = "Sam" }, "account");

            var cnts = await contactService.GetContactsAsync();

            Assert.Single(cnts);

            var addedContact = cnts.FirstOrDefault(n => n.Email == "a");

            Assert.NotNull(addedContact);

            Assert.Equal("John", addedContact.FirstName);
        }
    }
}
