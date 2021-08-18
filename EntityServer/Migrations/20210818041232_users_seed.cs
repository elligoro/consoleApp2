using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityServer.Migrations
{
    public partial class users_seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersCreds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCreds", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "UsersCreds",
                columns: new[] { "Id", "Email", "Password", "Username" },
                values: new object[,]
                {
                    { 1L, null, "ce7a35f64c1ee87d13ba91420d48639506ba8ac2432b7b14858d4651fdd69631", "simon" },
                    { 2L, null, "3bac7d66695f0726cca3969547aba728c7de6e4ad774379d7332ddd4f5e38265", "lake" },
                    { 3L, null, "c64ac0577a66787781e7c3391dcd48bb2d74c84bea6d239a51e9e8b4f034b6e2", "palmer" },
                    { 4L, null, "6899ed087724e0d9dfcd2c8208fcec323295fba436e02022539aa7d60aa18b73", "george" },
                    { 5L, null, "9a16f64870aeeb202f411964046006e294390f81eb740f54fe8f885d2a6d4086", "barak" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersCreds_Username",
                table: "UsersCreds",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersCreds");
        }
    }
}
