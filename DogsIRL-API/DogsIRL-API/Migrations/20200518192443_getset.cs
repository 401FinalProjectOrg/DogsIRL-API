using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogsIRL_API.Migrations
{
    public partial class getset : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetCards");
        }
    }
}
