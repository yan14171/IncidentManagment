using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IncidentManagment.DTOs;

public record ContactDTO([Required][property:JsonPropertyName("email")]string Email,
                         [Required][property:JsonPropertyName("firstName")]string FirstName,
                         [Required][property:JsonPropertyName("lastName")]string LastName);

