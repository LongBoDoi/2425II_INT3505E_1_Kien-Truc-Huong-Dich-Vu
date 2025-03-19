using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PastebinBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnalyticTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analytics_Pastes_PasteId",
                table: "Analytics");

            migrationBuilder.DropIndex(
                name: "IX_Analytics_PasteId",
                table: "Analytics");

            migrationBuilder.DropColumn(
                name: "PasteId",
                table: "Analytics");

            migrationBuilder.DropColumn(
                name: "ViewsCount",
                table: "Analytics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PasteId",
                table: "Analytics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "ViewsCount",
                table: "Analytics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Analytics_PasteId",
                table: "Analytics",
                column: "PasteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Analytics_Pastes_PasteId",
                table: "Analytics",
                column: "PasteId",
                principalTable: "Pastes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
