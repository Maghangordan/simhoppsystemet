using Microsoft.EntityFrameworkCore.Migrations;

namespace simhoppsystemet.Data.Migrations
{
    public partial class testDive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dive",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionId = table.Column<int>(nullable: false),
                    CompetitorId = table.Column<int>(nullable: false),
                    DiveGroup = table.Column<int>(nullable: false),
                    PointsA = table.Column<double>(nullable: false),
                    PointsB = table.Column<double>(nullable: false),
                    PointsC = table.Column<double>(nullable: false),
                    FinalScore = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dive_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dive_Competitor_CompetitorId",
                        column: x => x.CompetitorId,
                        principalTable: "Competitor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dive_CompetitionId",
                table: "Dive",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Dive_CompetitorId",
                table: "Dive",
                column: "CompetitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dive");
        }
    }
}
