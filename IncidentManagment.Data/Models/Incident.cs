using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagment.Data.Models;

public record Incident
{
    [Required]
    [Column("name")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    public Account Account { get; set; }

    public string AccountId { get; set; }
}

public class IncidentEntityTypeConfiguration : IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> builder)
    {
        builder.HasKey(i => i.Name);
        builder.Property(i => i.Name).HasDefaultValueSql("NEWID()");
        builder.HasOne(i => i.Account)
            .WithMany(a => a.Incidents)
            .HasForeignKey(i => i.AccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);
    }
}