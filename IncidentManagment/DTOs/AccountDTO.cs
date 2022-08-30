using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IncidentManagment.DTOs;

public record AccountDTO([Required][property:JsonPropertyName("name")]string Name,
                         [Required][property:JsonPropertyName("contactId")]string ContactId);
