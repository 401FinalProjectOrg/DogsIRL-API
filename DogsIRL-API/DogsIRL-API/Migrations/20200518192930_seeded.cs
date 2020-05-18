using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogsIRL_API.Migrations
{
    public partial class seeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PetCards",
                columns: new[] { "ID", "AgeYears", "Appetite", "Birthday", "Bravery", "DateCollected", "DateCreated", "Energy", "Floofiness", "GoodDog", "ImageURL", "Name", "Owner", "Sex", "Snuggles" },
                values: new object[] { 1, 2, (short)8, new DateTime(2018, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)9, new DateTime(2020, 5, 18, 12, 29, 29, 767, DateTimeKind.Local).AddTicks(333), new DateTime(2020, 5, 18, 12, 29, 29, 759, DateTimeKind.Local).AddTicks(6462), (short)8, (short)1, (short)8, "", "Tucker", "andrewbc", "Male", (short)8 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
