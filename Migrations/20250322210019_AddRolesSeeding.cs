using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResumeScreeningSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b37d218-e5f7-4a8b-ad23-a66c051c54ed", null, "Admin", "ADMIN" },
                    { "654011e8-7b7b-4f03-aabe-1e81aeb3369a", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b37d218-e5f7-4a8b-ad23-a66c051c54ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "654011e8-7b7b-4f03-aabe-1e81aeb3369a");
        }
    }
}
