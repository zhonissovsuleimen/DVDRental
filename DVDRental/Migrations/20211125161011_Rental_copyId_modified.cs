using Microsoft.EntityFrameworkCore.Migrations;

namespace DVDRental.Migrations
{
    public partial class Rental_copyId_modified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "Rentals",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Rentals",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
