using AutoMapper;
using IncidentManagment.Data.Models;
using IncidentManagment.DTOs;
using IncidentManagment.Logic.Interfaces;
using IncidentManagment.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace IncidentManagment.Controllers;

/// <summary>
/// Controller for managing accounts
/// </summary>
[ApiController]
[Route("[controller]")]
[ExceptionFilter]
public class AccountsController : Controller
{
    private readonly ILogger<AccountsController> _logger;
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    /// <summary>
    /// AccountsController constructor with injected logger, mapper and business logic facade
    /// </summary>
    /// <param name="logger">logger implementation compatible with ASP.NET interface</param>
    /// <param name="accountService">implementation of a service to control work with accounts, accounts and contacts</param>
    /// <param name="mapper">AutoMapper object</param>
    public AccountsController(ILogger<AccountsController> logger,
                               IAccountService accountService,
                               IMapper mapper) =>
        (_logger, _accountService, _mapper) = (logger, accountService, mapper);

    /// <summary>
    /// Reads all accounts 
    /// </summary>
    /// <returns>All accounts</returns>
    /// <response code="200">Returns all accounts</response>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await _accountService.GetAccountsAsync();
        var accountDTOs = _mapper.Map<IEnumerable<AccountDTO>>(accounts);
        return Ok(accountDTOs);
    }

    /// <summary>
    /// Reads an account with a given id
    /// </summary>
    /// <param name="id">Id to find an account</param>
    /// <returns></returns>
    /// <response code="204">If there is no account with a given Id</response>
    /// <response code="200">Returns an account with a given Id</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAccountById([FromRoute]string id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        var accountDTO = _mapper.Map<AccountDTO>(account);
        return Ok(accountDTO);
    }
    
    /// <summary>
    /// Creates an account and links to an existing contact
    /// </summary>
    /// <param name="account">account model to create</param>
    /// <returns>A newly created account</returns>
    /// <remarks>
    /// Sample request: 
    ///     Post /
    ///     {
    ///      "name": "AccountName",
    ///      "contactId": "ContactName"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created account</response>
    /// <response code="404">If the account with the given Id could not have been found</response>
    /// <response code="400">If the given model is not in the correct form</response>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAccount([FromBody]AccountDTO account, [FromServices] IDiagnosticContext diagnosticContext)
    {
        var addedAccount = await _accountService.AddAccountAsync(_mapper.Map<Account>(account));
        var addedAccountDTO = _mapper.Map<AccountDTO>(addedAccount);
        diagnosticContext.Set("accountId", addedAccount.Name);
        return Created(HttpContext.Request.GetDisplayUrl(), addedAccountDTO);
    }
}
