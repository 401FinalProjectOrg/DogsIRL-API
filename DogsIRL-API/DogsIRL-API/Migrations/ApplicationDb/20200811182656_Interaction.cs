using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogsIRL_API.Migrations.ApplicationDb
{
    public partial class Interaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpeningLine = table.Column<string>(nullable: true),
                    OpeningLineOther = table.Column<string>(nullable: true),
                    ConversationLine = table.Column<string>(nullable: true),
                    GoodbyeLine = table.Column<string>(nullable: true),
                    GoodbyeLineOther = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DateCollected", "DateCreated" },
                values: new object[] { new DateTime(2020, 8, 11, 11, 26, 56, 419, DateTimeKind.Local).AddTicks(162), new DateTime(2020, 8, 11, 11, 26, 56, 417, DateTimeKind.Local).AddTicks(902) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interactions");

            migrationBuilder.UpdateData(
                table: "PetCards",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DateCollected", "DateCreated" },
                values: new object[] { new DateTime(2020, 6, 26, 12, 20, 44, 447, DateTimeKind.Local).AddTicks(2230), new DateTime(2020, 6, 26, 12, 20, 44, 443, DateTimeKind.Local).AddTicks(2700) });
        }
    }
}
