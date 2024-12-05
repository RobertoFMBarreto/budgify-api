using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgifyAPI.Wallets.CA.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wallet",
                columns: table => new
                {
                    id_wallet = table.Column<Guid>(type: "uuid", nullable: false),
                    id_user = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    requisition = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    agreement_days = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wallet");
        }
    }
}
