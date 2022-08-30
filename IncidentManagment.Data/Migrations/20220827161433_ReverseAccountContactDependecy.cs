using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentManagment.Data.Migrations
{
    public partial class ReverseAccountContactDependecy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Accounts_account_id",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_account_id",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "account_id",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "contact_id",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_contact_id",
                table: "Accounts",
                column: "contact_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Contacts_contact_id",
                table: "Accounts",
                column: "contact_id",
                principalTable: "Contacts",
                principalColumn: "email",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Contacts_contact_id",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_contact_id",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "contact_id",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "account_id",
                table: "Contacts",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_account_id",
                table: "Contacts",
                column: "account_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Accounts_account_id",
                table: "Contacts",
                column: "account_id",
                principalTable: "Accounts",
                principalColumn: "name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
