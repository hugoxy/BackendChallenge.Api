using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendChallenge.MicroServices.Migrations
{
    /// <inheritdoc />
    public partial class addMoreUserVars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mail",
                schema: "BackendChallenge",
                table: "Client",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "BackendChallenge",
                table: "Client",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                schema: "BackendChallenge",
                table: "Client",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mail",
                schema: "BackendChallenge",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Password",
                schema: "BackendChallenge",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "BackendChallenge",
                table: "Client");
        }
    }
}
