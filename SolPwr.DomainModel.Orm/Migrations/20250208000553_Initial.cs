using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnionDlx.SolPwr.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "spa");

            migrationBuilder.CreateTable(
                name: "PowerPlants",
                schema: "spa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlantName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    UtcInstallDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    PowerCapacity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerPlants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GenerationHistory",
                schema: "spa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PowerPlantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UtcTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PowerGenerated = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenerationHistory_PowerPlants_PowerPlantId",
                        column: x => x.PowerPlantId,
                        principalSchema: "spa",
                        principalTable: "PowerPlants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenerationHistory_PowerPlantId",
                schema: "spa",
                table: "GenerationHistory",
                column: "PowerPlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenerationHistory",
                schema: "spa");

            migrationBuilder.DropTable(
                name: "PowerPlants",
                schema: "spa");
        }
    }
}
