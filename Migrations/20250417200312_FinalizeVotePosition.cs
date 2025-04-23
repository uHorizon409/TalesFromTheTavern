using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnDWebpage.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeVotePosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "BulletinVotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "BulletinVotes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "BulletinVotes");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "BulletinVotes");
        }
    }
}
