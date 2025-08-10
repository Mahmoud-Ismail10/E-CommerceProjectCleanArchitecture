using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablesAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Payments",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SubAmount",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Deliveries",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SubAmount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Deliveries");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Payments",
                newName: "Amount");
        }
    }
}
