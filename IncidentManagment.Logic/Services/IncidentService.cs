using IncidentManagment.Data.Models;
using IncidentManagment.Logic.Interfaces;
using IncidentManagment.Data.Exceptions;
using IncidentManagment.Data;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagment.Logic.Services;

public class IncidentService : IIncidentService
{
    private readonly IncidentContext _incidentContext;

    public IncidentService(IncidentContext incidentContext)
    {
        _incidentContext = incidentContext;
    }
    public async Task<IEnumerable<Incident>> GetIncidentsAsync()
    {
        return await _incidentContext.Incidents.ToListAsync();
    }

    public async Task<Incident> CreateIncidentAsync(Incident incident, string accountId)
    {
        if (await _incidentContext.Accounts.FindAsync(accountId) is null)
            throw new ValueNotFoundException("Account with given account id could not have been found", "accountId");

        incident.AccountId = accountId;
        _incidentContext.Entry(incident).State = EntityState.Added;

        await _incidentContext.SaveChangesAsync();

        return incident;
    }

    public async Task<Incident> GetIncidentById(string id)
    {
        var foundIncident = await _incidentContext.Incidents.FindAsync(id);

        if (foundIncident is null)
            throw new NoContentException("Incident with given id could not have been found", "id");

        return foundIncident;
    }
}
