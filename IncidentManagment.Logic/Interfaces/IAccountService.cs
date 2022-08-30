using IncidentManagment.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagment.Logic.Interfaces;
public interface IAccountService
{
    Task<Account> AddAccountAsync(Account account);
    Task<Account> GetAccountByIdAsync(string id);
    Task<IEnumerable<Account>> GetAccountsAsync();
}
