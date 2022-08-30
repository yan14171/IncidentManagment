using IncidentManagment.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagment.Logic.Interfaces;
public interface IIncidentService
{
    Task<IEnumerable<Incident>> GetIncidentsAsync();
    Task<Incident> GetIncidentById(string id);
    Task<Incident> CreateIncidentAsync(Incident incident, string accountId);
    Task<Incident> DeleteIncidentAsync(string id);
}
