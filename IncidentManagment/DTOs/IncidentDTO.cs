using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IncidentManagment.DTOs;

public record IncidentDTO([Required][property:JsonPropertyName("name")]string Name,
                          [Required][property:JsonPropertyName("description")]string Description,
                          [Required][property:JsonPropertyName("accountId")]string AccountId);
