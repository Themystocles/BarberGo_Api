using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberGo.Migrations
{
    /// <inheritdoc />
    public partial class recoveryandexpiration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecoveryCode",
                table: "AppUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecoveryCodeExpiration",
                table: "AppUsers",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecoveryCode",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "RecoveryCodeExpiration",
                table: "AppUsers");
        }
    }
}
