using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogsIRL_API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DateCollected", "DateCreated" },
                values: new object[] { new DateTime(2020, 5, 18, 14, 54, 35, 993, DateTimeKind.Local).AddTicks(8350), new DateTime(2020, 5, 18, 14, 54, 35, 978, DateTimeKind.Local).AddTicks(6240) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DateCollected", "DateCreated" },
                values: new object[] { new DateTime(2020, 5, 18, 12, 29, 29, 767, DateTimeKind.Local).AddTicks(333), new DateTime(2020, 5, 18, 12, 29, 29, 759, DateTimeKind.Local).AddTicks(6462) });
        }
    }
}
