using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnionProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentAuthorRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66f1e377-d7ff-4e86-ba90-38831241a614");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6d76777-a089-4907-8b06-14e41bccf4d9");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13c746ae-b555-4f91-aa77-93750c161f74", "00000000-0000-0000-0000-000000000000", "Member", "MEMBER" },
                    { "5e09ab77-5cf6-474c-aac9-85b50c52fa12", "00000000-0000-0000-0000-000000000000", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Authors_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Authors_AuthorId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13c746ae-b555-4f91-aa77-93750c161f74");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e09ab77-5cf6-474c-aac9-85b50c52fa12");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Comments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "66f1e377-d7ff-4e86-ba90-38831241a614", "00000000-0000-0000-0000-000000000000", "Member", "MEMBER" },
                    { "b6d76777-a089-4907-8b06-14e41bccf4d9", "00000000-0000-0000-0000-000000000000", "Admin", "ADMIN" }
                });
        }
    }
}
