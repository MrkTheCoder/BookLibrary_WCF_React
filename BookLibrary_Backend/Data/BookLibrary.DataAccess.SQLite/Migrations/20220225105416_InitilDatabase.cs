using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrary.DataAccess.SQLite.Migrations
{
    public partial class InitilDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BookSchema");

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "BookSchema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ISBN = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Title = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "BookSchema",
                table: "Book",
                columns: new[] { "Id", "ISBN", "Title" },
                values: new object[] { 1, "101-11113502222", "Clean Code: Best Practices" });

            migrationBuilder.InsertData(
                schema: "BookSchema",
                table: "Book",
                columns: new[] { "Id", "ISBN", "Title" },
                values: new object[] { 2, "101-22258489522", "Learn Design Patterns in R" });

            migrationBuilder.InsertData(
                schema: "BookSchema",
                table: "Book",
                columns: new[] { "Id", "ISBN", "Title" },
                values: new object[] { 3, "131-33333889820", "A Red Apple Far From Tree" });

            migrationBuilder.InsertData(
                schema: "BookSchema",
                table: "Book",
                columns: new[] { "Id", "ISBN", "Title" },
                values: new object[] { 4, "141-44444886529", "How to Fix Broken Chair" });

            migrationBuilder.InsertData(
                schema: "BookSchema",
                table: "Book",
                columns: new[] { "Id", "ISBN", "Title" },
                values: new object[] { 5, "131-55554825527", "Do We Need Another One?" });

            migrationBuilder.InsertData(
                schema: "BookSchema",
                table: "Book",
                columns: new[] { "Id", "ISBN", "Title" },
                values: new object[] { 6, "131-66684885431", "Hello World!" });

            migrationBuilder.InsertData(
                schema: "BookSchema",
                table: "Book",
                columns: new[] { "Id", "ISBN", "Title" },
                values: new object[] { 7, "121-77784885431", "Java va JavaScript" });

            migrationBuilder.InsertData(
                schema: "BookSchema",
                table: "Book",
                columns: new[] { "Id", "ISBN", "Title" },
                values: new object[] { 8, "101-88645218900", "WPF in C# v10" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book",
                schema: "BookSchema");
        }
    }
}
