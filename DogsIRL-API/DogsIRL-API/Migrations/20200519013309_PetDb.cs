using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogsIRL_API.Migrations
{
    public partial class PetDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetCards",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ImageURL = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    Owner = table.Column<string>(nullable: true),
                    AgeYears = table.Column<int>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateCollected = table.Column<DateTime>(nullable: false),
                    GoodDog = table.Column<short>(nullable: false),
                    Floofiness = table.Column<short>(nullable: false),
                    Energy = table.Column<short>(nullable: false),
                    Snuggles = table.Column<short>(nullable: false),
                    Appetite = table.Column<short>(nullable: false),
                    Bravery = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetCards", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "PetCards",
                columns: new[] { "ID", "AgeYears", "Appetite", "Birthday", "Bravery", "DateCollected", "DateCreated", "Energy", "Floofiness", "GoodDog", "ImageURL", "Name", "Owner", "Sex", "Snuggles" },
                values: new object[] { 1, 2, (short)8, new DateTime(2018, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)9, new DateTime(2020, 5, 18, 18, 33, 8, 791, DateTimeKind.Local).AddTicks(1660), new DateTime(2020, 5, 18, 18, 33, 8, 778, DateTimeKind.Local).AddTicks(2790), (short)8, (short)1, (short)8, "", "Tucker", "andrewbc", "Male", (short)8 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetCards");
        }
    }
}
