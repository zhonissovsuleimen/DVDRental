using Microsoft.EntityFrameworkCore.Migrations;

namespace DVDRental.Migrations
{
    public partial class RentalsModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "Rental",
                newName: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Copy_movieId",
                table: "Copy",
                column: "movieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Copy_Movies_movieId",
                table: "Copy",
                column: "movieId",
                principalTable: "Movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Copy_Movies_movieId",
                table: "Copy");

            migrationBuilder.DropIndex(
                name: "IX_Copy_movieId",
                table: "Copy");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Rental",
                newName: "movieId");
        }
    }
}
