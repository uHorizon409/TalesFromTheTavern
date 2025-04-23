using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnDWebpage.Migrations
{
    /// <inheritdoc />
    public partial class MakePostIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BulletinVotes_BulletinPosts_BulletinPostId",
                table: "BulletinVotes");

            migrationBuilder.AlterColumn<float>(
                name: "Y",
                table: "BulletinVotes",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "X",
                table: "BulletinVotes",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BulletinPostId",
                table: "BulletinVotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BulletinVotes_BulletinPosts_BulletinPostId",
                table: "BulletinVotes",
                column: "BulletinPostId",
                principalTable: "BulletinPosts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BulletinVotes_BulletinPosts_BulletinPostId",
                table: "BulletinVotes");

            migrationBuilder.AlterColumn<int>(
                name: "Y",
                table: "BulletinVotes",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "X",
                table: "BulletinVotes",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "BulletinPostId",
                table: "BulletinVotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BulletinVotes_BulletinPosts_BulletinPostId",
                table: "BulletinVotes",
                column: "BulletinPostId",
                principalTable: "BulletinPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
