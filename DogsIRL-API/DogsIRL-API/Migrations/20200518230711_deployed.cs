using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogsIRL_API.Migrations
{
    public partial class deployed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DateCollected", "DateCreated" },
                values: new object[] { new DateTime(2020, 5, 18, 16, 7, 11, 610, DateTimeKind.Local).AddTicks(2620), new DateTime(2020, 5, 18, 16, 7, 11, 595, DateTimeKind.Local).AddTicks(7780) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DateCollected", "DateCreated" },
                values: new object[] { new DateTime(2020, 5, 18, 14, 54, 35, 993, DateTimeKind.Local).AddTicks(8350), new DateTime(2020, 5, 18, 14, 54, 35, 978, DateTimeKind.Local).AddTicks(6240) });
        }
    }
}
