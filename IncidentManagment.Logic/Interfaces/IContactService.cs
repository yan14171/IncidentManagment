using IncidentManagment.Data.Models;

namespace IncidentManagment.Logic.Interfaces;
public interface IContactService
{
    Task<Contact> AddContactAsync(Contact contact);
    Task<Contact> DeleteIncidentAsync(string id);
    Task<Contact> GetContactByIdAsync(string id);
    Task<IEnumerable<Contact>> GetContactsAsync();
    Task<Contact> LinkContactToAccountAsync(Contact contact, string accountId);
    
}
