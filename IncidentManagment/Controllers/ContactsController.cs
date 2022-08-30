using AutoMapper;
using IncidentManagment.Data.Models;
using IncidentManagment.DTOs;
using IncidentManagment.Logic.Interfaces;
using IncidentManagment.Logic.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using IncidentManagment.Filters;

namespace IncidentManagment.Controllers;

[ApiController]
[Route("[controller]")]
[ExceptionFilter]
public class ContactsController : Controller
{
    private readonly ILogger<ContactsController> _logger;
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;

    /// <summary>
    /// AccountsController constructor with injected logger, mapper and business logic facade
    /// </summary>
    /// <param name="logger">logger implementation compatible with ASP.NET interface</param>
    /// <param name="contactService">implementation of a service to control work with contacts, accounts and contacts</param>
    /// <param name="mapper">AutoMapper object</param>
    public ContactsController(ILogger<ContactsController> logger,
                               IContactService contactService,
                               IMapper mapper) =>
        (_logger, _contactService, _mapper) = (logger, contactService, mapper);

    /// <summary>
    /// Reads all contacts 
    /// </summary>
    /// <returns>All contacts</returns>
    /// <response code="200">Returns all contacts</response>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var contacts = await _contactService.GetContactsAsync();
        var contactDTOs = _mapper.Map < IEnumerable < ContactDTO >> (contacts);
        return Ok(contactDTOs);
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
    public async Task<IActionResult> GetContactById([FromRoute]string id)
    {
        var contact = await _contactService.GetContactByIdAsync(id);
        var contactDTO = _mapper.Map<ContactDTO>(contact);
        return Ok(contactDTO);
    }
    
    /// <summary>
    /// Creates a contact
    /// </summary>
    /// <param name="contactDTO">contact model to create</param>
    /// <returns>A newly created contact</returns>
    /// <remarks>
    /// Sample request: 
    ///     Post /
    ///     {
    ///      "email": "ContactEmail@mail.com",
    ///      "firstName": "Contact Name",
    ///      "lastName": "Contact Lastname"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created contact</response>
    /// <response code="400">If the given model is not in the correct form</response>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddContact([FromBody] ContactDTO contactDTO)
    {
        var addedContact = await _contactService.AddContactAsync(_mapper.Map<Contact>(contactDTO));
        var addedContactDTO = _mapper.Map<ContactDTO>(addedContact);
        return Created(HttpContext.Request.GetEncodedUrl(), addedContactDTO);
    }

    /// <summary>
    /// Deletes a contact with a given Id
    /// </summary>
    /// <param name="id">Id of an contact to delete</param>
    /// <returns></returns>
    /// <response code="204">Deleted an contact, nothing to return</response>
    /// <response code="404">If the contact with the given Id could not have been found</response>
    /// <response code="422">If the contact with the given Id still has accounts</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _contactService.DeleteIncidentAsync(id);

        return NoContent();
    }
}
