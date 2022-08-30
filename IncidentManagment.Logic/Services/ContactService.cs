using IncidentManagment.Data;
using IncidentManagment.Data.Exceptions;
using IncidentManagment.Data.Models;
using IncidentManagment.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagment.Logic.Services;
public class ContactService : IContactService
{
    private readonly IncidentContext _incidentContext;

    public ContactService(IncidentContext context) => _incidentContext = context;

    public async Task<Contact> AddContactAsync(Contact contact)
    {
        var addedContact = _incidentContext.Contacts.Add(contact);
        await _incidentContext.SaveChangesAsync();

        return addedContact.Entity;
    }

    public async Task<Contact> GetContactByIdAsync(string id)
    {
        var foundContact = await _incidentContext.Contacts.FindAsync(id);

        if (foundContact is null)
            throw new NoContentException("Contact with given id could not have been found", "id");

        return foundContact;
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await _incidentContext.Contacts.ToListAsync();
    }

    public async Task<Contact> LinkContactToAccountAsync(Contact contact, string accountId)
    {
        var foundAccount = await _incidentContext.Accounts.FindAsync(accountId);
        if (foundAccount is null)
            throw new ValueNotFoundException("Account with given account id could not have been found", "accountId");

        var foundContact = await _incidentContext.Contacts.FindAsync(contact.Email);

        if (foundContact is not null)
        {
            (foundContact.Email, foundContact.FirstName, foundContact.LastName) = (contact.Email, contact.FirstName, contact.LastName);
            foundAccount.ContactId = foundContact.Email;
        }
        else
        {
            foundContact = contact;
            foundContact.Accounts.Add(foundAccount);
            _incidentContext.Entry(foundAccount).State = EntityState.Modified;
            _incidentContext.Entry(foundContact).State = EntityState.Added;
        }
        await _incidentContext.SaveChangesAsync();

        return foundContact;
    }
}
