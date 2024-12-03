using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChattingApplication.DataAccess.Migrations
{
    public partial class modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    groupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    groupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lastMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastMessageTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    groupAvatar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_createdBy",
                        column: x => x.createdBy,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    groupsid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    membersid = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.groupsid, x.membersid });
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_groupsid",
                        column: x => x.groupsid,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Users_membersid",
                        column: x => x.membersid,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    timestamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    to = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    from = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    groupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    groupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isRead = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_Messages_Groups_groupId",
                        column: x => x.groupId,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_from",
                        column: x => x.from,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_to",
                        column: x => x.to,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_createdBy",
                table: "Groups",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_membersid",
                table: "GroupUser",
                column: "membersid");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_from",
                table: "Messages",
                column: "from");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_groupId",
                table: "Messages",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_to",
                table: "Messages",
                column: "to");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
