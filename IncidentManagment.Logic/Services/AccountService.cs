using IncidentManagment.Data;
using IncidentManagment.Data.Exceptions;
using IncidentManagment.Data.Models;
using IncidentManagment.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagment.Logic.Services;
public class AccountService : IAccountService
{
    private readonly IncidentContext _incidentContext;

    public AccountService(IncidentContext context) => _incidentContext = context;
    
    public async Task<Account> AddAccountAsync(Account account)
    {
        if (await _incidentContext.Contacts.FindAsync(account.ContactId) is null)
            throw new ValueNotFoundException("Couldn't add an account, contact with given id could not have been found", nameof(account.ContactId));

        var addedAccount = _incidentContext.Accounts.Add(account);
        await _incidentContext.SaveChangesAsync();
        return addedAccount.Entity;
    }

    public async Task<Account> GetAccountByIdAsync(string id)
    {
        var foundAccount = await _incidentContext.Accounts.FindAsync(id);

        if (foundAccount is null)
            throw new NoContentException("Account with given id could not have been found", nameof(id));

        return foundAccount;
    }

    public async Task<IEnumerable<Account>> GetAccountsAsync()
    {
        return await _incidentContext.Accounts.ToListAsync();
    }

    public async Task<Account> DeleteIncidentAsync(string id)
    {
        var foundAccount = await _incidentContext.Accounts.FindAsync(id);

        if (foundAccount is null)
            throw new NoContentException("Account with given id could not have been found", nameof(id));

        try
        {
            _incidentContext.Accounts.Remove(foundAccount);

            await _incidentContext.SaveChangesAsync();
        }
        catch(DbUpdateException ex)
        {
            throw new UnprocessableException(ex.Message);
        }

        return foundAccount;
    }
}
