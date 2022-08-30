using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace IncidentManagment.DTOs;

public record IncidentCreationDTO([Required][property:JsonPropertyName("accountId")]string AccountId,
                                  [Required]string cFirstName, 
                                  [Required]string cLastName, 
                                  [Required]string cEmail, 
                                  [Required]string incidentDescription);