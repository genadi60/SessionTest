using Microsoft.EntityFrameworkCore.Migrations;

namespace SessionTest.Migrations
{
    public partial class AddedTempUnitToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TempUnit",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempUnit",
                table: "Products");
        }
    }
}
