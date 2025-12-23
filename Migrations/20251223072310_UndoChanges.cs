using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Migrations
{
    /// <inheritdoc />
    public partial class UndoChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartProducts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
