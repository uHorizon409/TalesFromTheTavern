using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnDWebpage.Migrations
{
    /// <inheritdoc />
    public partial class AddBulletinVoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BulletinVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BulletinPostId = table.Column<int>(type: "int", nullable: false),
                    VotedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulletinVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BulletinVotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BulletinVotes_BulletinPosts_BulletinPostId",
                        column: x => x.BulletinPostId,
                        principalTable: "BulletinPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BulletinVotes_BulletinPostId",
                table: "BulletinVotes",
                column: "BulletinPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BulletinVotes_UserId",
                table: "BulletinVotes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulletinVotes");
        }
    }
}
