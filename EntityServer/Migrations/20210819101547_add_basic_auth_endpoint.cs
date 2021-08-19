using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityServer.Migrations
{
    public partial class add_basic_auth_endpoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Tid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sub = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aud = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Tid);
                });

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Password",
                value: "CE7A35F64C1EE87D13BA91420D48639506BA8AC2432B7B14858D4651FDD69631");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Password",
                value: "3BAC7D66695F0726CCA3969547ABA728C7DE6E4AD774379D7332DDD4F5E38265");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Password",
                value: "C64AC0577A66787781E7C3391DCD48BB2D74C84BEA6D239A51E9E8B4F034B6E2");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Password",
                value: "6899ED087724E0D9DFCD2C8208FCEC323295FBA436E02022539AA7D60AA18B73");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Password",
                value: "9A16F64870AEEB202F411964046006E294390F81EB740F54FE8F885D2A6D4086");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Password",
                value: "ce7a35f64c1ee87d13ba91420d48639506ba8ac2432b7b14858d4651fdd69631");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Password",
                value: "3bac7d66695f0726cca3969547aba728c7de6e4ad774379d7332ddd4f5e38265");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Password",
                value: "c64ac0577a66787781e7c3391dcd48bb2d74c84bea6d239a51e9e8b4f034b6e2");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Password",
                value: "6899ed087724e0d9dfcd2c8208fcec323295fba436e02022539aa7d60aa18b73");

            migrationBuilder.UpdateData(
                table: "UsersCreds",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Password",
                value: "9a16f64870aeeb202f411964046006e294390f81eb740f54fe8f885d2a6d4086");
        }
    }
}
