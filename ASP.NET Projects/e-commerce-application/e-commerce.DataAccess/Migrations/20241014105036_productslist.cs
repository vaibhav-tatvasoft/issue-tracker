using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.DataAccess.Migrations
{
    public partial class productslist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[] { 1, "Robert C. Martin", "A Handbook of Agile Software Craftsmanship", "9780132350884", 50.0, 45.0, 35.0, 40.0, "Clean Code" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[] { 2, "Andrew Hunt, David Thomas", "Your Journey to Mastery", "9780135957059", 60.0, 55.0, 45.0, 50.0, "The Pragmatic Programmer" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[] { 3, "Martin Fowler", "Improving the Design of Existing Code", "9780201485677", 70.0, 65.0, 55.0, 60.0, "Refactoring" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
