﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PastebinBackend.Migrations
{
    /// <inheritdoc />
    public partial class AppPasteName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasteName",
                table: "Pastes",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasteName",
                table: "Pastes");
        }
    }
}
