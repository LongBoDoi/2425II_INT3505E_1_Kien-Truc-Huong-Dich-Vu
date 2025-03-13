using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PastebinBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddPasteExposure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Exposure",
                table: "Pastes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exposure",
                table: "Pastes");
        }
    }
}
