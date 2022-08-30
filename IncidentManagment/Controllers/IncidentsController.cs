using AutoMapper;
using IncidentManagment.Data.Models;
using IncidentManagment.DTOs;
using IncidentManagment.Filters;
using IncidentManagment.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SerilogTimings;

namespace IncidentManagment.Controllers;

/// <summary>
/// Controller for managing incident
/// </summary>
[ApiController]
[Route("[controller]")]
[ExceptionFilter]
public class IncidentsController : Controller
{
    private readonly ILogger<IncidentsController> _logger;
    private readonly IIncidentService _incidentService;
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;

    /// <summary>
    /// IncidentController constructor with injected logger, mapper and business logic facade
    /// </summary>
    /// <param name="logger">logger implementation compatible with ASP.NET interface</param>
    /// <param name="incidentService">implementation of a service to control work with incidents</param>
    /// <param name="contactService">implementation of a service to control work with contacts</param>
    /// <param name="mapper">AutoMapper object</param>
    public IncidentsController(ILogger<IncidentsController> logger,
                               IIncidentService incidentService,
                               IContactService contactService,
                               IMapper mapper) =>
        (_logger, _incidentService, _contactService, _mapper) = (logger, incidentService, contactService, mapper);

    /// <summary>
    /// Reads all incidents 
    /// </summary>
    /// <returns>All incidents</returns>
    /// <response code="200">Returns all incidents</response>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var incidents = await _incidentService.GetIncidentsAsync();
        var incidentDTOs = _mapper.Map<IEnumerable<IncidentDTO>>(incidents);
        return Ok(incidentDTOs);
    }

    /// <summary>
    /// Reads an incident with a given id
    /// </summary>
    /// <param name="id">Id to find an incident</param>
    /// <returns></returns>
    /// <response code="204">If there is no incident with a given Id</response>
    /// <response code="200">Returns an incident with a given Id</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetById([FromRoute]string id)
    {
        var incident = await _incidentService.GetIncidentById(id);
        var incidentDTO = _mapper.Map<IncidentDTO>(incident);
        return Ok(incidentDTO);
    }
    
    /// <summary>
    /// Creates an incident, links to a given contact
    /// </summary>
    /// <param name="incidentCreation">model for linking contact and creating an incident</param>
    /// <returns>A newly created incident</returns>
    /// <remarks>
    /// Sample request: 
    ///     Post /
    ///     {
    ///      "accountId": "AccountName",
    ///      "cFirstName": "Contact's First Name",
    ///      "cLastName": "Contact's Last Name",
    ///      "cEmail": "ContactsEmail@mail.com",
    ///      "incidentDescription": "Description of an incident to be added"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created incident</response>
    /// <response code="404">If the account with the given Id could not have been found</response>
    /// <response code="400">If the given model is not in the correct form</response>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] IncidentCreationDTO incidentCreation, [FromServices] IDiagnosticContext diagContext)
    {
        using var timing = Operation.Time("Linking Contact and creating a new incident");

        var contact = _mapper.Map<IncidentCreationDTO, Contact>(incidentCreation);
        var incident = _mapper.Map<IncidentCreationDTO, Incident>(incidentCreation);

        diagContext.Set("accountId", incidentCreation.AccountId);

        contact = await _contactService.LinkContactToAccountAsync(contact, incidentCreation.AccountId);

        incident = await _incidentService.CreateIncidentAsync(incident, incidentCreation.AccountId);

        return Ok(_mapper.Map<IncidentDTO>(incident));
    }

    [HttpDelete("")]
    public async Task<IActionResult> Delete(int incidentId)
    {
        throw new NotImplementedException();
    }
}