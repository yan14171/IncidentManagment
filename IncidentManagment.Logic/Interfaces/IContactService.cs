using IncidentManagment.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagment.Logic.Interfaces;
public interface IContactService
{
    Task<Contact> AddContactAsync(Contact contact);
    Task<Contact> GetContactByIdAsync(string id);
    Task<IEnumerable<Contact>> GetContactsAsync();
    Task<Contact> LinkContactToAccountAsync(Contact contact, string accountId);
    
}
