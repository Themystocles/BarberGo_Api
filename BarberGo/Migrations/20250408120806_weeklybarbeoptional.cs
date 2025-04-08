using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberGo.Migrations
{
    /// <inheritdoc />
    public partial class weeklybarbeoptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weeklySchedules_AppUsers_BarberId",
                table: "weeklySchedules");

            migrationBuilder.AlterColumn<int>(
                name: "BarberId",
                table: "weeklySchedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_weeklySchedules_AppUsers_BarberId",
                table: "weeklySchedules",
                column: "BarberId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weeklySchedules_AppUsers_BarberId",
                table: "weeklySchedules");

            migrationBuilder.AlterColumn<int>(
                name: "BarberId",
                table: "weeklySchedules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_weeklySchedules_AppUsers_BarberId",
                table: "weeklySchedules",
                column: "BarberId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
