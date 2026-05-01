using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Migrations
{
    /// <inheritdoc />
    public partial class AddExtraProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductOption",
                table: "OrderProducts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductOption",
                table: "CartProducts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductOption",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ProductOption",
                table: "CartProducts");
        }
    }
}
