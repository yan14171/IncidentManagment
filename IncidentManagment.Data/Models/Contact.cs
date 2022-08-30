using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagment.Data.Models;
public record Contact
{
    [Required]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("first_name")]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [Column("last_name")]
    [MaxLength(50)]
    public string LastName { get; set; }

    public List<Account> Accounts { get; set; } = new List<Account>();
}
public class ContactEntityTypeConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(i => i.Email);
        //builder.Property(i => i.Name).HasDefaultValueSql("NEWID()");
    }
}
