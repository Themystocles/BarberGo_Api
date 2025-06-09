using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberGo.Migrations
{
    /// <inheritdoc />
    public partial class boleanIsmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMaster",
                table: "AppUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMaster",
                table: "AppUsers");
        }
    }
}
