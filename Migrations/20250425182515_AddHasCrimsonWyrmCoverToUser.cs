using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnDWebpage.Migrations
{
    /// <inheritdoc />
    public partial class AddHasCrimsonWyrmCoverToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCrimsonWyrmCover",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCrimsonWyrmCover",
                table: "AspNetUsers");
        }
    }
}
