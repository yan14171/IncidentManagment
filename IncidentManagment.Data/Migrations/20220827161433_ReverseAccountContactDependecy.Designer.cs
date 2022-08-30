﻿// <auto-generated />
using IncidentManagment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IncidentManagment.Data.Migrations
{
    [DbContext(typeof(IncidentContext))]
    [Migration("20220827161433_ReverseAccountContactDependecy")]
    partial class ReverseAccountContactDependecy
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("IncidentManagment.Data.Models.Account", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("ContactId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("contact_id");

                    b.HasKey("Name");

                    b.HasIndex("ContactId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("IncidentManagment.Data.Models.Contact", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("last_name");

                    b.HasKey("Email");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("IncidentManagment.Data.Models.Incident", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("name")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Name");

                    b.HasIndex("AccountId");

                    b.ToTable("Incidents");
                });

            modelBuilder.Entity("IncidentManagment.Data.Models.Account", b =>
                {
                    b.HasOne("IncidentManagment.Data.Models.Contact", "Contact")
                        .WithMany("Accounts")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("IncidentManagment.Data.Models.Incident", b =>
                {
                    b.HasOne("IncidentManagment.Data.Models.Account", "Account")
                        .WithMany("Incidents")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("IncidentManagment.Data.Models.Account", b =>
                {
                    b.Navigation("Incidents");
                });

            modelBuilder.Entity("IncidentManagment.Data.Models.Contact", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
