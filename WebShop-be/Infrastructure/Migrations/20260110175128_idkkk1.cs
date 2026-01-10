using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class idkkk1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorUrl",
                table: "PaymentInitializationRequests");

            migrationBuilder.DropColumn(
                name: "FailedUrl",
                table: "PaymentInitializationRequests");

            migrationBuilder.DropColumn(
                name: "SuccessUrl",
                table: "PaymentInitializationRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorUrl",
                table: "PaymentInitializationRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FailedUrl",
                table: "PaymentInitializationRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SuccessUrl",
                table: "PaymentInitializationRequests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
