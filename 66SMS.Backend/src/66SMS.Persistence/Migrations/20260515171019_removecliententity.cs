using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _66SMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removecliententity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Client_ClientId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_ClientId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "RefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "RefreshToken",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "ClientId", "ClientName", "Description", "IsActive" },
                values: new object[,]
                {
                    { "android", "Android Client", "Android mobile app", true },
                    { "ios", "iOS Client", "iOS mobile app", true },
                    { "web", "Web Client", "Web browser clients", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ClientId",
                table: "RefreshToken",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Client_ClientId",
                table: "RefreshToken",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
