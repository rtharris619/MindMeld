using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsernameAuditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Quotes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Quotes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Authors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Authors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Authors");
        }
    }
}
