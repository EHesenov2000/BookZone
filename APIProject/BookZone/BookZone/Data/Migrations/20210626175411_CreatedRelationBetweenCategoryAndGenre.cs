using Microsoft.EntityFrameworkCore.Migrations;

namespace BookZone.Data.Migrations
{
    public partial class CreatedRelationBetweenCategoryAndGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Categories_CategoryId",
                table: "Genres");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Genres",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Categories_CategoryId",
                table: "Genres",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Categories_CategoryId",
                table: "Genres");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Genres",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Categories_CategoryId",
                table: "Genres",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
