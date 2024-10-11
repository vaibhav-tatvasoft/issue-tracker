using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.Migrations
{
    public partial class added_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "_categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[] { 1, "1", "something" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "_categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
