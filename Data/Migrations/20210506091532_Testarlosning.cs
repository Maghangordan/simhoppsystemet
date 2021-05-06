using Microsoft.EntityFrameworkCore.Migrations;

namespace simhoppsystemet.Data.Migrations
{
    public partial class Testarlosning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitionCompetitor_CompetitionId",
                table: "CompetitionCompetitor");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitionId",
                table: "CompetitionCompetitor",
                column: "CompetitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitionCompetitor_CompetitionId",
                table: "CompetitionCompetitor");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitionId",
                table: "CompetitionCompetitor",
                column: "CompetitionId",
                unique: true);
        }
    }
}
