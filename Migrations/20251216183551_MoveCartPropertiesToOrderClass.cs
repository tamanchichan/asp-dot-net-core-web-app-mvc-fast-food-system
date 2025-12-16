using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Migrations
{
    /// <inheritdoc />
    public partial class MoveCartPropertiesToOrderClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_CustomerId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CustomerAddress",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CustomerPhoneNumber",
                table: "Carts");

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalCharge",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalCharge",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddress",
                table: "Carts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Carts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Carts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhoneNumber",
                table: "Carts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_CustomerId",
                table: "Carts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
