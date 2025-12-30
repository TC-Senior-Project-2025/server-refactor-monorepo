using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCountriesAndCommanderiesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commanderies");

            migrationBuilder.DropTable(
                name: "countries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    efficiency = table.Column<int>(type: "integer", nullable: false),
                    history = table.Column<string>(type: "text", nullable: true),
                    history_summary = table.Column<string>(type: "text", nullable: true),
                    manpower = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    prestige = table.Column<int>(type: "integer", nullable: false),
                    stability = table.Column<int>(type: "integer", nullable: false),
                    treasury = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "commanderies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    defense = table.Column<int>(type: "integer", nullable: false),
                    history = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    population = table.Column<int>(type: "integer", nullable: false),
                    unrest = table.Column<int>(type: "integer", nullable: false),
                    wealth = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commanderies", x => x.id);
                    table.ForeignKey(
                        name: "FK_commanderies_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commanderies_country_id",
                table: "commanderies",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_countries_code",
                table: "countries",
                column: "code",
                unique: true);
        }
    }
}
