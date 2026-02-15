using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCycle.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataStaticHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$8bHsTyPgE.P/CB7rCNvgbeW5/Nw5iH1RwaOkKogQLdJAnlFAGAyaa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$Q.dlc4NyfjDilVOVb.YPDuABOiwGAunoMHBYm0bIb8GQ2T7.p3u3W");
        }
    }
}
