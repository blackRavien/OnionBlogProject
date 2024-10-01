using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnionProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeCommentIdsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Authors_AuthorId",
                table: "Comments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13c746ae-b555-4f91-aa77-93750c161f74");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e09ab77-5cf6-474c-aac9-85b50c52fa12");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "865b536a-dd85-40a4-b37a-4025db85f91e", "00000000-0000-0000-0000-000000000000", "Member", "MEMBER" },
                    { "b4afbe45-273b-4916-8add-dfa6cc843399", "00000000-0000-0000-0000-000000000000", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Authors_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Authors_AuthorId",
                table: "Comments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "865b536a-dd85-40a4-b37a-4025db85f91e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4afbe45-273b-4916-8add-dfa6cc843399");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13c746ae-b555-4f91-aa77-93750c161f74", "00000000-0000-0000-0000-000000000000", "Member", "MEMBER" },
                    { "5e09ab77-5cf6-474c-aac9-85b50c52fa12", "00000000-0000-0000-0000-000000000000", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Authors_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
