using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogsIRL_API.Migrations.ApplicationDb
{
    public partial class AddedCollectionTrackingToPetCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Collections",
                table: "PetCards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DateCollected", "DateCreated" },
                values: new object[] { new DateTime(2020, 11, 27, 14, 8, 47, 761, DateTimeKind.Local).AddTicks(6328), new DateTime(2020, 11, 27, 14, 8, 47, 758, DateTimeKind.Local).AddTicks(1641) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collections",
                table: "PetCards");

            migrationBuilder.UpdateData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DateCollected", "DateCreated" },
                values: new object[] { new DateTime(2020, 8, 11, 11, 26, 56, 419, DateTimeKind.Local).AddTicks(162), new DateTime(2020, 8, 11, 11, 26, 56, 417, DateTimeKind.Local).AddTicks(902) });
        }
    }
}
