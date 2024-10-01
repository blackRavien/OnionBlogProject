using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnionProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentityCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "865b536a-dd85-40a4-b37a-4025db85f91e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4afbe45-273b-4916-8add-dfa6cc843399");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4b36de38-b32d-44f8-a7a8-3ce478c8240a", "00000000-0000-0000-0000-000000000000", "Admin", "ADMIN" },
                    { "56f08e22-35e6-4c79-b270-d7d21c03e513", "00000000-0000-0000-0000-000000000000", "Member", "MEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b36de38-b32d-44f8-a7a8-3ce478c8240a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56f08e22-35e6-4c79-b270-d7d21c03e513");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "865b536a-dd85-40a4-b37a-4025db85f91e", "00000000-0000-0000-0000-000000000000", "Member", "MEMBER" },
                    { "b4afbe45-273b-4916-8add-dfa6cc843399", "00000000-0000-0000-0000-000000000000", "Admin", "ADMIN" }
                });
        }
    }
}
