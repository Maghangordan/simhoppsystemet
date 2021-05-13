using Microsoft.EntityFrameworkCore.Migrations;

namespace simhoppsystemet.Migrations
{
    public partial class hello5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FinalScore",
                table: "CompetitionCompetitor",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FinalScore",
                table: "CompetitionCompetitor",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
