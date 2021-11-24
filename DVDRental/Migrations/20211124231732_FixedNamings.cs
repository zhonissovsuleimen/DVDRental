using Microsoft.EntityFrameworkCore.Migrations;

namespace DVDRental.Migrations
{
    public partial class FixedNamings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Copy_Movies_movieId",
                table: "Copy");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Copy_copyId",
                table: "Rental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rental",
                table: "Rental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Copy",
                table: "Copy");

            migrationBuilder.RenameTable(
                name: "Rental",
                newName: "Rentals");

            migrationBuilder.RenameTable(
                name: "Copy",
                newName: "Copies");

            migrationBuilder.RenameIndex(
                name: "IX_Rental_copyId",
                table: "Rentals",
                newName: "IX_Rentals_copyId");

            migrationBuilder.RenameIndex(
                name: "IX_Copy_movieId",
                table: "Copies",
                newName: "IX_Copies_movieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rentals",
                table: "Rentals",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Copies",
                table: "Copies",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Copies_Movies_movieId",
                table: "Copies",
                column: "movieId",
                principalTable: "Movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Copies_copyId",
                table: "Rentals",
                column: "copyId",
                principalTable: "Copies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Copies_Movies_movieId",
                table: "Copies");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Copies_copyId",
                table: "Rentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rentals",
                table: "Rentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Copies",
                table: "Copies");

            migrationBuilder.RenameTable(
                name: "Rentals",
                newName: "Rental");

            migrationBuilder.RenameTable(
                name: "Copies",
                newName: "Copy");

            migrationBuilder.RenameIndex(
                name: "IX_Rentals_copyId",
                table: "Rental",
                newName: "IX_Rental_copyId");

            migrationBuilder.RenameIndex(
                name: "IX_Copies_movieId",
                table: "Copy",
                newName: "IX_Copy_movieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rental",
                table: "Rental",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Copy",
                table: "Copy",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Copy_Movies_movieId",
                table: "Copy",
                column: "movieId",
                principalTable: "Movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Copy_copyId",
                table: "Rental",
                column: "copyId",
                principalTable: "Copy",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
