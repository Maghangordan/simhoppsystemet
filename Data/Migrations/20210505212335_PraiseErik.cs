using Microsoft.EntityFrameworkCore.Migrations;

namespace simhoppsystemet.Data.Migrations
{
    public partial class PraiseErik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitionCompetitor_CompetitorId",
                table: "CompetitionCompetitor");

            migrationBuilder.DropColumn(
                name: "CompetitionsId",
                table: "Competitor");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitorId",
                table: "CompetitionCompetitor",
                column: "CompetitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitionCompetitor_CompetitorId",
                table: "CompetitionCompetitor");

            migrationBuilder.AddColumn<string>(
                name: "CompetitionsId",
                table: "Competitor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitorId",
                table: "CompetitionCompetitor",
                column: "CompetitorId",
                unique: true);
        }
    }
}
