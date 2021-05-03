using Microsoft.EntityFrameworkCore.Migrations;

namespace simhoppsystemet.Data.Migrations
{
    public partial class ourfatherwhoarthinheaven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionCompetitor",
                columns: table => new
                {
                    CompetitionCompetitorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionId = table.Column<int>(nullable: false),
                    CompetitorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionCompetitor", x => x.CompetitionCompetitorId);
                    table.ForeignKey(
                        name: "FK_CompetitionCompetitor_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionCompetitor_Competitor_CompetitorId",
                        column: x => x.CompetitorId,
                        principalTable: "Competitor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitionId",
                table: "CompetitionCompetitor",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitorId",
                table: "CompetitionCompetitor",
                column: "CompetitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionCompetitor");
        }
    }
}
