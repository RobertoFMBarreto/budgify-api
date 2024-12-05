using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgifyAPI.Transactions.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id_category = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_user = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_pkey", x => x.id_category);
                });

            migrationBuilder.CreateTable(
                name: "transaction_group",
                columns: table => new
                {
                    id_transaction_group = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    description = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    planned_amount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transaction_group_pkey", x => x.id_transaction_group);
                });

            migrationBuilder.CreateTable(
                name: "subcategory",
                columns: table => new
                {
                    id_subcategory = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    id_category = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_user = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("subcategory_pkey", x => x.id_subcategory);
                    table.ForeignKey(
                        name: "FKCategory",
                        column: x => x.id_category,
                        principalTable: "category",
                        principalColumn: "id_category");
                });

            migrationBuilder.CreateTable(
                name: "reocurring",
                columns: table => new
                {
                    id_reocurring = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    id_wallet = table.Column<Guid>(type: "uuid", nullable: false),
                    id_category = table.Column<Guid>(type: "uuid", nullable: true),
                    id_subcategory = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_yearly = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_monthly = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_weekly = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("reocurring_pkey", x => x.id_reocurring);
                    table.ForeignKey(
                        name: "FKCategory",
                        column: x => x.id_category,
                        principalTable: "category",
                        principalColumn: "id_category");
                    table.ForeignKey(
                        name: "FKSubcategory",
                        column: x => x.id_subcategory,
                        principalTable: "subcategory",
                        principalColumn: "id_subcategory");
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id_transaction = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    id_wallet = table.Column<Guid>(type: "uuid", nullable: false),
                    id_category = table.Column<Guid>(type: "uuid", nullable: true),
                    id_subcategory = table.Column<Guid>(type: "uuid", nullable: true),
                    id_transaction_group = table.Column<Guid>(type: "uuid", nullable: true),
                    id_reocurring = table.Column<Guid>(type: "uuid", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false),
                    is_planned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    latitude = table.Column<float>(type: "real", nullable: true),
                    longitue = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transactions_pkey", x => x.id_transaction);
                    table.ForeignKey(
                        name: "FKReocurring",
                        column: x => x.id_reocurring,
                        principalTable: "reocurring",
                        principalColumn: "id_reocurring");
                    table.ForeignKey(
                        name: "FKSubcategory",
                        column: x => x.id_subcategory,
                        principalTable: "subcategory",
                        principalColumn: "id_subcategory");
                    table.ForeignKey(
                        name: "FKTransactiongroup",
                        column: x => x.id_transaction_group,
                        principalTable: "transaction_group",
                        principalColumn: "id_transaction_group");
                    table.ForeignKey(
                        name: "FkCategory",
                        column: x => x.id_category,
                        principalTable: "category",
                        principalColumn: "id_category");
                });

            migrationBuilder.CreateIndex(
                name: "IX_reocurring_id_category",
                table: "reocurring",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_reocurring_id_subcategory",
                table: "reocurring",
                column: "id_subcategory");

            migrationBuilder.CreateIndex(
                name: "IX_subcategory_id_category",
                table: "subcategory",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_id_category",
                table: "transactions",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_id_reocurring",
                table: "transactions",
                column: "id_reocurring");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_id_subcategory",
                table: "transactions",
                column: "id_subcategory");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_id_transaction_group",
                table: "transactions",
                column: "id_transaction_group");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "reocurring");

            migrationBuilder.DropTable(
                name: "transaction_group");

            migrationBuilder.DropTable(
                name: "subcategory");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
