using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloggerBlazorServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArticleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_AuthorId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Articles",
                newName: "LastModifiedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                newName: "IX_Articles_LastModifiedBy");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatedBy",
                table: "Articles",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_CreatedBy",
                table: "Articles",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_LastModifiedBy",
                table: "Articles",
                column: "LastModifiedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_CreatedBy",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_LastModifiedBy",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CreatedBy",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Articles",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_LastModifiedBy",
                table: "Articles",
                newName: "IX_Articles_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_AuthorId",
                table: "Articles",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
