using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManPowerSupplyAPI.Migrations
{
    public partial class AddedRTokenExpiry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "RefreshToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedOn",
                value: new DateTime(2021, 4, 25, 18, 6, 28, 883, DateTimeKind.Utc).AddTicks(5804));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires",
                table: "RefreshToken");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedOn",
                value: new DateTime(2021, 4, 25, 16, 51, 59, 102, DateTimeKind.Utc).AddTicks(9089));
        }
    }
}
