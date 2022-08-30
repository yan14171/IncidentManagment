using Castle.Core.Logging;
using IncidentManagment.Controllers;
using IncidentManagment.Data.Models;
using IncidentManagment.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace IncidentManagment.Tests;

public class TestAccountsController
{
    [Fact]
    public void GetAllReturnsOKResultWithAccountDTOModels()
    {
        var loggerMoq = new Mock<ILogger<AccountsController>>();
        var accountServiceMoq = new Mock<IAccountService>();
        accountServiceMoq
            .Setup(x => x.GetAccountsAsync())
            .ReturnsAsync(
            new List<Account>
            {
                new Account 
                { 
                    Name = "A", 
                    Incidents = new List<Incident> { new Incident { Description = "This description should not be in a final result"} } 
                },
                new Account
                {
                    Name = "B"
                }
            });

        // а насколько єто надо вообще, если все равно я или все мокаю или єто уже интеграционніе
    }
}