using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AjusteEntidadesTipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "daily_balances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "daily_balances",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
