using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChattingApplication.DataAccess.Migrations
{
    public partial class addMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    groupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    groupDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lastMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastMessageTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    groupAvatar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    timestamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    groupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isRead = table.Column<bool>(type: "bit", nullable: true),
                    Groupid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_Messages_Groups_Groupid",
                        column: x => x.Groupid,
                        principalTable: "Groups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Groupid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Groupid1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Groups_Groupid",
                        column: x => x.Groupid,
                        principalTable: "Groups",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Users_Groups_Groupid1",
                        column: x => x.Groupid1,
                        principalTable: "Groups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_createdBy",
                table: "Groups",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Groupid",
                table: "Messages",
                column: "Groupid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Groupid",
                table: "Users",
                column: "Groupid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Groupid1",
                table: "Users",
                column: "Groupid1");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_createdBy",
                table: "Groups",
                column: "createdBy",
                principalTable: "Users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_createdBy",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
