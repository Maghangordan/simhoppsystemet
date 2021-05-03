using Microsoft.EntityFrameworkCore.Migrations;

namespace simhoppsystemet.Data.Migrations
{
    public partial class something : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionCompetitor");

            migrationBuilder.AddColumn<int>(
                name: "CompetitorsId",
                table: "Competition",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Competition_CompetitorsId",
                table: "Competition",
                column: "CompetitorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competition_Competitor_CompetitorsId",
                table: "Competition",
                column: "CompetitorsId",
                principalTable: "Competitor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competition_Competitor_CompetitorsId",
                table: "Competition");

            migrationBuilder.DropIndex(
                name: "IX_Competition_CompetitorsId",
                table: "Competition");

            migrationBuilder.DropColumn(
                name: "CompetitorsId",
                table: "Competition");

            migrationBuilder.CreateTable(
                name: "CompetitionCompetitor",
                columns: table => new
                {
                    CompetitionCompetitorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    CompetitorId = table.Column<int>(type: "int", nullable: false)
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
    }
}
