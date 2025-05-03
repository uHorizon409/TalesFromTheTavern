using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnDWebpage.Migrations
{
    /// <inheritdoc />
    public partial class AddHitPointsToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HitPoints",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HitPoints",
                table: "Characters");
        }
    }
}
