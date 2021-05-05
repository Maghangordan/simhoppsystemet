using Microsoft.EntityFrameworkCore.Migrations;

namespace simhoppsystemet.Data.Migrations
{
    public partial class EnNyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitionCompetitor_CompetitionId",
                table: "CompetitionCompetitor");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionCompetitor_CompetitorId",
                table: "CompetitionCompetitor");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitionId",
                table: "CompetitionCompetitor",
                column: "CompetitionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitorId",
                table: "CompetitionCompetitor",
                column: "CompetitorId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitionCompetitor_CompetitionId",
                table: "CompetitionCompetitor");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionCompetitor_CompetitorId",
                table: "CompetitionCompetitor");

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
