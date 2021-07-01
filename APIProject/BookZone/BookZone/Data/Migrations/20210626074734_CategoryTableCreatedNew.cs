using Microsoft.EntityFrameworkCore.Migrations;

namespace BookZone.Data.Migrations
{
    public partial class CategoryTableCreatedNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Genres",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_CategoryId",
                table: "Genres",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Categories_CategoryId",
                table: "Genres",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Categories_CategoryId",
                table: "Genres");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Genres_CategoryId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Genres");
        }
    }
}
