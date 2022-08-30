using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagment.Data.Models;
public record Account
{
    [Required]
    [Column("name")]
    [MaxLength(50)]
    public string Name { get; set; }

    public Contact Contact { get; set; }

    [Column("contact_id")]
    public string ContactId { get; set; }

    public List<Incident>? Incidents { get; set; }
}

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(i => i.Name);
        //builder.Property(i => i.Name).HasDefaultValueSql("NEWID()");


        builder.HasOne(c => c.Contact)
            .WithMany(a => a.Accounts)
            .HasForeignKey(c => c.ContactId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);
    }
}
