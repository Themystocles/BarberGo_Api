using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removerdto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AppUsers_AppUserId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_BarberDto_BarberId",
                table: "Feedback");

            migrationBuilder.DropTable(
                name: "BarberDto");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AppUsers_AppUserId",
                table: "Feedback",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AppUsers_BarberId",
                table: "Feedback",
                column: "BarberId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AppUsers_AppUserId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AppUsers_BarberId",
                table: "Feedback");

            migrationBuilder.CreateTable(
                name: "BarberDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarberDto", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AppUsers_AppUserId",
                table: "Feedback",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_BarberDto_BarberId",
                table: "Feedback",
                column: "BarberId",
                principalTable: "BarberDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
