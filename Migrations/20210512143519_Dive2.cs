using Microsoft.EntityFrameworkCore.Migrations;

namespace simhoppsystemet.Migrations
{
    public partial class Dive2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Difficulty",
                table: "DiveGroup",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Difficulty",
                table: "DiveGroup",
                type: "real",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
