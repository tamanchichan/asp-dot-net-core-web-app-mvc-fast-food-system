using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Migrations
{
    /// <inheritdoc />
    public partial class GetNewTableFromInstructionsToInstruction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Instruction",
                table: "OrderProducts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instruction",
                table: "CartProducts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instruction",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "Instruction",
                table: "CartProducts");
        }
    }
}
